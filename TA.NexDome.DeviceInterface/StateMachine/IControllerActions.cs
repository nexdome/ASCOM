// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    public interface IControllerActions
        {
        /// <summary>
        ///     Requests that the controller send a status report on the current state of the
        ///     hardware.
        /// </summary>
        void RequestHardwareStatus();

        /// <summary>
        ///     Instructs the controller to immediately stop all movement.
        /// </summary>
        void PerformEmergencyStop();

        /// <summary>
        ///     Requests that the controller rotate to the specified azimuth position.
        /// </summary>
        void RotateToAzimuth(int degreesClockwiseFromNorth);

        /// <summary>
        ///     Requests that the controller move to the specified step position.
        ///     The direction of rotation is at the controller's discretion.
        /// </summary>
        /// <param name="targetPosition">The target whole step position.</param>
        void RotateToStepPosition(int targetPosition);

        /// <summary>
        ///     Requests that the controller open the shutter.
        /// </summary>
        void OpenShutter();

        /// <summary>
        ///     Requests that the controller close the shutter.
        /// </summary>
        void CloseShutter();

        /// <summary>
        ///     Requests that the controller rotates to the azimuth that it considers to be the home position.
        /// </summary>
        void RotateToHomePosition();

        /// <summary>
        ///     Requests that the rotator controller send a status report on the current state of the rotator hardware.
        /// </summary>
        void RequestRotatorStatus();

        /// <summary>
        ///     Requests that the shutter controller send a status report on the current state of the shutter hardware.
        /// </summary>
        void RequestShutterStatus();

        /// <summary>
        ///     Sets the home sensor position in steps clockwise from true north.
        /// </summary>
        /// <param name="stepsFromTrueNorth">The azimuth in decimal degrees.</param>
        /// <remarks>
        ///     The home sensor position is defined as the number of steps clockwise, starting from true north,
        ///     that the dome must be rotated to activate the home sensor. The reason it is defined this way is that
        ///     the value is dependent on both the structure orientation (which determines the sensor position with
        ///     respect to true north) and the placement of the magnet around the dome (which determines the shutter
        ///     position with respect to the sensor). This definition encapsulates both variables in a single value.
        /// </remarks>
        void SetHomeSensorPosition(int stepsFromTrueNorth);

        void SavePersistentSettings();
        }
    }