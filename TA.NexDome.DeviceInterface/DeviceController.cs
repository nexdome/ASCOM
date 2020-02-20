// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface
    {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using JetBrains.Annotations;

    using NLog.Fluent;

    using PostSharp.Patterns.Model;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.DeviceInterface.StateMachine.Shutter;
    using TA.NexDome.SharedTypes;

    using RequestStatusState = TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState;

    [NotifyPropertyChanged]
    public class DeviceController : INotifyPropertyChanged, IDisposable
        {
        // private SemanticVersion shutterFirmwareVersion;
        private static readonly SemanticVersion MinimumRequiredRotatorVersion = new SemanticVersion(2, 9, 9);

        private static readonly SemanticVersion MinimumRequiredShutterVersion = new SemanticVersion(2, 9, 9);
        private static readonly SemanticVersion HighPrecisionSlewingSupport = new SemanticVersion("3.2.0-alpha.19");

        [NotNull]
        private readonly ICommunicationChannel channel;

        private readonly DeviceControllerOptions configuration;

        [NotNull]
        private readonly List<IDisposable> disposableSubscriptions = new List<IDisposable>();

        private readonly ITransactionProcessor processor;

        [NotNull]
        private readonly ControllerStateMachine stateMachine;

        [NotNull]
        private readonly ControllerStatusFactory statusFactory;

        private bool disposed;

        private SemanticVersion rotatorFirmwareVersion;

        public DeviceController(
            ICommunicationChannel channel,
            ControllerStatusFactory factory,
            ControllerStateMachine machine,
            DeviceControllerOptions configuration,
            ITransactionProcessor processor)
            {
            this.channel = channel;
            statusFactory = factory;
            stateMachine = machine;
            this.configuration = configuration;
            this.processor = processor;
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
        public bool IsConnected => channel?.IsOpen ?? false;

        public bool IsShutterOperational => IsConnected && stateMachine.ShutterLinkState == ShutterLinkState.Online;

        public bool AzimuthMotorActive => stateMachine.AzimuthMotorActive;

        public RotationDirection AzimuthDirection => stateMachine.AzimuthDirection;

        public ShutterDirection ShutterMovementDirection => stateMachine.ShutterMovementDirection;

        public int ShutterMotorCurrent => stateMachine.ShutterMotorCurrent;

        public bool ShutterMotorActive => stateMachine.ShutterMotorActive;

        public SensorState ShutterLimitSwitches => stateMachine.ShutterLimitSwitches;

        public ShutterDisposition ShutterDisposition => stateMachine.ShutterDisposition;

        public float ShutterBatteryVolts { get; private set; } = Constants.BatteryHalfChargedVolts;

        public int ShutterPercentOpen =>
            (int)(stateMachine.ShutterStepPosition / (float)stateMachine.ShutterLimitOfTravel * 100f);

        public bool AtHome => stateMachine.AtHome;

        public ShutterLinkState ShutterLinkState => stateMachine.ShutterLinkState;

        public bool AtPark { get; private set; }

        public bool IsRaining { get; private set; }

        /// <inheritdoc />
        public void Dispose()
            {
            Dispose(true);
            GC.SuppressFinalize(this);
            }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RotateToHomePosition() => stateMachine.RotateToHomePosition();

        public void Open(bool performOnConnectActions = true)
            {
            SubscribeControllerEvents();
            channel.Open();
            stateMachine.Initialize(new RequestStatusState(stateMachine));
            stateMachine.Initialize(new OfflineState(stateMachine));
            stateMachine.WaitForReady(configuration.TimeToWaitForShutterOnConnect);

            if (performOnConnectActions)
                PerformActionsOnConnect();
            }

        private void PerformActionsOnConnect()
            {
            try
                {
                EnsureMinimumRequiredFirmwareVersion();
                }
            catch (UnsupportedFirmwareVersionException ex)
                {
                Environment.FailFast("Unsupported firmware detected", ex);
                }

            stateMachine.SetHomeSensorAzimuth(configuration.HomeAzimuth);
            TransactEmptyReponse(
                string.Format(Constants.CmdSetMotorSpeedTemplate, 'R', configuration.RotatorMaximumSpeed));
            TransactEmptyReponse(
                string.Format(Constants.CmdSetRampTimeTemplate, 'R', configuration.RotatorRampTime.TotalMilliseconds));

            if (configuration.ShutterIsInstalled && stateMachine.ShutterLinkState == ShutterLinkState.Online)
                {
                TransactEmptyReponse(string.Format(Constants.CmdSetMotorSpeedTemplate, 'S',
                    configuration.ShutterMaximumSpeed));
                TransactEmptyReponse(string.Format(Constants.CmdSetRampTimeTemplate, 'S',
                    configuration.ShutterRampTime.TotalMilliseconds));
                }
            }

        private void TransactEmptyReponse(string command)
            {
            var transaction = new EmptyResponseTransaction(command);
            processor.CommitTransaction(transaction);
            transaction.WaitForCompletionOrTimeout();
            }

        private void EnsureMinimumRequiredFirmwareVersion()
            {
            var versionTransaction = new SemVerTransaction(Constants.CmdGetRotatorVersion);
            processor.CommitTransaction(versionTransaction);
            versionTransaction.WaitForCompletionOrTimeout();
            versionTransaction.ThrowIfFailed();
            rotatorFirmwareVersion = versionTransaction.SemanticVersion;
            if (rotatorFirmwareVersion < MinimumRequiredRotatorVersion)
                {
                Log.Error().Message(
                    "Unsupported rotator firmware version {version}; will throw.",
                    rotatorFirmwareVersion).Write();
                MessageBox.Show(
                    "Your rotator firmware is too old to work with this driver.\nPlease contact NexDome for an upgrade.\n\n"
                    + $"Your version: {rotatorFirmwareVersion}\n"
                    + $"Minimum required version: {MinimumRequiredRotatorVersion}\n\n"
                    + "The connection will now close.\n\n" + "See the Seup Dialog for firmware update options.",
                    "Firmware Version Incompatible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Close();
                throw new UnsupportedFirmwareVersionException(MinimumRequiredRotatorVersion, rotatorFirmwareVersion);
                }

            // versionTransaction = new SemVerTransaction(Constants.CmdGetShutterVersion);
            // processor.CommitTransaction(versionTransaction);
            // versionTransaction.WaitForCompletionOrTimeout();
            // versionTransaction.ThrowIfFailed();
            // shutterFirmwareVersion = versionTransaction.SemanticVersion;
            // if (rotatorFirmwareVersion < MinimumRequiredRotatorVersion)
            // {
            // Log.Error()
            // .Message("Unsupported shutter firmware version {version}; will throw.", shutterFirmwareVersion)
            // .Write();
            // MessageBox.Show(
            // "Your rotator firmware is too old to work with this driver.\nPlease contact NexDome for an upgrade.\n\n"
            // + $"Your version: {rotatorFirmwareVersion}\n"
            // + $"Minimum required version: {MinimumRequiredRotatorVersion}\n\n"
            // + "The connection will now close.", "Firmware Version Incompatible", MessageBoxButtons.OK,
            // MessageBoxIcon.Stop);
            // Close();
            // throw new UnsupportedFirmwareVersionException(MinimumRequiredRotatorVersion, rotatorFirmwareVersion);
            // }
            Log.Info().Message("Rotator firmware version {version}", rotatorFirmwareVersion).Write();

            // Log.Info().Message("Shutter firmware version {version}", shutterFirmwareVersion).Write();
            // if (rotatorFirmwareVersion != shutterFirmwareVersion)
            // Log.Warn()
            // .Message("Rotator/Shutter firmware version mismatch - this is not a recommended configuration");
            }

        /// <summary>
        ///     Uses a variety of pattern matches over the input character stream (defined in
        ///     <see cref="ObservableExtensions" />) to generate a number of observable sequences. For
        ///     example, there is an observable sequence of azimuth positions. Each of these sequences
        ///     is then subscribed to an <c>*OnNext()</c> handler method, which catches any exceptions
        ///     generated and passes the sequence elements on to the state machine. Thus the state
        ///     machine "reacts" to the serial input. Each sequence is assumed to only produce correct
        ///     and valid values. Each of the sequences produced is added to
        ///     <see cref="disposableSubscriptions" /> for later disposal and cleanup.
        /// </summary>
        private void SubscribeControllerEvents()
            {
            SubscribeAzimuthEncoderTicks();
            SubscribeShutterPositionTicks();
            SubscribeRotationDirection();
            SubscribeShutterDirection();
            SubscribeStatusUpdates();
            SubscribeLinkStateUpdates();
            SubscribeShutterVolts();
            SubscribeRainSensorUpdates();
            }

        private void SubscribeLinkStateUpdates()
            {
            var linkStateUpdates = channel.ObservableReceivedCharacters.LinkStatusUpdates();
            var subscription = linkStateUpdates.Subscribe(
                LinkStateUpdateOnNext,
                ex => throw new InvalidOperationException(
                          "Shutter Status Update sequence produced an unexpected error (see inner exception)",
                          ex),
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
                Log.Error().Exception(ex).Message("Error while processing link state update: {linkState}", state)
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
            var shutterStatusUpdateSubscription = shutterUpdates.Subscribe(
                ShutterStatusUpdateOnNext,
                ex => throw new InvalidOperationException(
                          "Shutter Status Update sequence produced an unexpected error (see inner exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "Shutter Status Update sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(shutterStatusUpdateSubscription);
            }

        private void SubscribeRotatorStatusUpdates()
            {
            var rotatorStatusUpdates = channel.ObservableReceivedCharacters.RotatorStatusUpdates(statusFactory);
            var statusUpdateSubscription = rotatorStatusUpdates.Subscribe(
                RotatorStatusUpdateOnNext,
                ex => throw new InvalidOperationException(
                          "Rotator Status Update sequence produced an unexpected error (see inner exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "Rotator Status Update sequence completed unexpectedly, this is probably a bug"));
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
                Log.Error().Exception(ex).Message($"Error while processing status notification: {statusNotification}")
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
                Log.Error().Exception(ex).Message($"Error while processing status notification: {statusNotification}")
                    .Write();
                }
            }

        private void SubscribeShutterDirection()
            {
            var shutterDirectionSubscription = channel.ObservableReceivedCharacters.ShutterDirectionUpdates().Subscribe(
                stateMachine.ShutterDirectionReceived,
                ex => throw new InvalidOperationException(
                          "Shutter Direction sequence produced an unexpected error (see ineer exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "Shutter Direction sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(shutterDirectionSubscription);
            }

        private void SubscribeRotationDirection()
            {
            var rotationDirectionSequence = channel.ObservableReceivedCharacters.RotatorDirectionUpdates();
            var rotationDirectionSubscription = rotationDirectionSequence.Subscribe(
                stateMachine.RotationDirectionReceived,
                ex => throw new InvalidOperationException(
                          "RotationDirection sequence produced an unexpected error (see inner exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "RotationDirection sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(rotationDirectionSubscription);
            }

        private void SubscribeShutterVolts()
            {
            var observableBatteryVolts = channel.ObservableReceivedCharacters.BatteryVoltageUpdates();
            var subscription = observableBatteryVolts.Subscribe(value => ShutterBatteryVolts = value);
            disposableSubscriptions.Add(subscription);
            }

        private void SubscribeRainSensorUpdates()
            {
            var observableRain = channel.ObservableReceivedCharacters.RainSensorUpdates();
            var subscription = observableRain.Subscribe(value => IsRaining = value);
            disposableSubscriptions.Add(subscription);
            }

        private void SubscribeAzimuthEncoderTicks()
            {
            var azimuthEncoderTicks = channel.ObservableReceivedCharacters.AzimuthEncoderTicks();
            var azimuthEncoderSubscription = azimuthEncoderTicks.Subscribe(
                OnNextAzimuthEncoderTick,
                ex => throw new InvalidOperationException(
                          "Encoder tick sequence produced an unexpected error (see ineer exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "Encoder tick sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(azimuthEncoderSubscription);
            }

        private void OnNextAzimuthEncoderTick(int position)
            {
            AtPark = false;
            stateMachine.AzimuthEncoderTickReceived(position);
            }

        private void SubscribeShutterPositionTicks()
            {
            var shutterPositionTicks = channel.ObservableReceivedCharacters.ShutterPositionTicks();
            var subscription = shutterPositionTicks.Subscribe(
                ShutterPositionTickOnNext,
                ex => throw new InvalidOperationException(
                          "Encoder tick sequence produced an unexpected error (see ineer exception)",
                          ex),
                () => throw new InvalidOperationException(
                          "Encoder tick sequence completed unexpectedly, this is probably a bug"));
            disposableSubscriptions.Add(subscription);
            }

        private void ShutterPositionTickOnNext(int position)
            {
            try
                {
                AtPark = false;
                stateMachine.ShutterEncoderTickReceived(position);
                }
            catch (Exception ex)
                {
                Log.Error().Exception(ex).Message("Error while processing shutter position: {position}", position)
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
                Log.Error().Exception(ex).Message("Error while processing rotation direction: {direction}", direction)
                    .Write();
                }
            }

        public void Close()
            {
            UnsubscribeControllerEvents();
            if (IsConnected)
                {
                stateMachine.SavePersistentSettings();
                channel.Close();
                }
            }

        private void UnsubscribeControllerEvents()
            {
            disposableSubscriptions.ForEach(m => m.Dispose());
            disposableSubscriptions.Clear();
            }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

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
            if (rotatorFirmwareVersion < HighPrecisionSlewingSupport)
                LowPrecisionSlewStrategy(azimuth);
            else
                HighPrecisionSlewStrategy(azimuth);
            }

        private void HighPrecisionSlewStrategy(double azimuth)
            {
            var circumference = (double)stateMachine.DomeCircumference;
            var stepsPerDegree = circumference / 360.0;
            var targetStepPosition = (int)(azimuth * stepsPerDegree);
            stateMachine.RotateToStepPosition(targetStepPosition);
            }

        private void LowPrecisionSlewStrategy(double azimuth)
            {
            if (azimuth < 0.0 || azimuth >= 360.0)
                throw new ArgumentOutOfRangeException(
                    nameof(azimuth),
                    azimuth,
                    "Invalid azimuth. Must be 0.0 <= azimuth < 360.0");
            stateMachine.RotateToAzimuthDegrees(azimuth);
            }

        public void OpenShutter()
            {
            if (ShutterLimitSwitches == SensorState.Open)
                {
                Log.Warn().Message(
                    "Ignoring OpenShutter request because ShutterPosition is {state}",
                    ShutterLimitSwitches).Property("state", ShutterLimitSwitches).Write();
                return;
                }

            Log.Info().Message("Opening shutter").Write();
            stateMachine.OpenShutter();
            }

        public void CloseShutter()
            {
            if (ShutterLimitSwitches == SensorState.Closed)
                {
                Log.Warn().Message(
                    "Ignoring CloseShutter request because {limits} {disposition}",
                    ShutterLimitSwitches,
                    ShutterDisposition).Write();
                return;
                }

            Log.Info().Message("Closing shutter").Write();
            stateMachine.CloseShutter();
            }

        /// <summary>
        ///     Parks the dome by closing the shutter.
        ///     Blocks until completed or an error occurs.
        /// </summary>
        public async Task Park()
            {
            await Task.Run(ParkAsync).ContinueOnCurrentThread();
            }

        private void ParkAsync()
            {
            try
                {
                Log.Info().Message("Rotating to park position for park").Write();
                stateMachine.RotateToAzimuthDegrees((double)configuration.ParkAzimuth);
                stateMachine.WaitForReady(configuration.MaximumFullRotationTime);
                if (IsShutterOperational)
                    {
                    Log.Info().Message("Closing shutter for park").Write();
                    stateMachine.CloseShutter();
                    stateMachine.WaitForReady(configuration.MaximumShutterCloseTime);
                    }
                else
                    {
                    Log.Warn().Message("Shutter is not operational, not closing shutter").Write();
                    }
                AtPark = true;
                }
            // ReSharper disable once CatchAllClause
            catch (Exception e)
                {
                Log.Error().Message("Exception while parking").Exception(e).Write();
                }
            }

        private void ReleaseUnmanagedResources() { }

        private void Dispose(bool disposing)
            {
            if (disposed) return;
            Close();
            ReleaseUnmanagedResources();
            if (disposing) channel.Dispose();
            disposed = true;
            }

        /// <inheritdoc />
        ~DeviceController() => Dispose(false);
        }
    }