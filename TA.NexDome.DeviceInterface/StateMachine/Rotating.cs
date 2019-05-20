// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: Rotating.cs  Last modified: 2018-03-30@03:23 by Tim Long

using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal sealed class Rotating : ControllerStateBase
        {
        private static readonly TimeSpan RotationTimeout = TimeSpan.FromSeconds(5);

        public Rotating(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            ResetTimeout(RotationTimeout);
            machine.AzimuthMotorActive = true;
            machine.AtHome = false;
            }

        public override void OnExit()
            {
            base.OnExit();
            machine.AzimuthMotorActive = false;
            machine.AzimuthDirection = RotationDirection.None;
            }

        /// <summary>
        ///     Trigger: updates the encoder position
        /// </summary>
        public override void RotationDetected() => ResetTimeout(RotationTimeout);

        /// <summary>
        ///     Trigger: invalid for this state.
        /// </summary>
        public override void ShutterMovementDetected()
            {
            base.ShutterMovementDetected();
            machine.TransitionToState(new ShutterMoving(machine));
            }

        /// <summary>
        ///     Trigger: => Ready.
        /// </summary>
        /// <param name="status"></param>
        public override void StatusUpdateReceived(IHardwareStatus status)
            {
            CancelTimeout();
            machine.TransitionToState(new Ready(machine));
            }

        protected override void HandleTimeout()
            {
            base.HandleTimeout();
            machine.TransitionToState(new RequestStatus(machine));
            }
        }
    }