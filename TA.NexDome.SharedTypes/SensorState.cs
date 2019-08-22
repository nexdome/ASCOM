// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    ///     Positional sensor states
    /// </summary>
    /// <remarks>
    ///     Typically used where mechanical movement needs to be detected and a pair of reed-switch
    ///     sensors is used. One sensor detects the 'Open' position and another detects the 'Closed'
    ///     position. When neither sensor is active, then the moving apparatus is somewhere in
    ///     between the two positions.
    /// </remarks>
    public enum SensorState
        {
        /// <summary>
        ///     The position is indeterminate (neither the open nor the closed sensor is active)
        /// </summary>
        Indeterminate = 0,

        /// <summary>
        ///     The item being sensed is in the Closed position (closed sensor active)
        /// </summary>
        Closed = 1,

        /// <summary>
        ///     The item being sensed is in the Open position (open sensor active)
        /// </summary>
        Open = 2
        }
    }