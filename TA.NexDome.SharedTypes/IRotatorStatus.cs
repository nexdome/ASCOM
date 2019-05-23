// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
{
    /// <summary>
    /// Represents the information contained in a Rotator Status Event (SER event).
    /// </summary>
    public interface IRotatorStatus
    {
        /// <summary>
        ///     Indicates when the dome is in the Home Position.
        ///     Note that the home position covers a small range of encoder ticks and is not
        ///     a single discrete value.
        ///     <seealso cref="HomePosition" />
        /// </summary>
        /// <value>true if the dome is in the Home Position, false otherwise.</value>
        bool AtHome { get; }

        /// <summary>
        ///     Current dome azimuth in azimuth encoder ticks.
        /// </summary>
        /// <value>Positive integer</value>
        int Azimuth { get; }

        /// <summary>
        ///     The dead zone is the minimum amount by which the dome will move. Any commanded rotation
        ///     that would result in a move smaller than the dead zone is ignored. This is designed
        ///     to prevent "hunting" to and fro past the desired encoder position (since it is
        ///     unlikely that the dome rotation will stop at the exact target position, due to
        ///     momentum). The size of the dead zone is set on DIP switches on the controller PCB.
        /// </summary>
        int DeadZone { get; }

        /// <summary>
        ///     The dome circumference, measured in raw azimuth encoder ticks (1 tick ~ 1 inch).
        ///     This value is measured by the dome training algorithm and should not change unless
        ///     the firmware is re-trained.
        /// </summary>
        /// <value>0 to 32767</value>
        int DomeCircumference { get; }

        /// <summary>
        ///     The home position, in azimuth encoder ticks clockwise from True North.
        /// </summary>
        /// <value>Positive integer</value>
        int HomePosition { get; }
    }
}