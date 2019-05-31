using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter {
    class ClosedState : ShutterStateBase {
        /// <inheritdoc />
        public ClosedState(ControllerStateMachine machine) : base(machine) { }
        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterInReadyState.Reset();
            }

        /// <inheritdoc />
        public override void ShutterDirectionReceived(ShutterDirection direction)
            {
            base.ShutterDirectionReceived(direction);
            if (direction== ShutterDirection.Opening)
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
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
        public override void LinkStateReceived(ShutterLinkState state)
            {
            base.LinkStateReceived(state);
            if (state!= ShutterLinkState.Online)
                Machine.TransitionToState(new OfflineState(Machine));
            }
        }
    }