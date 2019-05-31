using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class ClosingState : ShutterStateBase {
        private static readonly TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public ClosingState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterMotorActive = true;
            Machine.ShutterMovementDirection = ShutterDirection.Closing;
            Machine.ShutterPosition = SensorState.Open;
            ResetTimeout(EncoderTickTimeout);
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterMotorActive = false;
            Machine.ShutterMovementDirection = ShutterDirection.None;
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
            Machine.ShutterStepPosition = encoderPosition;
            ResetTimeout(EncoderTickTimeout);
            }

        /// <inheritdoc />
        protected override void HandleTimeout()
            {
            base.HandleTimeout();
            Machine.ControllerActions.PerformEmergencyStop();
            Machine.TransitionToState(new RequestStatusState(Machine));
            }
        }
    }