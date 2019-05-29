// Copyright © Tigra Astronomy, all rights reserved.
using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator
    {
    internal class RequestStatusState : RotatorStateBase
        {
        /// <inheritdoc />
        public RequestStatusState(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            ResetTimeout(TimeSpan.FromSeconds(2));
            Machine.ControllerActions.RequestRotatorStatus();
            }

        public override void StatusUpdateReceived(IRotatorStatus status)
            {
            base.StatusUpdateReceived(status);
            Machine.UpdateStatus(status);
            Machine.TransitionToState(new ReadyState(Machine));
            }

        protected internal override void HandleTimeout()
            {
            base.HandleTimeout();
            Machine.ControllerActions.PerformEmergencyStop();
            Machine.ControllerActions.RequestRotatorStatus();
            }
        }
    }