// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: DeviceController.cs  Last modified: 2020-07-21@21:33 by Tim Long

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using JetBrains.Annotations;
using PostSharp.Patterns.Model;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.Utils.Core;
using TA.Utils.Core.Diagnostics;
using RequestStatusState = TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState;

namespace TA.NexDome.DeviceInterface
    {
    [NotifyPropertyChanged]
    public class DeviceController : INotifyPropertyChanged, IDisposable
        {
        // private SemanticVersion shutterFirmwareVersion;
        private static readonly SemanticVersion MinimumRequiredRotatorVersion = new SemanticVersion(2, 9, 9);
        private static readonly SemanticVersion MinimumRequiredShutterVersion = new SemanticVersion(2, 9, 9);
        private static readonly SemanticVersion FirmwareHighPrecisionSlewingSupport =
            new SemanticVersion("3.2.0-alpha.19");
        private static readonly SemanticVersion FirmwareShutterAutoCloseSupport = new SemanticVersion("3.4.0-alpha.1");

        [NotNull] private readonly ICommunicationChannel channel;

        private readonly DeviceControllerOptions configuration;

        [NotNull] private readonly List<IDisposable> disposableSubscriptions = new List<IDisposable>();
        private readonly ILog log;

        private readonly ITransactionProcessor processor;

        [NotNull] private readonly ControllerStateMachine stateMachine;

        [NotNull] private readonly ControllerStatusFactory statusFactory;
        private DateTime connectedTimestamp = DateTime.MinValue;

        private bool disposed;

        private SemanticVersion rotatorFirmwareVersion;
        private SemanticVersion shutterFirmwareVersion;

        public DeviceController(
            ICommunicationChannel channel,
            ControllerStatusFactory factory,
            ControllerStateMachine machine,
            DeviceControllerOptions configuration,
            ITransactionProcessor processor, ILog log)
            {
            this.channel = channel;
            statusFactory = factory;
            stateMachine = machine;
            this.configuration = configuration;
            this.processor = processor;
            this.log = log;
            }

        public Octet UserPins => stateMachine.UserPins;

        public int AzimuthEncoderPosition => stateMachine.AzimuthEncoderPosition;

        public float AzimuthDegrees => AzimuthEncoderPosition * DegreesPerTick;

        private float DegreesPerTick => 360f / stateMachine.DomeCircumference;

        /// <summary><c>true</c> if any part of the building is moving.</summary>
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

        /*
         * ShutterSisposition is marked [SafeForDependencyAnalysis] because PostSharp
         * otherwise tries to monitor changes in the configuration field.
         * configuration is immutable type so should not be analyzed.
         */
        [SafeForDependencyAnalysis]
        public ShutterDisposition ShutterDisposition
            {
            get
                {
                /*
                 * Prevent the shutter from reporting an error condition for a short time
                 * after connecting, to give the wireless connection a chance to stabilize
                 * without freaking the client application out.
                 */
                var timeSinceConnected = DateTime.UtcNow - connectedTimestamp;
                var actualShutterDisposition = stateMachine.ShutterDisposition;
                if (configuration.ShutterIsInstalled
                    && timeSinceConnected <= configuration.TimeToWaitForShutterOnConnect
                    && actualShutterDisposition == ShutterDisposition.Offline)
                    return ShutterDisposition.Closed;
                return actualShutterDisposition;
                }
            }

        public float ShutterBatteryVolts { get; private set; } = Constants.BatteryHalfChargedVolts;

        public BatteryChargeState ShutterBatteryChargeState { get; private set; } = BatteryChargeState.OK;

        public int ShutterPercentOpen =>
            (int)(stateMachine.ShutterStepPosition / (float)stateMachine.ShutterLimitOfTravel * 100f);

        public bool AtHome => stateMachine.AtHome;

        public ShutterLinkState ShutterLinkState => stateMachine.ShutterLinkState;

        public bool AtPark { get; private set; }

        public bool IsRaining { get; private set; }

        public bool IsBatteryLow { get; set; }

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
            stateMachine.Initialize(new RequestStatusState(stateMachine)); // Rotator
            stateMachine.Initialize(new OfflineState(stateMachine)); // Shutter
            stateMachine.WaitForReady(configuration.TimeToWaitForShutterOnConnect);
            connectedTimestamp = DateTime.UtcNow;
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
                log.Error().Message(
                    "Unsupported rotator firmware version {firmware}; will throw.", rotatorFirmwareVersion).Write();
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
            log.Info().Message("Rotator firmware version {firmware}", rotatorFirmwareVersion).Write();
            if (!configuration.ShutterIsInstalled || !IsShutterOperational)
                return; // No shutter
            versionTransaction = new SemVerTransaction(Constants.CmdGetShutterVersion);
            processor.CommitTransaction(versionTransaction);
            versionTransaction.WaitForCompletionOrTimeout();
            versionTransaction.ThrowIfFailed();
            shutterFirmwareVersion = versionTransaction.SemanticVersion;
            if (shutterFirmwareVersion < MinimumRequiredShutterVersion)
                {
                log.Error().Message(
                    "Unsupported shutter firmware version {firmware}; will throw.",
                    shutterFirmwareVersion).Write();
                MessageBox.Show(
                    "Your shutter firmware is too old to work with this driver.\nPlease contact NexDome for an upgrade.\n\n"
                    + $"Your version: {shutterFirmwareVersion}\n"
                    + $"Minimum required version: {MinimumRequiredShutterVersion}\n\n"
                    + "The connection will now close.\n\n" + "See the Setup screen for firmware update options.",
                    "Firmware Version Incompatible",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                Close();
                throw new UnsupportedFirmwareVersionException(MinimumRequiredShutterVersion, shutterFirmwareVersion);
                }
            log.Info().Message("Shutter firmware version {firmware}", shutterFirmwareVersion).Write();
            if (rotatorFirmwareVersion != shutterFirmwareVersion)
                {
                log.Warn()
                    .Message("Rotator/Shutter firmware version mismatch - this is not a recommended configuration")
                    .Property("rotatorVersion", rotatorFirmwareVersion)
                    .Property("shutterVersion", shutterFirmwareVersion)
                    .Write();
                var messageBuilder = new StringBuilder();
                messageBuilder.AppendLine("Your shutter and rotator firmware versions do not match.");
                messageBuilder.AppendLine("This is not a recommended configuration.");
                messageBuilder.AppendLine("Please upgrade one or both units so they are on the same version.");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine($"Rotator version: {rotatorFirmwareVersion}");
                messageBuilder.AppendLine($"Shutter version: {shutterFirmwareVersion}");
                messageBuilder.AppendLine();
                messageBuilder.AppendLine("See the Setup screen for firmware update options.");
                MessageBox.Show(messageBuilder.ToString(),
                    "Firmware Versions Mismatch",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                }
            }

        /// <summary>
        ///     Uses a variety of pattern matches over the input character stream (defined in
        ///     <see cref="ObservableExtensions" />) to generate a number of observable sequences. For example,
        ///     there is an observable sequence of azimuth positions. Each of these sequences is then
        ///     subscribed to an <c>*OnNext()</c> handler method, which catches any exceptions generated and
        ///     passes the sequence elements on to the state machine. Thus the state machine "reacts" to the
        ///     serial input. Each sequence is assumed to only produce correct and valid values. Each of the
        ///     sequences produced is added to <see cref="disposableSubscriptions" /> for later disposal and
        ///     cleanup.
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
            SubscribeLowVoltsNotification();
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
                log.Error()
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
                log.Error()
                    .Exception(ex)
                    .Message("Error while processing status notification: {status}", statusNotification)
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
                log.Error()
                    .Exception(ex)
                    .Message("Error while processing shutter status notification: {status}", statusNotification)
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
            var subscription = observableBatteryVolts.Subscribe(volts => HandleBatteryVoltsUpdate(volts));
            disposableSubscriptions.Add(subscription);
            }

        private void HandleBatteryVoltsUpdate(float volts)
            {
            ShutterBatteryVolts = volts;
            decimal dVolts = (decimal)volts;
            if (dVolts <= configuration.ShutterLowBatteryThresholdVolts)
                ShutterBatteryChargeState = BatteryChargeState.Alarm;
            else if (dVolts < (configuration.ShutterLowBatteryThresholdVolts * 1.1M))
                ShutterBatteryChargeState = BatteryChargeState.Warning;
            else ShutterBatteryChargeState = BatteryChargeState.OK;
            }

        private void SubscribeRainSensorUpdates()
            {
            var observableRain = channel.ObservableReceivedCharacters.RainSensorUpdates();
            var subscription = observableRain.Subscribe(value => IsRaining = value);
            disposableSubscriptions.Add(subscription);
            }

        private void SubscribeLowVoltsNotification()
            {
            var observableLowVolts =
                channel.ObservableReceivedCharacters.LowVoltsNotifications(configuration
                    .ShutterLowVoltsNotificationTimeToLive);
            var subscription = observableLowVolts.Subscribe(s => IsBatteryLow = s);
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
                log.Error()
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
                log.Error()
                    .Exception(ex)
                    .Message("Error while processing rotation direction: {direction}", direction)
                    .Write();
                }
            }

        public void Close()
            {
            log.Info().Message("Device Controller closing").Write();
            UnsubscribeControllerEvents();
            if (IsConnected)
                {
                connectedTimestamp = DateTime.MinValue;
                stateMachine.SavePersistentSettings();
                channel.Close();
                }
            log.Info().Message("Device Controller closed").Write();
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
            if (rotatorFirmwareVersion < FirmwareHighPrecisionSlewingSupport)
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
                log.Warn()
                    .Message(
                        "Ignoring OpenShutter request because ShutterPosition is {state}", ShutterLimitSwitches)
                    .Write();
                return;
                }

            log.Info().Message("Opening shutter").Write();
            stateMachine.OpenShutter();
            }

        public void CloseShutter()
            {
            if (ShutterLimitSwitches == SensorState.Closed)
                {
                log.Warn().Message(
                        "Ignoring CloseShutter request because {limits} {disposition}", ShutterLimitSwitches,
                        ShutterDisposition)
                    .Write();
                return;
                }

            log.Info().Message("Closing shutter").Write();
            stateMachine.CloseShutter();
            }

        /// <summary>Parks the dome by closing the shutter. Blocks until completed or an error occurs.</summary>
        public async Task Park()
            {
            log.Debug().Message("Starting asynchronous park").Write();
            await Task.Run(ParkAsync).ContinueOnAnyThread();
            log.Debug().Message("Completed asynchronous park").Write();
            }

        private void ParkAsync()
            {
            try
                {
                log.Info().Message("Rotating to park position for park").Write();
                stateMachine.RotateToAzimuthDegrees((double)configuration.ParkAzimuth);
                stateMachine.WaitForReady(configuration.MaximumFullRotationTime);
                if (IsShutterOperational)
                    {
                    log.Info().Message("Closing shutter for park").Write();
                    stateMachine.CloseShutter();
                    stateMachine.WaitForReady(configuration.MaximumShutterCloseTime);
                    }
                else
                    {
                    log.Warn().Message("Shutter is not operational, not closing shutter").Write();
                    }
                AtPark = true;
                }
            // ReSharper disable once CatchAllClause
            catch (Exception e)
                {
                log.Error()
                    .Message("Exception while parking")
                    .Exception(e)
                    .Write();
                }
            }

        private void ReleaseManagedResources() { }

        private void Dispose(bool disposing)
            {
            if (disposed) return;
            log.Debug().Message("Disposing").Write();
            Close();
            ReleaseManagedResources();
            if (disposing) channel.Dispose();
            disposed = true;
            }

        /// <inheritdoc />
        ~DeviceController() => Dispose(false);
        }
    }