// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.
namespace TA.NexDome.DeviceInterface.StateMachine.Rotator {
    class ReadyState : RotatorStateBase
        {
        public ReadyState(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            Machine.AzimuthMotorActive = false;

            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();

            }
        }
    }