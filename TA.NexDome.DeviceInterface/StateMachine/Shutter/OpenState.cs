// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine.Shutter
    {
    using TA.NexDome.SharedTypes;

    internal class OpenState : ShutterStateBase
        {
        /// <inheritdoc />
        public OpenState(ControllerStateMachine machine)
            : base(machine) { }

        /// <inheritdoc />
        public override void OnEnter()
            {
            base.OnEnter();
            Machine.ShutterDisposition = ShutterDisposition.Open;
            Machine.ShutterMotorActive = false;
            Machine.ShutterInReadyState.Set();
            }

        /// <inheritdoc />
        public override void OnExit()
            {
            base.OnExit();
            Machine.ShutterInReadyState.Reset();
            }

        public override void ShutterDirectionReceived(ShutterDirection direction)
            {
            base.ShutterDirectionReceived(direction);
            if (direction == ShutterDirection.Closing)
                Machine.TransitionToState(new ClosingState(Machine));
            else
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void EncoderTickReceived(int encoderPosition)
            {
            base.EncoderTickReceived(encoderPosition);
            int oldPosition = Machine.ShutterStepPosition;
            Machine.ShutterStepPosition = encoderPosition;
            if (encoderPosition < oldPosition)
                Machine.TransitionToState(new ClosingState(Machine));
            else
                Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void OpenShutter()
            {
            base.OpenShutter();
            Machine.ControllerActions.OpenShutter();
            Machine.TransitionToState(new OpeningState(Machine));
            }

        /// <inheritdoc />
        public override void CloseShutter()
            {
            base.CloseShutter();
            Machine.ControllerActions.CloseShutter();
            Machine.TransitionToState(new ClosingState(Machine));
            }
        }
    }