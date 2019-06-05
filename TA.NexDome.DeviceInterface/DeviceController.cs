// Copyright © Tigra Astronomy, all rights reserved.
using JetBrains.Annotations;
using NLog.Fluent;
using PostSharp.Patterns.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface
    {
    [NotifyPropertyChanged]
    public class DeviceController : INotifyPropertyChanged, IDisposable
        {
        [NotNull] private readonly ICommunicationChannel channel;
        private readonly DeviceControllerOptions configuration;
        [NotNull] private readonly List<IDisposable> disposableSubscriptions = new List<IDisposable>();
        [NotNull] private readonly ControllerStateMachine stateMachine;
        [NotNull] private readonly ControllerStatusFactory statusFactory;
        private bool disposed = false;

        public DeviceController(ICommunicationChannel channel, ControllerStatusFactory factory,
            ControllerStateMachine machine, DeviceControllerOptions configuration)
            {
            this.channel = channel;
            statusFactory = factory;
            stateMachine = machine;
            this.configuration = configuration;
            }

        public Octet UserPins => stateMachine.UserPins;

        public int AzimuthEncoderPosition => stateMachine.AzimuthEncoderPosition;

        public float AzimuthDegrees => AzimuthEncoderPosition * DegreesPerTick;

        private float DegreesPerTick => 360f / stateMachine.DomeCircumference;

        /// <summary>
        ///     <c>true</c> if any part of the building is moving.
        /// </summary>
        public bool IsMoving => stateMachine.IsMoving;

        [IgnoreAutoChangeNotification]
        public bool IsConnected => channel.IsOpen;

        public bool AzimuthMotorActive => stateMachine.AzimuthMotorActive;

        public RotationDirection AzimuthDirection => stateMachine.AzimuthDirection;

        public ShutterDirection ShutterMovementDirection => stateMachine.ShutterMovementDirection;

        public int ShutterMotorCurrent => stateMachine.ShutterMotorCurrent;

        public bool ShutterMotorActive => stateMachine.ShutterMotorActive;

        public SensorState ShutterLimitSwitches => stateMachine.ShutterLimitSwitches;

        public ShutterDisposition ShutterDisposition => stateMachine.ShutterDisposition;

        public int ShutterPercentOpen =>
            (int)((float)stateMachine.ShutterStepPosition / (float)stateMachine.ShutterLimitOfTravel * 100f);

        public bool AtHome => stateMachine.AtHome;

        public ShutterLinkState ShutterLinkState => stateMachine.ShutterLinkState;

        public event PropertyChangedEventHandler PropertyChanged;

        public void RotateToHomePosition() => stateMachine.RotateToHomePosition();

        public void Open(bool performOnConnectActions = true)
            {
            SubscribeControllerEvents();
            channel.Open();
            stateMachine.Initialize(new TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState(stateMachine));
            stateMachine.Initialize(new TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState(stateMachine));
            stateMachine.WaitForReady(TimeSpan.FromSeconds(5));
            if (performOnConnectActions && configuration.PerformShutterRecovery) PerformShutterRecovery();
            }

        /// <summary>
        ///     Tries to establish a known shutter condition at startup.
        ///     Assumes that a valid status packet has already been received.
        /// </summary>
        /// <exception cref="TimeoutException">
        ///     Thrown if shutter recovery does not complete within the
        ///     allotted time.
        /// </exception>
        private void PerformShutterRecovery()
            {
            Log.Debug()
                .Message("Shutter recovery heuristic.")
                .Property(nameof(ShutterLimitSwitches), ShutterLimitSwitches)
                .Write();
            if (ShutterLimitSwitches == SensorState.Indeterminate)
                {
                Log.Info()
                    .Message("Shutter position is indeterminate, attempting to close the shutter.")
                    .Write();
                stateMachine.CloseShutter();
                stateMachine.WaitForReady(configuration.MaximumFullRotationTime +
                                          configuration.MaximumShutterCloseTime);
                }
            Log.Debug("Shutter recovery heuristic finished");
            }


        /// <summary>
        /// Uses a variety of pattern matches over the input character stream (defined in 
        /// <see cref="ObservableExtensions"/>) to generate a number of observable sequences. For
        /// example, there is an observable sequence of azimuth positions. Each of these sequences
        /// is then subscribed to an <c>*OnNext()</c> handler method, which catches any exceptions
        /// generated and passes the sequence elements on to the state machine. Thus the state
        /// machine "reacts" to the serial input. Each sequence is assumed to only produce correct
        /// and valid values. Each of the sequences produced is added to
        /// <see cref="disposableSubscriptions"/> for later disposal and cleanup.
        /// </summary>
        private void SubscribeControllerEvents()
            {
            SubscribeAzimuthEncoderTicks();
            SubscribeShutterPositionTicks();
            SubscribeRotationDirection();
            SubscribeShutterDirection();
            SubscribeStatusUpdates();
            SubscribeLinkStateUpdates();
            }

        private void SubscribeLinkStateUpdates()
            {
            var linkStateUpdates = channel.ObservableReceivedCharacters.LinkStatusUpdates();
            var subscription = linkStateUpdates.Subscribe(LinkStateUpdateOnNext,
                ex => throw new InvalidOperationException(
                    "Shutter Status Update sequence produced an unexpected error (see inner exception)", ex),
                () => throw new InvalidOperationException(
                    "Shutter Status Update sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(subscription);
            }

        private void LinkStateUpdateOnNext(ShutterLinkState state)
            {
            try
                {
                stateMachine.ShutterLinkStateChanged(state);
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message("Error while processing link state update: {linkState}", state)
                    .Write();
                }
            }

        private void SubscribeStatusUpdates()
            {
            SubscribeRotatorStatusUpdates();
            SubscribeShutterStatusUpdates();
            }

        private void SubscribeShutterStatusUpdates()
            {
            var shutterUpdates = channel.ObservableReceivedCharacters.ShutterStatusUpdates(statusFactory);
            var shutterStatusUpdateSubscription = shutterUpdates.Subscribe(ShutterStatusUpdateOnNext,
                ex => throw new InvalidOperationException(
                    "Shutter Status Update sequence produced an unexpected error (see inner exception)", ex),
                () => throw new InvalidOperationException(
                    "Shutter Status Update sequence completed unexpectedly, this is probably a bug")

            );
            disposableSubscriptions.Add(shutterStatusUpdateSubscription);
            }

        private void SubscribeRotatorStatusUpdates()
            {
            var rotatorStatusUpdates = channel.ObservableReceivedCharacters.RotatorStatusUpdates(statusFactory);
            var statusUpdateSubscription = rotatorStatusUpdates.Subscribe(RotatorStatusUpdateOnNext,
                ex => throw new InvalidOperationException(
                    "Rotator Status Update sequence produced an unexpected error (see inner exception)", ex),
                () => throw new InvalidOperationException(
                    "Rotator Status Update sequence completed unexpectedly, this is probably a bug")

            );
            disposableSubscriptions.Add(statusUpdateSubscription);
            }

        private void RotatorStatusUpdateOnNext(IRotatorStatus statusNotification)
            {
            try
                {
                stateMachine.HardwareStatusReceived(statusNotification);
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message($"Error while processing status notification: {statusNotification}")
                    .Write();
                }

            }

        private void ShutterStatusUpdateOnNext(IShutterStatus statusNotification)
            {
            try
                {
                stateMachine.HardwareStatusReceived(statusNotification);
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message($"Error while processing status notification: {statusNotification}")
                    .Write();
                }
            }

        private void SubscribeShutterDirection()
            {
            var shutterDirectionSubscription = channel.ObservableReceivedCharacters.ShutterDirectionUpdates()
                .Subscribe(stateMachine.ShutterDirectionReceived,
                    ex => throw new InvalidOperationException(
                        "Shutter Direction sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "Shutter Direction sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(shutterDirectionSubscription);
            }

        private void SubscribeRotationDirection()
            {
            var rotationDirectionSequence = channel.ObservableReceivedCharacters.RotatorDirectionUpdates();
            var rotationDirectionSubscription = rotationDirectionSequence
                .Subscribe(stateMachine.RotationDirectionReceived,
                    ex => throw new InvalidOperationException(
                        "RotationDirection sequence produced an unexpected error (see inner exception)", ex),
                    () => throw new InvalidOperationException(
                        "RotationDirection sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(rotationDirectionSubscription);
            }

        private void SubscribeAzimuthEncoderTicks()
            {
            var azimuthEncoderTicks = channel.ObservableReceivedCharacters.AzimuthEncoderTicks();
            var azimuthEncoderSubscription = azimuthEncoderTicks
                .Subscribe(
                    stateMachine.AzimuthEncoderTickReceived,
                    ex => throw new InvalidOperationException(
                        "Encoder tick sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "Encoder tick sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(azimuthEncoderSubscription);
            }

        private void SubscribeShutterPositionTicks()
            {
            var shutterPositionTicks = channel.ObservableReceivedCharacters.ShutterPositionTicks();
            var subscription = shutterPositionTicks.Subscribe(ShutterPositionTickOnNext,
                    ex => throw new InvalidOperationException(
                        "Encoder tick sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "Encoder tick sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(subscription);
            }

        private void ShutterPositionTickOnNext(int position)
            {
            try
                {
                stateMachine.ShutterEncoderTickReceived(position);
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message("Error while processing shutter position: {position}", position)
                    .Write();
                }
            }

        private void RotationDirectionOnNext(RotationDirection direction)
            {
            try
                {
                stateMachine.RotationDirectionReceived(direction);
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message("Error while processing rotation direction: {direction}", direction)
                    .Write();
                }
            }

        public void Close()
            {
            UnsubscribeControllerEvents();
            channel.Close();
            }

        private void UnsubscribeControllerEvents()
            {
            disposableSubscriptions.ForEach(m => m.Dispose());
            disposableSubscriptions.Clear();
            }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void RequestEmergencyStop()
            {
            var pause = TimeSpan.FromSeconds(1);
            stateMachine.AllStop();
            Task.Delay(pause).Wait();
            stateMachine.AllStop();
            Task.Delay(pause).Wait();
            stateMachine.AllStop();
            }

        public void SlewToAzimuth(double azimuth)
            {
            if (azimuth < 0.0 || azimuth >= 360.0)
                throw new ArgumentOutOfRangeException(nameof(azimuth), azimuth,
                    "Invalid azimuth. Must be 0.0 <= azimuth < 360.0");
            stateMachine.RotateToAzimuthDegrees(azimuth);
            }

        public void OpenShutter()
            {
            if (ShutterLimitSwitches == SensorState.Open)
                {
                Log.Warn()
                    .Message("Ignoring OpenShutter request because ShutterPosition is {state}", ShutterLimitSwitches)
                    .Property("state", ShutterLimitSwitches)
                    .Write();
                return;
                }
            Log.Info().Message("Opening shutter").Write();
            stateMachine.OpenShutter();
            }

        public void CloseShutter()
            {
            if (ShutterLimitSwitches == SensorState.Closed)
                {
                Log.Warn()
                    .Message("Ignoring CloseShutter request because {limits} {disposition}", ShutterLimitSwitches, ShutterDisposition)
                    .Write();
                return;
                }
            Log.Info().Message("Closing shutter").Write();
            stateMachine.CloseShutter();
            }

        /// <summary>
        ///     Parks the dome by closing the shutter.
        ///     Blocks until completed or an error occurs.
        /// </summary>
        public void Park()
            {
            TimeSpan timeout;
            if (ShutterLimitSwitches != SensorState.Closed)
                {
                stateMachine.CloseShutter();
                timeout = configuration.MaximumFullRotationTime + configuration.MaximumShutterCloseTime;
                }
            else
                {
                stateMachine.RotateToHomePosition();
                timeout = configuration.MaximumFullRotationTime;
                }
            // Potentially throws TimeoutException - let this propagate to the client application.
            stateMachine.WaitForReady(timeout);
            }

        private void ReleaseUnmanagedResources()
            {
            }

        private void Dispose(bool disposing)
            {
            if (disposed) return;
            ReleaseUnmanagedResources();
            if (disposing)
                {
                channel.Dispose();
                }
            disposed = true;
            }

        /// <inheritdoc />
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        /// <inheritdoc />
        ~DeviceController() => Dispose(false);
        }
    }