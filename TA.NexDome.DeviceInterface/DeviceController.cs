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
using TA.Ascom.ReactiveCommunications.Diagnostics;
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

        private float DegreesPerTick => 360f / stateMachine.HardwareStatus?.DomeCircumference ?? 100;

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

        public SensorState ShutterPosition => stateMachine.ShutterPosition;

        public bool AtHome => stateMachine.AtHome;

        public IHardwareStatus CurrentStatus => stateMachine.HardwareStatus;

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
                .Property(nameof(ShutterPosition), ShutterPosition)
                .Write();
            if (ShutterPosition == SensorState.Indeterminate)
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

        private void SubscribeControllerEvents()
            {
            SubscribeAzimuthEncoderTicks();
            SubscribeRotationDirection();
            SubscribeShutterCurrentReadings();
            SubscribeShutterDirection();
            SubscribeStatusUpdates();
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
                //.ObserveOn(Scheduler.Default)
                .Subscribe(
                    stateMachine.ShutterDirectionReceived,
                    ex => throw new InvalidOperationException(
                        "Shutter Direction sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "Shutter Direction sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(shutterDirectionSubscription);
            }

        private void SubscribeShutterCurrentReadings()
            {
            var shutterCurrentReadings = channel.ObservableReceivedCharacters.ShutterCurrentReadings();
            var shutterCurrentSubscription = shutterCurrentReadings
                //.ObserveOn(Scheduler.Default)
                .Subscribe(
                    stateMachine.ShutterMotorCurrentReceived,
                    ex => throw new InvalidOperationException(
                        "Shutter Current sequence produced an unexpected error (see inner exception)", ex),
                    () => throw new InvalidOperationException(
                        "ShutterCurrent sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(shutterCurrentSubscription);
            }

        private void SubscribeRotationDirection()
            {
            var rotationDirectionSequence = channel.ObservableReceivedCharacters.RotatorDirectionUpdates();
            var rotationDirectionSubscription = rotationDirectionSequence
                .Trace("RotationDirection")
                //.ObserveOn(Scheduler.Default)
                .Subscribe(
                    stateMachine.RotationDirectionReceived,
                    ex => throw new InvalidOperationException(
                        "RotationDirection sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "RotationDirection sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(rotationDirectionSubscription);
            }

        private void SubscribeAzimuthEncoderTicks()
            {
            var azimuthEncoderTicks = channel.ObservableReceivedCharacters.AzimuthEncoderTicks();
            var azimuthEncoderSubscription = azimuthEncoderTicks
                //.ObserveOn(Scheduler.Default)
                .Subscribe(
                    stateMachine.AzimuthEncoderTickReceived,
                    ex => throw new InvalidOperationException(
                        "Encoder tick sequence produced an unexpected error (see ineer exception)", ex),
                    () => throw new InvalidOperationException(
                        "Encoder tick sequence completed unexpectedly, this is probably a bug")
                );
            disposableSubscriptions.Add(azimuthEncoderSubscription);
            }

        private void RotationDirectionOnNext(RotationDirection direction)
            {
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

        public void SetUserOutputPin(int pinNumber, bool state)
            {
            var newState = UserPins.WithBitSetTo(pinNumber, state);
            stateMachine.SetUserOutputPins(newState);
            }

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
            if (ShutterPosition == SensorState.Open)
                {
                Log.Warn()
                    .Message("Ignoring OpenShutter request because ShutterPosition is {state}")
                    .Property("state", ShutterPosition)
                    .Write();
                return;
                }
            Log.Info().Message("Closing shutter").Write();
            stateMachine.OpenShutter();
            }

        public void CloseShutter()
            {
            if (ShutterPosition == SensorState.Closed)
                {
                Log.Warn()
                    .Message("Ignoring CloseShutter request because ShutterPosition is {state}")
                    .Property("state", ShutterPosition)
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
            if (ShutterPosition != SensorState.Closed)
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