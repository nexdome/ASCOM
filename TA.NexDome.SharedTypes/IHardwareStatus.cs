// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;

    public interface IHardwareStatus
        {
        /// <summary>
        ///     Indicates when the dome is in the Home Position. Note that the home position covers a
        ///     small range of encoder ticks and is not a single discrete value.
        ///     <seealso cref="HomePosition" />
        /// </summary>
        /// <value>true if the dome is in the Home Position, false otherwise.</value>
        bool AtHome { get; }

        /// <summary>
        ///     Number of azimuth ticks that the dome is expected to coast after the motors are stopped.
        /// </summary>
        /// <value>0 to 255</value>
        int Coast { get; }

        /// <summary>
        ///     Current dome azimuth in azimuth encoder ticks.
        /// </summary>
        /// <value>0 to 32767</value>
        int CurrentAzimuth { get; }

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
        ///     The state of the Dome Support Ring Swingout position sensor.
        ///     Only valid if <see cref="AtHome" /> is true. While <see cref="AtHome" />
        ///     is <c>false</c>, then this property will be <see cref="SensorState.Indeterminate" />.
        /// </summary>
        SensorState DsrSensor { get; }

        /// <summary>
        ///     The firmware version string.
        /// </summary>
        /// <value>String, format 'Vn' where n is a single digit</value>
        string FirmwareVersion { get; }

        /// <summary>
        ///     Position of clockwise edge of the home position, in azimuth encoder ticks.
        /// </summary>
        int HomeClockwise { get; }

        /// <summary>
        ///     Position of counter-clockwise edge of the home position, in azimuth encode ticks.
        /// </summary>
        int HomeCounterClockwise { get; }

        /// <summary>
        ///     The home position, in azimuth encoder ticks.
        /// </summary>
        /// <value>0 to 32767</value>
        int HomePosition { get; }

        /// <summary>
        ///     Relative humidity percentage
        /// </summary>
        /// <value>0 to 100 %</value>
        int Humidity { get; }

        /// <summary>
        ///     LX-200 Azimuth reading from LX-200 interface module (if present).
        /// </summary>
        /// <value>0 to 359 degrees, 999 if not available</value>
        int Lx200Azimuth { get; }

        /// <summary>
        ///     A verbatim copy of the raw status packet, as received from DDW hardware
        /// </summary>
        string RawStatusPacket { get; }

        /// <summary>
        ///     The state of the shutter position sensors.
        ///     <see cref="SensorState" />
        /// </summary>
        SensorState ShutterSensor { get; }

        /// <summary>
        ///     Dome Slave Mode (LX-200 interface)
        /// </summary>
        /// <value>true if Slaved, false if not slaved</value>
        bool Slaved { get; }

        /// <summary>
        ///     Snow sensor
        /// </summary>
        /// <value>0 (no snow) to 100 (sensor covered)</value>
        int Snow { get; }

        /// <summary>
        ///     Temperature
        /// </summary>
        /// <value>0 to 255 ADU</value>
        /// <remarks>
        ///     Degrees C = value - 100
        /// </remarks>
        int Temperature { get; }

        /// <summary>
        ///     Timestamp.
        /// </summary>
        /// <value>
        ///     A DateTime object recording the date and time that the status packet was parsed.
        /// </value>
        DateTime TimeStamp { get; }

        /// <summary>
        ///     The state of all of the user output pins
        /// </summary>
        /// <value>bitmap, 8 bits</value>
        Octet UserPins { get; }

        /// <summary>
        ///     The age of the weather information
        /// </summary>
        /// <value>0 to 254 minutes, 255 means expired</value>
        int WeatherAge { get; }

        /// <summary>
        ///     Wetness
        /// </summary>
        /// <value>0 (dry) to 100 (soaking wet)</value>
        int Wetness { get; }

        /// <summary>
        ///     Wind direction
        /// </summary>
        /// <value>0 to 255 ADU</value>
        /// <remarks>
        ///     Use the formula (value / 255) * 359 to compute bearing in degrees
        /// </remarks>
        int WindDirection { get; }

        /// <summary>
        ///     Peak wind speed
        /// </summary>
        /// <value>0 to 255 MPH</value>
        /// <remarks>
        ///     New undocumented field, missing from DDW manual
        /// </remarks>
        int WindPeak { get; }

        /// <summary>
        ///     Wind speed
        /// </summary>
        /// <value>0 to 255 MPH</value>
        int WindSpeed { get; }

        /// <summary>
        ///     Internal offset
        /// </summary>
        /// <remarks>
        ///     DDW attempts to position the dome ahead of telescope movement by this amount,
        ///     in order to keep the aperture well centered on the telescope optical axis.
        /// </remarks>
        /// <value>Unknown (angular degrees? Encoder ticks?)</value>
        int Offset { get; }
        }
    }