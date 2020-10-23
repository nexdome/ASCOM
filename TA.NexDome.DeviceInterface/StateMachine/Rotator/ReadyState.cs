// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator
    {
    internal class ReadyState : RotatorStateBase
        {
        public ReadyState(ControllerStateMachine machine)
            : base(machine) { }

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
            Machine.ControllerActions.RotateToAzimuth((int)azimuth);
            Machine.TransitionToState(new RotatingState(Machine));
            }

        /// <inheritdoc />
        public override void RotateToStepPosition(int targetPosition)
            {
            base.RotateToStepPosition(targetPosition);
            Machine.ControllerActions.RotateToStepPosition(targetPosition);
            Machine.TransitionToState(new RotatingState(Machine));
            }

        /// <inheritdoc />
        public override void RotateToHomePosition()
            {
            base.RotateToHomePosition();
            Machine.ControllerActions.RotateToHomePosition();
            Machine.TransitionToState(new RotatingState(Machine));
            }
        }
    }