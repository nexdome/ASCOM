using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class OpeningState : ShutterStateBase {
        private static TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public OpeningState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterMotorActive = true;
            Machine.ShutterMovementDirection = ShutterDirection.Opening;
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
            ResetTimeout(EncoderTickTimeout);
            }
        }
    }