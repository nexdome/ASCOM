// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using TA.NexDome.SharedTypes;

    internal class ClosedState : ShutterStateBase
        {
        /// <inheritdoc />
        public ClosedState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterDisposition = ShutterDisposition.Closed;
            Machine.ShutterMotorActive = false;
            Machine.ShutterInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterInReadyState.Reset();
            }

        /// <inheritdoc />
        public override void ShutterDirectionReceived(ShutterDirection direction)
            {
            base.ShutterDirectionReceived(direction);
            if (direction == ShutterDirection.Opening)
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
            Machine.ShutterStepPosition = encoderPosition;
            Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void OpenShutter()
            {
            base.OpenShutter();
            Machine.ControllerActions.OpenShutter();
            Machine.TransitionToState(new OpeningState(Machine));
            }
        }
    }