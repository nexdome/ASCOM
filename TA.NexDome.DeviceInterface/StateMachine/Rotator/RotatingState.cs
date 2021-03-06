﻿// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Rotator
    {
    using System;

    internal class RotatingState : RotatorStateBase
        {
        /// <summary>
        ///     While rotating, azimuth position updates are expected to arrive at least this often.
        ///     If they do not, then it will be assumed that rotation may have stopped and we must
        ///     attempt to request a status update.
        /// </summary>
        private static readonly TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public RotatingState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.AzimuthMotorActive = true;
            ResetTimeout(EncoderTickTimeout);
            }

        public override void RotationDetected()
            {
            base.RotationDetected();
            ResetTimeout(EncoderTickTimeout);
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
            Machine.TransitionToState(new RequestStatusState(Machine));
            }

        /// <inheritdoc />
        public override void HardStopRequested()
            {
            base.HardStopRequested();
            Machine.TransitionToState(new RequestStatusState(Machine));
            }
        }
    }