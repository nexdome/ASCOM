// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ShutterMoving.cs  Last modified: 2018-03-20@00:56 by Tim Long

using System;
using NLog.Fluent;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal sealed class ShutterMoving : ControllerStateBase
        {
        /// <summary>
        ///     If no shutter movement indications are received for this long,
        ///     the state will time out and attempt to request a status update.
        /// </summary>
        private static readonly TimeSpan shutterTimeout = TimeSpan.FromSeconds(5);
        /// <summary>
        /// Records the moment when movement was first detected.
        /// </summary>
        private DateTime movementStartTime;
        /// <summary>
        /// Starts at <c>false</c> and transitions to <c>true</c> when the current draw
        /// exceeds a specified threshold.
        /// </summary>
        private bool currentDrawThresholdReached;

        public ShutterMoving(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            base.OnEnter();
            ResetTimeout(shutterTimeout);
            movementStartTime = machine.Clock.GetCurrentTime();
            currentDrawThresholdReached = false;
            machine.ShutterMotorActive = true;
            }

        public override void OnExit()
            {
            base.OnExit();
            machine.ShutterMotorCurrent = 0;
            machine.ShutterMotorActive = false;
            machine.ShutterMovementDirection = ShutterDirection.None;
            }

        public override void RotationDetected()
            {
            base.RotationDetected();
            Log.Error()
                .Message($"Invalid trigger: {nameof(RotationDetected)}")
                .Write();
            }

        public override void ShutterMovementDetected()
            {
            base.ShutterMovementDetected();
            ResetTimeout(shutterTimeout);
            InferShutterState(machine.ShutterMotorCurrent);
            }

        public override void StatusUpdateReceived(IHardwareStatus status)
            {
            base.StatusUpdateReceived(status);
            CancelTimeout();
            machine.TransitionToState(new Ready(machine));
            }

        private void InferShutterState(int shutterMotorCurrent)
            {
            var timeNow = machine.Clock.GetCurrentTime();
            var elapsedMoveTime = timeNow - movementStartTime;
            var elapsedSeconds = elapsedMoveTime.TotalSeconds;
            var minimumRequiredMoveSeconds = machine.Options.MaximumShutterCloseTime.TotalSeconds / 2;
            if (shutterMotorCurrent >= machine.Options.CurrentDrawDetectionThreshold)
                currentDrawThresholdReached = true;
            if (currentDrawThresholdReached && elapsedSeconds >= minimumRequiredMoveSeconds)
                {
                switch (machine.ShutterMovementDirection)
                    {
                        case ShutterDirection.Closing:
                            machine.InferredShutterPosition = SensorState.Closed;
                            break;
                        case ShutterDirection.Opening:
                            machine.InferredShutterPosition = SensorState.Open;
                            break;
                    }
                }
            else
                {
                machine.InferredShutterPosition = SensorState.Indeterminate;
                }
            }

        protected override void HandleTimeout()
            {
            base.HandleTimeout();
            machine.TransitionToState(new RequestStatus(machine));
            }
        }
    }