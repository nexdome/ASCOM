// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using System;

    internal class OpeningState : ShutterStateBase
        {
        private static readonly TimeSpan EncoderTickTimeout = TimeSpan.FromSeconds(2.5);

        /// <inheritdoc />
        public OpeningState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterDisposition = ShutterDisposition.Opening;
            Machine.ShutterMotorActive = true;
            Machine.ShutterMovementDirection = ShutterDirection.Opening;
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
            ResetTimeout(EncoderTickTimeout);
            Machine.ShutterStepPosition = encoderPosition;
            }

        /// <inheritdoc />
        public override void StatusUpdateReceived(IShutterStatus status)
            {
            base.StatusUpdateReceived(status);
            Machine.UpdateStatus(status);
            Machine.TransitionToState(new OpenState(Machine));
            }

        /// <inheritdoc />
        protected override void HandleTimeout()
            {
            base.HandleTimeout();
            Machine.ControllerActions.PerformEmergencyStop();
            Machine.TransitionToState(new RequestStatusState(Machine));
            }

        /// <summary>
        ///     It's possible for the shutter to reverse direction while opening, especially if
        ///     the rain sensor becomes active.
        /// </summary>
        /// <param name="direction">The direction.</param>
        public override void ShutterDirectionReceived(ShutterDirection direction)
            {
            base.ShutterDirectionReceived(direction);
            if (direction == ShutterDirection.Closing)
                Machine.TransitionToState(new ClosingState(Machine));
            }
        }
    }