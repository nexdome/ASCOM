using System;

namespace TA.NexDome.SharedTypes
{
    /// <summary>
    ///     An immutable class representing the state of the dome controller hardware at a point in time.
    /// </summary>
    public sealed class HardwareStatus : IHardwareStatus
    {
        /// <summary>
        ///     Indicates when the dome is in the Home Position.
        ///     Note that the home position covers a small range of encoder ticks and is not
        ///     a single discrete value.
        ///     <seealso cref="HomePosition" />
        /// </summary>
        /// <value>true if the dome is in the Home Position, false otherwise.</value>
        public bool AtHome { get; set; }

        /// <summary>
        ///     Number of azimuth ticks that the dome is expected to coast after the motors are stopped.
        /// </summary>
        /// <value>0 to 255</value>
        public int Coast { get; set; }

        /// <summary>
        ///     Current dome azimuth in azimuth encoder ticks.
        /// </summary>
        /// <value>0 to 32767</value>
        public int CurrentAzimuth { get; set; }

        /// <summary>
        ///     The dead zone is the minimum amount by which the dome will move. Any commanded rotation
        ///     that would result in a move smaller than the dead zone is ignored. This is designed
        ///     to prevent "hunting" to and fro past the desired encoder position (since it is
        ///     unlikely that the dome rotation will stop at the exact target position, due to
        ///     momentum). The size of the dead zone is set on DIP switches on the controller PCB.
        /// </summary>
        public int DeadZone { get; set; }

        /// <summary>
        ///     The dome circumference, measured in raw azimuth encoder ticks (1 tick ~ 1 inch).
        ///     This value is measured by the dome training algorithm and should not change unless
        ///     the firmware is re-trained.
        /// </summary>
        /// <value>0 to 32767</value>
        public int DomeCircumference { get; set; }

        /// <summary>
        ///     The state of the Dome Support Ring Swingout position sensor.
        ///     Only valid if <see cref="AtHome" /> is true. While <see cref="AtHome" />
        ///     is <c>false</c>, then this property will be <see cref="SensorState.Indeterminate" />.
        /// </summary>
        public SensorState DsrSensor { get; set; }

        /// <summary>
        ///     The firmware version string.
        /// </summary>
        /// <value>String, format 'Vn' where n is a single digit</value>
        public string FirmwareVersion { get; set; }

        /// <summary>
        ///     Position of clockwise edge of the home position, in azimuth encoder ticks.
        /// </summary>
        public int HomeClockwise { get; set; }

        /// <summary>
        ///     Position of counter-clockwise edge of the home position, in azimuth encode ticks.
        /// </summary>
        public int HomeCounterClockwise { get; set; }

        /// <summary>
        ///     The home position, in azimuth encoder ticks.
        /// </summary>
        /// <value>0 to 32767</value>
        public int HomePosition { get; set; }

        /// <summary>
        ///     Relative humidity percentage
        /// </summary>
        /// <value>0 to 100 %</value>
        public int Humidity { get; set; }

        /// <summary>
        ///     LX-200 Azimuth reading from LX-200 interface module (if present).
        /// </summary>
        /// <value>0 to 359 degrees, 999 if not available</value>
        public int Lx200Azimuth { get; set; }

        /// <summary>
        ///     A verbatim copy of the raw status packet, as received from DDW hardware
        /// </summary>
        public string RawStatusPacket { get; private set; }

        /// <summary>
        ///     The state of the shutter position sensors.
        ///     <see cref="SensorState" />
        /// </summary>
        public SensorState ShutterSensor { get; set; }

        /// <summary>
        ///     Dome Slave Mode (LX-200 interface)
        /// </summary>
        /// <value>true if Slaved, false if not slaved</value>
        public bool Slaved { get; set; }

        /// <summary>
        ///     Snow sensor
        /// </summary>
        /// <value>0 (no snow) to 100 (sensor covered)</value>
        public int Snow { get; set; }

        /// <summary>
        ///     Temperature
        /// </summary>
        /// <value>0 to 255 ADU</value>
        /// <remarks>
        ///     Degrees C = value - 100
        /// </remarks>
        public int Temperature { get; set; }

        /// <summary>
        ///     Timestamp.
        /// </summary>
        /// <value>
        ///     A value recording the date and time that the status packet was createdS.
        /// </value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        ///     The state of all of the user output pins
        /// </summary>
        /// <value>bitmap, 8 bits</value>
        public Octet UserPins { get; set; }

        /// <summary>
        ///     The age of the weather information
        /// </summary>
        /// <value>0 to 254 minutes, 255 means expired</value>
        public int WeatherAge { get; set; }

        /// <summary>
        ///     Wetness
        /// </summary>
        /// <value>0 (dry) to 100 (soaking wet)</value>
        public int Wetness { get; set; }

        /// <summary>
        ///     Wind direction
        /// </summary>
        /// <value>0 to 255 ADU</value>
        /// <remarks>
        ///     Use the formula (value / 255) * 359 to compute bearing in degrees
        /// </remarks>
        public int WindDirection { get; set; }

        /// <summary>
        ///     Peak wind speed
        /// </summary>
        /// <value>0 to 255 MPH</value>
        /// <remarks>
        ///     New undocumented field, missing from DDW manual
        /// </remarks>
        public int WindPeak { get; set; }

        /// <summary>
        ///     Wind speed
        /// </summary>
        /// <value>0 to 255 MPH</value>
        public int WindSpeed { get; set; }

        /// <summary>
        ///     Internal offset
        /// </summary>
        /// <remarks>
        ///     DDW attempts to position the dome ahead of telescope movement by this amount,
        ///     in order to keep the aperture well centered on the telescope optical axis.
        /// </remarks>
        /// <value>Unknown (angular degrees? Encoder ticks?)</value>
        public int Offset { get; set; }

        /// <summary>
        ///     Returns the status in the exact same format as a Digital DomeWorks
        ///     valid status packet, that could be transmitted over the serial port 'as-is'.
        /// </summary>
        /// <returns>
        ///     A <see cref="string" /> that represents this instance.
        /// </returns>
        public string ToDdwStatusString()
        {
            const string statusFormat =
                "{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22}";
            return string.Format(
                statusFormat,
                FirmwareVersion,
                DomeCircumference,
                HomePosition,
                Coast,
                CurrentAzimuth,
                Slaved ? 1 : 0,
                (int) ShutterSensor,
                (int) DsrSensor,
                AtHome ? 0 : 1,
                HomeCounterClockwise,
                HomeClockwise,
                (byte) UserPins,
                WeatherAge,
                WindDirection,
                WindSpeed,
                Temperature,
                Humidity,
                Wetness,
                Snow,
                WindPeak,
                Lx200Azimuth,
                DeadZone,
                Offset);
        }

        public override string ToString()
        {
            return
                $"{nameof(TimeStamp)}: {TimeStamp}, {nameof(CurrentAzimuth)}: {CurrentAzimuth}, {nameof(AtHome)}: {AtHome}, {nameof(ShutterSensor)}: {ShutterSensor}, {nameof(DsrSensor)}: {DsrSensor}, {nameof(DeadZone)}: {DeadZone}, {nameof(DomeCircumference)}: {DomeCircumference}, {nameof(HomeClockwise)}: {HomeClockwise}, {nameof(HomeCounterClockwise)}: {HomeCounterClockwise}, {nameof(HomePosition)}: {HomePosition}, {nameof(UserPins)}: {UserPins}, {nameof(Offset)}: {Offset}";
        }
    }
}