using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class OpenState : ShutterStateBase {
        /// <inheritdoc />
        public OpenState(ControllerStateMachine machine) : base(machine)
            { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterPosition = SensorState.Open;
            Machine.ShutterInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterInReadyState.Reset();
            }

        public override void ShutterDirectionReceived(ShutterDirection direction)
            {
            base.ShutterDirectionReceived(direction);
            if (direction == ShutterDirection.Closing)
                Machine.TransitionToState(new ClosingState(Machine));
            else
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
            var oldPosition = Machine.ShutterStepPosition;
            Machine.ShutterStepPosition = encoderPosition;
            if (encoderPosition < oldPosition)
                Machine.TransitionToState(new ClosingState(Machine));
            else
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void OpenShutter()
            {
            base.OpenShutter();
            Machine.ControllerActions.OpenShutter();
            Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void CloseShutter()
            {
            base.CloseShutter();
            Machine.ControllerActions.CloseShutter();
            Machine.TransitionToState(new ClosingState(Machine));
            }


        }
    }