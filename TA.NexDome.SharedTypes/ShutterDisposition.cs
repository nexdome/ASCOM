using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.NexDome.SharedTypes
    {
    public enum ShutterDisposition
        {
        /// <summary>
        /// Wireless link down
        /// </summary>
        Offline,
        /// <summary>
        /// Moving towards open
        /// </summary>
        Opening,
        /// <summary>
        /// Stationary and not closed
        /// </summary>
        Open,
        /// <summary>
        /// Moving towards closed
        /// </summary>
        Closing,
        /// <summary>
        /// Stationary and closed
        /// </summary>
        Closed
        }
    }
