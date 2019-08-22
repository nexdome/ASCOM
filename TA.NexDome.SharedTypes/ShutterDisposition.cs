// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    public enum ShutterDisposition
        {
        /// <summary>
        ///     Wireless link down
        /// </summary>
        Offline,

        /// <summary>
        ///     Moving towards open
        /// </summary>
        Opening,

        /// <summary>
        ///     Stationary and not closed
        /// </summary>
        Open,

        /// <summary>
        ///     Moving towards closed
        /// </summary>
        Closing,

        /// <summary>
        ///     Stationary and closed
        /// </summary>
        Closed
        }
    }