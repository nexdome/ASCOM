// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator
    {
    using System;

    internal class RequestStatusState : RotatorStateBase
        {
        /// <inheritdoc />
        public RequestStatusState(ControllerStateMachine machine)
            : base(machine) { }

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