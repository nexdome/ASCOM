using System;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator {
    class RequestStatusState : RotatorStateBase {
        /// <inheritdoc />
        public RequestStatusState(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            ResetTimeout(TimeSpan.FromSeconds(2));
            Machine.ControllerActions.RequestRotatorStatus();
            }
        }
    }