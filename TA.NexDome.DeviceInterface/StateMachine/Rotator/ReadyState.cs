// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator {
    class ReadyState : RotatorStateBase
        {
        public ReadyState(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            Machine.AzimuthMotorActive = false;
            Machine.AzimuthDirection = RotationDirection.None;
            Machine.RotatorInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            Machine.RotatorInReadyState.Reset();
            base.OnExit();
            }

        /// <inheritdoc />
        public override void RotationDetected()
            {
            base.RotationDetected();
            Machine.TransitionToState(new RotatingState(Machine));
            }

        /// <inheritdoc />
        public override void RotateToAzimuthDegrees(double azimuth)
            {
            base.RotateToAzimuthDegrees(azimuth);
            Machine.ControllerActions.RotateToAzimuth((int) azimuth);
            Machine.TransitionToState(new RotatingState(Machine));
            }
    }
    }