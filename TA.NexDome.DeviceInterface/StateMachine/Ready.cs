// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: Ready.cs  Last modified: 2018-03-20@01:00 by Tim Long

using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal sealed class Ready : ControllerStateBase
        {
        public Ready(ControllerStateMachine machine) : base(machine) { }

        public override void OnEnter()
            {
            machine.ResetKeepAliveTimer();
            machine.InReadyState.Set();
            }

        public override void OnExit()
            {
            machine.InReadyState.Reset();
            machine.ResetKeepAliveTimer();
            }

        public override void RotationDetected()
            {
            machine.TransitionToState(new Rotating(machine));
            }

        public override void ShutterMovementDetected()
            {
            machine.TransitionToState(new ShutterMoving(machine));
            }

        public override void RotateToAzimuthDegrees(double azimuth)
            {
            base.RotateToAzimuthDegrees(azimuth);
            machine.ControllerActions.RotateToAzimuth((int) azimuth);
            machine.TransitionToState(new Rotating(machine));
            }

        public override void OpenShutter()
            {
            base.OpenShutter();
            machine.ShutterMovementDirection = ShutterDirection.Opening;
            machine.ControllerActions.OpenShutter();
            machine.TransitionToState(new Rotating(machine));
            }

        public override void CloseShutter()
            {
            base.CloseShutter();
            machine.ShutterMovementDirection = ShutterDirection.Closing;
            machine.ControllerActions.CloseShutter();
            machine.TransitionToState(new Rotating(machine));
            }

        public override void RotateToHomePosition()
            {
            base.RotateToHomePosition();
            machine.ControllerActions.RotateToHomePosition();
            machine.TransitionToState(new Rotating(machine));
            }

        public override void SetUserOutputPins(Octet newState)
            {
            base.SetUserOutputPins(newState);
            machine.ControllerActions.SetUserOutputPins(newState);
            }

        public override void RequestHardwareStatus()
            {
            base.RequestHardwareStatus();
            machine.ResetKeepAliveTimer();
            machine.RequestHardwareStatus();
            }
        }
    }