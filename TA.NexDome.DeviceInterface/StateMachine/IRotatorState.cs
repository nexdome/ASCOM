// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    using TA.NexDome.SharedTypes;

    public interface IRotatorState : IState
        {
        /// <summary>
        ///     Trigger: called to signal that dome rotation is detected. This can be triggered by a
        ///     dome rotation direction notification, or by an azimuth encoder tick. States are not
        ///     interested in the actual encoder position, only that movement is detected.
        /// </summary>
        void RotationDetected();

        /// <summary>
        ///     Trigger: called when a status report is received from the controller.
        /// </summary>
        /// <param name="status">An object containing the current hardware state.</param>
        void StatusUpdateReceived(IRotatorStatus status);

        /// <summary>
        ///     Requests that the dome rotate to the specified azimuth in degrees,
        ///     measured from North clockwise.
        /// </summary>
        void RotateToAzimuthDegrees(double azimuth);

        /// <summary>
        ///     Action: requests that the dome is rotated to the home position.
        /// </summary>
        void RotateToHomePosition();

        /// <summary>
        ///     Request a hardware status update.
        /// </summary>
        void RequestHardwareStatus();

        /// <summary>
        ///     Attempts to stop any rotation in progress as quickly as possible.
        /// </summary>
        void HardStopRequested();

        /// <summary>
        /// Requests that the dome rotate to the given step position in whole motor steps,
        /// where position zero is true north, increasing clockwise.
        /// </summary>
        /// <param name="targetStepPosition">The target step position.</param>
        void RotateToStepPosition(int targetStepPosition);
        }
    }