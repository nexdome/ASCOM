// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine {
    public interface IShutterState : IState
        {

        /// <summary>
        ///     Trigger: called to signal that a shutter motor current measurement
        ///     has been received.
        /// </summary>
        void ShutterMovementDetected();

        /// <summary>
        ///     Trigger: called when a status report is received from the controller.
        /// </summary>
        /// <param name="status">An object containing the current hardware state.</param>
        void StatusUpdateReceived(IShutterStatus status);

        /// <summary>
        ///     Action: Open Shutter
        /// </summary>
        void OpenShutter();

        /// <summary>
        ///     Action: Close Shutter
        /// </summary>
        void CloseShutter();

        /// <summary>
        ///     Request a hardware status update.
        /// </summary>
        void RequestHardwareStatus();

        void LinkStateReceived(ShutterLinkState state);

        void ShutterDirectionReceived(ShutterDirection direction);
        }
    }