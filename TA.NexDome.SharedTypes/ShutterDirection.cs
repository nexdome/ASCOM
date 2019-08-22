// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    ///     Shutter movement directions. Note: ordinal values are important and are used for parsing
    ///     received data within <c>DeviceController</c>.
    /// </summary>
    public enum ShutterDirection
        {
        /// <summary>
        ///     The shutter is not moving, or the direction is unknown
        /// </summary>
        None = 0,

        /// <summary>
        ///     The shutter is closing.
        /// </summary>
        Closing = 1,

        /// <summary>
        ///     The shutter is opening.
        /// </summary>
        Opening = 2
        }
    }