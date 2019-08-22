// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    ///     Rotation direction
    /// </summary>
    public enum RotationDirection
        {
        /// <summary>
        ///     Counterclockwise of left.
        /// </summary>
        CounterClockwise = -1,

        /// <summary>
        ///     Clockwise or right.
        /// </summary>
        Clockwise = 1,

        /// <summary>
        ///     Not rotating
        /// </summary>
        None = 0
        }
    }