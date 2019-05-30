using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class OpeningState : ShutterStateBase {
        /// <inheritdoc />
        protected internal OpeningState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterMotorActive = true;
            Machine.ShutterMovementDirection = ShutterDirection.Opening;
            Machine.ShutterPosition = SensorState.Open;
            }
        }
    }