// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: INotifyHardwareStateChanged.cs  Last modified: 2018-03-17@15:07 by Tim Long

using System.ComponentModel;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface
    {
    /// <summary>
    ///     Properties that describe the current state of the Digital Domeworks controller.
    ///     All properties support change notifications via <see cref="INotifyPropertyChanged" />
    /// </summary>
    public interface INotifyHardwareStateChanged : INotifyPropertyChanged
        {
        /// <summary>
        ///     The current value of the azimuth encoder. Updates in real time as the dome rotates
        ///     and whenever a status report is received.
        /// </summary>
        int AzimuthEncoderPosition { get; }

        /// <summary>
        ///     A relative indication of the amount of current being drawn by the shutter motor.
        ///     Zero means the motor is not running and values up to about 30 are normal.
        ///     Updates about once per second during shutter movement and whenever a status report
        ///     is received.
        /// </summary>
        int ShutterMotorCurrent { get; }

        /// <summary>
        ///     The direction of rotation, if known. A value of <see cref="RotationDirection.None" />
        ///     does not necessarily indicate that the motor is inactive because under some
        ///     circumstances the rotation direction cannot be determined.
        /// </summary>
        RotationDirection AzimuthDirection { get; }

        /// <summary>
        ///     The direction of shutter travel, if known. A value of <see cref="ShutterDirection.None" />
        ///     does not necessarily indicate that the motor is inactive because under some
        ///     circumstances the direction of travel cannot be determined.
        /// </summary>
        ShutterDirection ShutterMovementDirection { get; }

        /// <summary>
        ///     Indicates whether the azimuth motor is energized.
        /// </summary>
        bool AzimuthMotorActive { get; }

        /// <summary>
        ///     Indicates whether the shutter motor is energized.
        /// </summary>
        bool ShutterMotorActive { get; }
        }
    }