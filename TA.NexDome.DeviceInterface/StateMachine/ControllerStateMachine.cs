// Copyright © Tigra Astronomy, all rights reserved.
// This file is part of the TA.DigitalDomeworks project
//
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
//
// File: ControllerStateMachine.cs  Last modified: 2018-09-14@18:13 by Tim Long

using JetBrains.Annotations;
using NLog.Fluent;
using PostSharp.Patterns.Model;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    [NotifyPropertyChanged]
    public class ControllerStateMachine : INotifyHardwareStateChanged
        {
        internal readonly ManualResetEvent InReadyState = new ManualResetEvent(false);
        internal readonly ManualResetEvent ShutterInReadyState = new ManualResetEvent(false);
        internal readonly ManualResetEvent RotatorInReadyState = new ManualResetEvent(false);
        [CanBeNull] internal CancellationTokenSource KeepAliveCancellationSource;

        public ControllerStateMachine(IControllerActions controllerActions, DeviceControllerOptions options,
            IClock clock)
            {
            ControllerActions = controllerActions;
            Options = options;
            Clock = clock;
            CurrentState = new Uninitialized();
            }

        internal IControllerActions ControllerActions { get; }

        internal DeviceControllerOptions Options { get; }

        public IClock Clock { get; }

        internal IControllerState CurrentState { get; private set; }

        [CanBeNull]
        public IHardwareStatus HardwareStatus { get; private set; }

        [CanBeNull]
        public IRotatorStatus RotatorStatus { get; private set; }

        [CanBeNull]
        public IShutterStatus ShutterStatus { get; private set; }
        public ShutterLinkState ShutterLinkState { get; internal set; }

        public bool AtHome { get; set; }

        public SensorState ShutterPosition { get; set; }

        /// <summary>
        ///     The state of the user output pins. Bits 0..3 are significant, other bits are unused.
        /// </summary>
        public Octet UserPins { get; private set; } = Octet.Zero;

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

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Initializes the state machine and optionally sets the starting state.
        /// </summary>
        /// <param name="startState"></param>
        public void Initialize(IControllerState startState) => TransitionToState(startState);

        /// <summary>
        ///     Initializes the specified rotator initial state.
        /// </summary>
        /// <param name="rotatorInitialState">
        /// Sets the Initial state of the rotator state machine.
        /// </param>
        public void Initialize(IRotatorState rotatorInitialState) => TransitionToState(rotatorInitialState);

        /// <summary>
        /// Initializes the specified shutter initial state.
        /// </summary>
        /// <param name="shutterInitialState">
        /// Sets the Initial state of the shutter state machine.
        /// </param>
        public void Initialize(IShutterState shutterInitialState) => TransitionToState(shutterInitialState);

        private void SetCurrentState<TState>(TState newState) where TState : IState
            {
            switch (newState)
                {
                case IRotatorState rotator:
                    RotatorState = rotator;
                    break;
                case IShutterState shutter:
                    ShutterState = shutter;
                    break;
                case IControllerState state:
                    CurrentState = state;
                    break;
                }
            }

        private IState GetCurrentState<TState>(TState targetState) where TState : IState
            {
            switch (targetState)
                {
                case IRotatorState state:
                    return RotatorState;
                case IShutterState state:
                    return ShutterState;
                case IControllerState state:
                    return CurrentState;
                default:
                    throw new InvalidOperationException("Funky state type");
                }
            }

        public void TransitionToState<TState>([NotNull] TState targetState) where TState : class, IState
            {
            if (targetState == null) throw new ArgumentNullException(nameof(targetState));
            try
                {
                var state = GetCurrentState(targetState);
                state?.OnExit();
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message($"Unexpected exception leaving state {CurrentState.Name}")
                    .Write();
                }

            SetCurrentState(targetState);

            try
                {
                targetState.OnEnter();
                }
            catch (Exception ex)
                {
                Log.Error()
                    .Exception(ex)
                    .Message($"Unexpected exception entering state {targetState.Name}")
                    .Write();
                }
            }

        [NotifyPropertyChangedInvocator]
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        ///     Update state machine properties from a <see cref="IHardwareStatus" /> record.
        ///     By definition, when a status report is received, all movement has ceased.
        /// </summary>
        /// <param name="status">The status report received from the hardware.</param>
        internal void UpdateStatus(IHardwareStatus status)
            {
            AzimuthEncoderPosition = status.CurrentAzimuth;
            AzimuthMotorActive = false;
            AzimuthDirection = RotationDirection.None;
            ShutterMotorActive = false;
            ShutterMovementDirection = ShutterDirection.None;
            ShutterMotorCurrent = 0;
            ShutterPosition = SetInferredShutterPosition(status.ShutterSensor);
            AtHome = status.AtHome;
            UserPins = status.UserPins;
            }

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
            ShutterMovementDirection = ShutterDirection.None;
            if (status.OpenSensorActive)
                ShutterPosition = SensorState.Open;
            else if (status.ClosedSensorActive)
                ShutterPosition = SensorState.Closed;
            else ShutterPosition = SensorState.Indeterminate;
            }

        private SensorState SetInferredShutterPosition(SensorState statusShutterSensor)
            {
            if (!Options.IgnoreHardwareShutterSensor)
                return statusShutterSensor;

            if (InferredShutterPosition == SensorState.Indeterminate)
                return statusShutterSensor;

            return InferredShutterPosition;
            }

        internal void RequestHardwareStatus() => ControllerActions.RequestHardwareStatus();

        public void ShutterMotorCurrentReceived(int current)
            {
            ShutterMotorCurrent = current;
            CurrentState.ShutterMovementDetected();
            }

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
            }

        public void RotationDirectionReceived(RotationDirection direction)
            {
            AzimuthDirection = direction;
            CurrentState.RotationDetected();
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
            bool signalled = InReadyState.WaitOne(timeout);
            if (!signalled)
                {
                string message = $"State machine did not enter the ready state within the allotted time of {timeout}";
                Log.Error().Message(message).Write();
                throw new TimeoutException(message);
                }
            }

        public void RotateToAzimuthDegrees(double azimuth)
            {
            //CurrentState.RotateToAzimuthDegrees(azimuth);
            RotatorState.RotateToAzimuthDegrees(azimuth);
            }

        public void OpenShutter() => ShutterState.OpenShutter();

        public void CloseShutter() => ShutterState.CloseShutter();

        public void RotateToHomePosition() => CurrentState.RotateToHomePosition();

        public void SetUserOutputPins(Octet newState)
            {
            UserPins = newState;
            CurrentState.SetUserOutputPins(newState);
            }

        internal void ResetKeepAliveTimer()
            {
            Log.Debug().Message("Keep-alive timer reset").Write();
            KeepAliveCancellationSource?.Cancel(); // Cancel any previous timer
            KeepAliveCancellationSource = new CancellationTokenSource();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            PollStatusAfterKeepAliveIntervalAsync(KeepAliveCancellationSource.Token); // Do not await the result
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }

        private async Task PollStatusAfterKeepAliveIntervalAsync(CancellationToken cancel)
            {
            await Task.Delay(Options.KeepAliveTimerInterval, cancel);
            if (cancel.IsCancellationRequested)
                {
                Log.Debug("KeepAlive poll cancelled");
                return;
                }
            Log.Debug()
                .Message("Keep-alive timer expired - generating status request")
                .Write();
            CurrentState.RequestHardwareStatus();
            }

        #region State triggers
        public void AzimuthEncoderTickReceived(int encoderPosition)
            {
            AzimuthEncoderPosition = encoderPosition;
            //CurrentState.RotationDetected();
            RotatorState.RotationDetected();
            }

        public void ShutterEncoderTickReceived(int encoderPosition)
            {
            ShutterStepPosition = encoderPosition;
            //CurrentState.RotationDetected();
            ShutterState.EncoderTickReceived(encoderPosition);
            }

        public int ShutterStepPosition { get; set; }

        public void HardwareStatusReceived(IHardwareStatus status)
            {
            Log.Info()
                .Message("Status update {status}", status)
                .Write();
            /*
             * [TPL] update the status records first, before allowing the state machine to transition.
             * Otherwise there could be a race condition where a new state sees the old status data.
             */
            HardwareStatus = status;
            UpdateStatus(status);
            CurrentState.StatusUpdateReceived(status);
            }

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

        public void ShutterLinkStateChanged(ShutterLinkState state)
            {
            Log.Info().Message("Shutter link state {state}", state).Write();
            ShutterLinkState = state;
            ShutterState.LinkStateReceived(state);
            }
        }
    }