// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: Uninitialized.cs  Last modified: 2018-03-18@16:59 by Tim Long

using System;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    internal class Uninitialized : IControllerState
        {
        private readonly InvalidOperationException uninitialized =
            new InvalidOperationException("Call Initialize() before using the state machine");

        public void OnEnter()
            {
            throw uninitialized;
            }

        public void OnExit() { }

        public void RotationDetected()
            {
            throw uninitialized;
            }

        public void ShutterMovementDetected()
            {
            throw uninitialized;
            }

        public void StatusUpdateReceived(IHardwareStatus status)
            {
            throw uninitialized;
            }

        /// <inheritdoc />
        public void StatusUpdateReceived(IRotatorStatus status)
            {
            throw uninitialized;
            }

        public string Name => nameof(Uninitialized);

        /// <inheritdoc />
        public void StatusUpdateReceived(IShutterStatus status)
            {
            throw uninitialized;
            }

        public void RotateToAzimuthDegrees(double azimuth)
            {
            throw uninitialized;
            }

        public void OpenShutter()
            {
            throw uninitialized;
            }

        public void CloseShutter()
            {
            throw uninitialized;
            }

        public void RotateToHomePosition()
            {
            throw uninitialized;
            }

        public void SetUserOutputPins(Octet newState)
            {
            throw uninitialized;
            }

        public void RequestHardwareStatus()
            {
            throw uninitialized;
            }
        }
    }