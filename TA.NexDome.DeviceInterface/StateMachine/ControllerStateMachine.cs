// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using JetBrains.Annotations;

    using NLog.Fluent;

    using PostSharp.Patterns.Model;

    using TA.NexDome.DeviceInterface.StateMachine.Rotator;
    using TA.NexDome.SharedTypes;

    [NotifyPropertyChanged]
    public class ControllerStateMachine : INotifyPropertyChanged
        {
        internal readonly ManualResetEvent RotatorInReadyState = new ManualResetEvent(false);

        internal readonly ManualResetEvent ShutterInReadyState = new ManualResetEvent(false);

        [CanBeNull]
        internal CancellationTokenSource KeepAliveCancellationSource;

        public ControllerStateMachine(
            IControllerActions controllerActions,
            DeviceControllerOptions options,
            IClock clock)
            {
            ControllerActions = controllerActions;
            Options = options;
            Clock = clock;
            }

        internal IControllerActions ControllerActions { get; }

        internal DeviceControllerOptions Options { get; }

        public IClock Clock { get; }

        [CanBeNull]
        public IRotatorStatus RotatorStatus { get; private set; }

        [CanBeNull]
        public IShutterStatus ShutterStatus { get; private set; }

        public ShutterLinkState ShutterLinkState { get; internal set; }

        public bool AtHome { get; set; }

        public SensorState ShutterLimitSwitches { get; set; }

        /// <summary>
        ///     The state of the user output pins. Bits 0..3 are significant, other bits are unused.
        /// </summary>
        public Octet UserPins { get; } = Octet.Zero;

        [IgnoreAutoChangeNotification]
        internal SensorState InferredShutterPosition { get; set; } = SensorState.Indeterminate;

        public bool IsMoving => AzimuthMotorActive || ShutterMotorActive;

        public int AzimuthEncoderPosition { get; internal set; }

        public int ShutterMotorCurrent { get; internal set; }

        public RotationDirection AzimuthDirection { get; internal set; }

        public ShutterDirection ShutterMovementDirection { get; internal set; }

        public bool AzimuthMotorActive { get; internal set; }

        public bool ShutterMotorActive { get; internal set; }

        public IRotatorState RotatorState { get; internal set; }

        public IShutterState ShutterState { get; internal set; }

        public int DomeCircumference { get; internal set; }

        public int HomePosition { get; internal set; }

        public ShutterDisposition ShutterDisposition { get; internal set; } = ShutterDisposition.Offline;

        public int ShutterLimitOfTravel { get; private set; } = 46000;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initializes the specified rotator initial state.
        /// </summary>
        /// <param name="rotatorInitialState">
        ///     Sets the Initial state of the rotator state machine.
        /// </param>
        public void Initialize(IRotatorState rotatorInitialState) => TransitionToState(rotatorInitialState);

        /// <summary>
        ///     Initializes the specified shutter initial state.
        /// </summary>
        /// <param name="shutterInitialState">
        ///     Sets the Initial state of the shutter state machine.
        /// </param>
        public void Initialize(IShutterState shutterInitialState) => TransitionToState(shutterInitialState);

        private void SetCurrentState<TState>(TState newState)
            where TState : IState
            {
            switch (newState)
                {
                    case IRotatorState rotator:
                        RotatorState = rotator;
                        break;
                    case IShutterState shutter:
                        ShutterState = shutter;
                        break;
                }
            }

        [NotNull]
        private IState GetCurrentState<TState>(TState targetState)
            where TState : IState
            {
            switch (targetState)
                {
                    case IRotatorState state:
                        return RotatorState;
                    case IShutterState state:
                        return ShutterState;
                    default:
                        throw new InvalidOperationException("Funky state type");
                }
            }

        public void TransitionToState<TState>([NotNull] TState targetState)
            where TState : class, IState
            {
            if (targetState == null) throw new ArgumentNullException(nameof(targetState));
            IState state = null;
            try
                {
                state = GetCurrentState(targetState);
                state?.OnExit();
                }
            catch (Exception ex)
                {
                Log.Error().Exception(ex).Message($"Unexpected exception leaving state {state?.Name ?? "Null state"}")
                    .Write();
                }

            SetCurrentState(targetState);

            try
                {
                targetState.OnEnter();
                }
            catch (Exception ex)
                {
                Log.Error().Exception(ex).Message($"Unexpected exception entering state {targetState.Name}").Write();
                }
            }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal void UpdateStatus(IRotatorStatus status)
            {
            AzimuthEncoderPosition = status.Azimuth;
            AzimuthMotorActive = false;
            AzimuthDirection = RotationDirection.None;
            AtHome = status.AtHome;
            DomeCircumference = status.DomeCircumference;
            HomePosition = status.HomePosition;
            }

        internal void UpdateStatus(IShutterStatus status)
            {
            ShutterMotorActive = false;
            ShutterLimitOfTravel = status.LimitOfTravel;
            ShutterMovementDirection = ShutterDirection.None;
            if (status.OpenSensorActive)
                ShutterLimitSwitches = SensorState.Open;
            else if (status.ClosedSensorActive)
                ShutterLimitSwitches = SensorState.Closed;
            else ShutterLimitSwitches = SensorState.Indeterminate;
            ShutterStepPosition = status.Position;
            }

        internal void RequestHardwareStatus() => ControllerActions.RequestHardwareStatus();

        public void ShutterDirectionReceived(ShutterDirection direction)
            {
            if (direction == ShutterDirection.Closing || direction == ShutterDirection.Opening)
                ShutterMovementDirection = direction;
            ShutterState.ShutterDirectionReceived(direction);
            }

        public void AllStop()
            {
            Log.Warn().Message("Emergency Stop requested").Write();
            ControllerActions.PerformEmergencyStop();
            RotatorState.HardStopRequested();
            }

        public void RotationDirectionReceived(RotationDirection direction)
            {
            AzimuthDirection = direction;
            RotatorState.RotationDetected();
            }

        /// <summary>
        ///     Waits for the state machine to enter the Ready state. If the state is not reached within
        ///     the specified time limit, an exception is thrown.
        /// </summary>
        /// <param name="timeout">THe maximum amount of time to wait.</param>
        /// <exception cref="TimeoutException">
        ///     Thrown if the state machine is not ready within the
        ///     allotted time.
        /// </exception>
        public void WaitForReady(TimeSpan timeout)
            {
            Log.Info()
                .Message("Waiting for state machines to be ready, Shutter Installed={shutter}", Options.ShutterIsInstalled)
                .Write();
            var waitHandles = new List<WaitHandle> { RotatorInReadyState };
            if (Options.ShutterIsInstalled)
                waitHandles.Add(ShutterInReadyState);
            bool signalled = WaitHandle.WaitAll(waitHandles.ToArray(), timeout);

            if (!signalled)
                {
                Log.Error()
                    .Message("State machine did not enter the ready state within the allotted time of {timeout}", timeout)
                    .Write();
                throw new TimeoutException($"State machine did not enter the ready state within the allotted time of {timeout}");
                }
            }

        public void RotateToAzimuthDegrees(double azimuth)
            {
            // CurrentState.RotateToAzimuthDegrees(azimuth);
            RotatorState.RotateToAzimuthDegrees(azimuth);
            }

        public void OpenShutter() => ShutterState.OpenShutter();

        public void CloseShutter() => ShutterState.CloseShutter();

        public void RotateToHomePosition() => RotatorState.RotateToHomePosition();

        internal void ResetKeepAliveTimer()
            {
            Log.Debug().Message("Keep-alive timer reset").Write();
            KeepAliveCancellationSource?.Cancel(); // Cancel any previous timer
            KeepAliveCancellationSource = new CancellationTokenSource();
            }

        public void ShutterLinkStateChanged(ShutterLinkState state)
            {
            Log.Info().Message("Shutter link state {state}", state).Write();
            ShutterLinkState = state;
            ShutterState.LinkStateReceived(state);
            }

        public void SetHomeSensorAzimuth(decimal azimuth)
            {
            Log.Info().Message("Set home sensor azimuth to {azimuth}", azimuth).Write();
            decimal ticksPerDegree = DomeCircumference / 360.0m;
            decimal homeStepPosition = azimuth * ticksPerDegree;
            ControllerActions.SetHomeSensorPosition((int)homeStepPosition);
            TransitionToState(new RequestStatusState(this));
            }

        public void SavePersistentSettings()
            {
            Log.Info("Saving persistent settings").Write();
            ControllerActions.SavePersistentSettings();
            }

        #region State triggers
        public void AzimuthEncoderTickReceived(int encoderPosition)
            {
            AzimuthEncoderPosition = encoderPosition;

            // CurrentState.RotationDetected();
            RotatorState.RotationDetected();
            }

        public void ShutterEncoderTickReceived(int encoderPosition)
            {
            // The state must process the position update because it needs to make decisions based on the relative position.
            // ShutterStepPosition = encoderPosition;
            ShutterState.EncoderTickReceived(encoderPosition);
            }

        public int ShutterStepPosition { get; set; }

        public void HardwareStatusReceived(IRotatorStatus status)
            {
            Log.Info().Message("Rotator status {status}", status).Write();
            RotatorStatus = status;
            UpdateStatus(status);
            RotatorState.StatusUpdateReceived(status);
            }

        public void HardwareStatusReceived(IShutterStatus status)
            {
            Log.Info().Message("Shutter status {status}", status).Write();
            ShutterStatus = status;
            UpdateStatus(status);
            ShutterState.StatusUpdateReceived(status);
            }

        #endregion State triggers
        }
    }