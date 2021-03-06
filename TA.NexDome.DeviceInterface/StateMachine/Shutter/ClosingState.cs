﻿// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using System;

    internal class ClosingState : ShutterStateBase
        {
        private static readonly TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public ClosingState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterDisposition = ShutterDisposition.Closing;
            Machine.ShutterMotorActive = true;
            Machine.ShutterMovementDirection = ShutterDirection.Closing;
            Machine.ShutterLimitSwitches = SensorState.Open;
            ResetTimeout(EncoderTickTimeout);
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterMotorActive = false;
            Machine.ShutterMovementDirection = ShutterDirection.None;
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
            Machine.ShutterStepPosition = encoderPosition;
            ResetTimeout(EncoderTickTimeout);
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
            Machine.ControllerActions.PerformEmergencyStop();
            Machine.TransitionToState(new RequestStatusState(Machine));
            }
        }
    }