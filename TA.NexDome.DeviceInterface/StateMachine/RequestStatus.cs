// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: RequestStatus.cs  Last modified: 2018-03-20@00:55 by Tim Long

using NLog.Fluent;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal sealed class RequestStatus : ControllerStateBase
        {
        public RequestStatus(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            machine.RequestHardwareStatus();
            }

        public override void RotationDetected()
            {
            Log.Warn()
                .Message("Rotation detected while expecting status. Issuing AllStop and re-requesting status.")
                .Write();
            EmergencyStopAndRequestStatus();
            }

        private void EmergencyStopAndRequestStatus()
            {
            machine.AllStop();
            machine.RequestHardwareStatus();
            }

        /// <summary>
        ///     This trigger is not valid in this state, so we basically ignore it.
        /// </summary>
        public override void ShutterMovementDetected()
            {
            Log.Warn()
                .Message("Shutter movement detected while expecting status. Issuing AllStop and re-requesting status.")
                .Write();
            EmergencyStopAndRequestStatus();
            }

        public override void StatusUpdateReceived(IHardwareStatus status)
            {
            machine.TransitionToState(new Ready(machine));
            }
        }
    }