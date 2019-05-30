using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    internal class OfflineState : ShutterStateBase
        {
        /// <inheritdoc />
        public OfflineState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void LinkStateReceived(ShutterLinkState state)
            {
            base.LinkStateReceived(state);
            if (state == ShutterLinkState.Online)
                Machine.TransitionToState(new RequestStatusState(Machine));
            }
        }
    }