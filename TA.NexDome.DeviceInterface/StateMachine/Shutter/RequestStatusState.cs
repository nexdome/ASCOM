// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using System;

    internal class RequestStatusState : ShutterStateBase
        {
        private static readonly TimeSpan StatusReceiveTimeout = TimeSpan.FromSeconds(10);

        /// <inheritdoc />
        public RequestStatusState(ControllerStateMachine machine)
            : base(machine) { }

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

        /// <inheritdoc />
        protected override void HandleTimeout()
            {
            base.HandleTimeout();
            ResetTimeout(StatusReceiveTimeout);
            //Machine.ControllerActions.PerformEmergencyStop();
            //Machine.ControllerActions.RequestShutterStatus();
            Machine.TransitionToState(new OfflineState(Machine));
            }
        }
    }