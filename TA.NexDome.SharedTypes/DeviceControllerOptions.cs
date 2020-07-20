// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;

    /// <summary>
    ///     Data transfer object for passing configuration to the device controller
    /// </summary>
    public class DeviceControllerOptions
        {
        /// <summary>
        /// Estimated maximum time to close the shutter (used for safety timeouts)
        /// </summary>
        public TimeSpan MaximumShutterCloseTime { get; set; }

        /// <summary>
        /// Estimated maximum time for one full dome rotation (used for safety timeouts)
        /// </summary>
        public TimeSpan MaximumFullRotationTime { get; set; }

        /// <summary>
        /// Maximum time between shutter position updates before it is assumed
        /// that the shutter has stopped moving.
        /// </summary>
        public TimeSpan ShutterTickTimeout { get; set; }

        /// <summary>
        /// Maximum time between rotator position updates before it is assumed
        /// that the rotator has stopped moving.
        /// </summary>
        public TimeSpan RotatorTickTimeout { get; set; }

        /// <summary>
        /// The configured home sensor azimuth
        /// </summary>
        public decimal HomeAzimuth { get; set; }
        /// <summary>
        /// The configured Park azimuth, which should be the position where the battery charger
        /// contacts engage with the closed shutter.
        /// </summary>
        /// <value>The park azimuth.</value>
        public decimal ParkAzimuth { get; set; }
        /// <summary>
        /// User configured maximum movement speed
        /// </summary>
        public int RotatorMaximumSpeed { get; set; }

        /// <summary>
        /// User configured maximum movement speed
        /// </summary>
        public int ShutterMaximumSpeed { get; set; }
        /// <summary>
        /// User configured acceleration ramp time
        /// </summary>
        public TimeSpan RotatorRampTime { get; set; }

        /// <summary>
        /// User configured acceleration ramp time
        /// </summary>
        public TimeSpan ShutterRampTime { get; set; }

        /// <summary>
        /// The maximum time to wait for the shutter state machine to become "ready"
        /// upon connecting, before declaring a shutter error.
        /// </summary>
        public TimeSpan TimeToWaitForShutterOnConnect { get; set; }

        /// <summary>
        /// Determines whether the driver attempts to control the shutter, which may or may not be
        /// installed as the shutter kit is sold separately from the rotator kit.
        /// </summary>
        /// <value><c>true</c> if shutter control should be enabled; otherwise, <c>false</c>.</value>
        public bool ShutterIsInstalled { get; set; }

        /// <summary>
        /// Determines whether the shutter auto-close mechanism will be configured on connecting.
        /// Requires firmware 3.4.0.
        /// </summary>
        /// <value><c>true</c> if [enable automatic close on low battery]; otherwise, <c>false</c>.</value>
        public bool EnableAutoCloseOnLowBattery { get; set; }
        /// <summary>
        /// The value to be used by the shutter (in analog-to-digital-units) to determine when
        /// the battery is low.
        /// </summary>
        public Decimal ShutterLowBatteryThresholdVolts { get; set; }
        /// <summary>
        /// The amount of time without a "battery low" notification before the charge level is
        /// considered nominal again
        /// </summary>
        public TimeSpan ShutterLowVoltsNotificationTimeToLive { get; set; }
        /// <summary>
        /// If <c>true</c> the device controller waits for the shutter to report that it is
        /// online before returning from the Open() method. If <c>false</c> then the shutter is
        /// assumed to be closed and offline and Open() returns immediately.
        /// </summary>
        public bool ShutterWaitForReadyOnConnect { get; set; }
        }
    }