// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: StateLoggingDecorator.cs  Last modified: 2018-03-30@02:02 by Tim Long

using System.Diagnostics.Contracts;
using NLog.Fluent;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal sealed class StateLoggingDecorator : IControllerState
        {
        private readonly IControllerState decoratedState;

        public StateLoggingDecorator(IControllerState targetState)
            {
            decoratedState = targetState;
            }

        [ContractInvariantMethod]
        private void ObjectInvariant()
            {
            Contract.Invariant(decoratedState != null);
            }

        public string Name => decoratedState.Name;

        public void RotateToAzimuthDegrees(double azimuth)
            {
            Log.Info()
                .Message($"Rotate to azimuth {azimuth}° requested")
                .Write();
            decoratedState.RotateToAzimuthDegrees(azimuth);
            }

        public void OpenShutter()
            {
            Log.Info()
                .Message("Open Shutter requested")
                .Write();
            decoratedState.OpenShutter();
            }

        public void CloseShutter()
            {
            Log.Info()
                .Message("Close Shutter requested")
                .Write();
            decoratedState.CloseShutter();
            }

        public void RotateToHomePosition()
            {
            Log.Info()
                .Message("Rotate To Home requested")
                .Write();
            decoratedState.RotateToHomePosition();
            }

        public void SetUserOutputPins(Octet newState)
            {
            Log.Info()
                .Message($"User output pins set to {newState}")
                .Write();
            decoratedState.SetUserOutputPins(newState);
            }

        public void OnEnter()
            {
            Log.Info()
                .Message($"Entering state {decoratedState.Name}")
                .Write();
            decoratedState.OnEnter();
            }

        public void OnExit()
            {
            Log.Info()
                .Message($"Exiting state {decoratedState.Name}")
                .Write();
            decoratedState.OnExit();
            }

        public void RotationDetected()
            {
            Log.Info()
                .Message($"[{decoratedState.Name}] Trigger: Rotation detected")
                .Write();
            decoratedState.RotationDetected();
            }

        public void ShutterMovementDetected()
            {
            Log.Info()
                .Message($"[{decoratedState.Name}] Trigger: Shutter movement detected")
                .Write();
            decoratedState.ShutterMovementDetected();
            }

        public void StatusUpdateReceived(IHardwareStatus status)
            {
            Log.Info()
                .Message($"[{decoratedState.Name}] Trigger: Status update")
                .Property("status", status)
                .Write();
            decoratedState.StatusUpdateReceived(status);
            }

        public void RequestHardwareStatus()
            {
            Log.Info()
                .Message($"[{decoratedState.Name}] Action: Request hardware status")
                .Write();
            decoratedState.RequestHardwareStatus();
            }
        }
    }