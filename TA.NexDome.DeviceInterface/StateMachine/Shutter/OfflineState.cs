// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using TA.NexDome.SharedTypes;

    internal class OfflineState : ShutterStateBase
        {
        /// <inheritdoc />
        public OfflineState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterDisposition = ShutterDisposition.Offline;
            Machine.ShutterStepPosition = 0;
            Machine.ShutterMotorActive = false;
            Machine.ShutterMovementDirection = ShutterDirection.None;
            Machine.ShutterLimitSwitches = SensorState.Indeterminate;
            }

        /// <inheritdoc />
        public override void LinkStateReceived(ShutterLinkState state)
            {
            base.LinkStateReceived(state);
            if (state == ShutterLinkState.Online)
                {
                Machine.ConfigureShutter();
                Machine.TransitionToState(new RequestStatusState(Machine));
                }
            }
        }
    }