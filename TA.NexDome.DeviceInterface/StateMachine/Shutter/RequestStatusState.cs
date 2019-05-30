using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    internal class RequestStatusState : ShutterStateBase
        {
        private static readonly TimeSpan StatusReceiveTimeout = TimeSpan.FromSeconds(4);

        /// <inheritdoc />
        public RequestStatusState(ControllerStateMachine machine) : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            ResetTimeout(StatusReceiveTimeout);
            Machine.ControllerActions.RequestShutterStatus();
            }

        /// <inheritdoc />
        public override void StatusUpdateReceived(IShutterStatus status)
            {
            base.StatusUpdateReceived(status);
            Machine.UpdateStatus(status);
            if (status.ClosedSensorActive)
                Machine.TransitionToState(new ClosedState(Machine));
            else
                Machine.TransitionToState(new OpenState(Machine));
            }
        }
    }