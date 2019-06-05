// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.
namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    /// Represents the information contained in a Shutter Status Event (SES)
    /// </summary>
    public interface IShutterStatus
        {
        /// <summary>
        ///     Current position in encoder ticks (closed = 0).
        /// </summary>
        int Position { get; }

        /// <summary>
        /// Gets the limit of travel (fully open position).
        /// </summary>
        /// <value>The limit of travel in whole steps.</value>
        int LimitOfTravel { get; }
        /// <summary>
        ///     True if the Open sensor is active (shutter fully open)
        /// </summary>
        bool OpenSensorActive { get; }
        /// <summary>
        /// True of the Closed sensor is active (shutter fully closed)
        /// </summary>
        bool ClosedSensorActive { get; }
        }
    }