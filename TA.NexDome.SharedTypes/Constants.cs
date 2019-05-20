namespace TA.NexDome.SharedTypes
{
    /// <summary>
    ///     Constants that apply to Digital DomeWorks hardware, software, firmware and settings. This struct includes
    ///     all of the commands used by the DD protocol plus other data and magic numbers.
    /// </summary>
    public struct Constants
    {
        /// <summary>
        ///     Diagnostic category used for DDW hardware interface methods and properties.
        /// </summary>
        internal const string StrDdw = "Digital DomeWorks";

        /// <summary>
        ///     The template for formatting the DDW version number.
        /// </summary>
        internal const string StrFirmwareTemplate = "DDW Firmware version: {0}";

        /// <summary>
        ///     Diagnostic category used for Serial I/O operations.
        /// </summary>
        internal const string StrSerIo = "Serial I/O";

        /// <summary>
        ///     The port name of the simulator serial port. This constant is used by
        ///     <see cref="DdwSetupControl" /> to append a phantom port to the end of the
        ///     list of available ports. This feature relies upon the conditional compilation
        ///     flag <code>SIMULATE_SERIALIO</code> being defined.
        /// </summary>
        internal const string StrSimulateSerial = "Simulator";

        /// <summary>
        ///     Hard-coded status response used when serial I/O is being simulated.
        /// </summary>
        internal const string StrSimulatedStatusResponse =
            "V4,437,427,2,419,0,1,0,1,422,433,7,128,255,255,255,255,255,255,255,68,3,0";

        #region DDW Command Codes

        /// <summary>
        ///     Command to stop all movement. The Digital Domeworks controller will actually accept
        ///     pretty much any spurious character for this purpose.
        /// </summary>
        public const string CmdEmergencyStop = "STOP\n";

        /// <summary>
        ///     DDW command to cancel the 4-minute auto-park timeout.
        /// </summary>
        public const string CmdCancelTimeout = "GSSK";

        /// <summary>
        ///     DDW command to close the shutter.
        /// </summary>
        public const string CmdClose = "GCLS";

        /// <summary>
        ///     DDW command to enable 'fast tracking' mode.
        /// </summary>
        public const string CmdFastTrack = "GTCK";

        /// <summary>
        ///     DDW "Get Info" command. Response is a GINF status packet.
        /// </summary>
        public const string CmdGetInfo = "GINF"; // Response is GINF status packet

        /// <summary>
        ///     Format string used with <see cref="string.Format(string,object)" /> for building a DDW "GoTo Azimuth" command.
        ///     Format Gxxx, 3 digits packed with leading zeroes as necessary.
        ///     <seealso cref="string.Format(string,object)" />
        ///     Response is a series of Pxxx position update packets, followed by a GINF status packet when
        ///     movement has ceased.
        /// </summary>
        public const string CmdGotoAz = "G{0:000}"; // GoTo Azimuth. Use with String.Format.

        /// <summary>
        ///     DDW command to move the dome to the home position.
        ///     Response is a series of Pxxx position update packets, followed by a GINF status packet.
        /// </summary>
        public const string CmdGotoHome = "GHOM";

        /// <summary>
        ///     DDW command to open the shutter.
        /// </summary>
        public const string CmdOpen = "GOPN";

        /// <summary>
        ///     DDW command to park the LX-200 telescope
        /// </summary>
        public const string CmdPark = "GSPK";

        /// <summary>
        ///     Format string used with String.Format for building a DDW command to configure the user output pins.
        ///     Format GPxx, where xx is a 2 digit hexadecimal number packed with leading zeroes as necessary.
        ///     <seealso cref="string.Format(string,object)" />
        ///     Response is a GINF status packet.
        /// </summary>
        public const string CmdSetUserPins = "GP{0:X2}";

        /// <summary>
        ///     DDW command to turn off LX-200 slave mode. Response is a GINF status packet.
        /// </summary>
        public const string CmdSlaveOff = "GVLS"; // Response is GINF status packet

        /// <summary>
        ///     DDW command to turn on LX-200 slave mode. Response is a GINF status packet.
        /// </summary>
        public const string CmdSlaveOn = "GSLV"; // Response is GINF status packet

        /// <summary>
        ///     DDW command to initiate diagnostic tests. Response is undefined.
        /// </summary>
        public const string CmdTest = "GTST";

        /// <summary>
        ///     DDW command to start the dome training sequence.
        /// </summary>
        public const string CmdTrain = "GTRN";

        /// <summary>
        ///     DDW command to unpark the LX-200 telescope.
        /// </summary>
        public const string CmdUnpark = "GSRK";

        #endregion

        #region DDW responses

        #endregion

        #region Other Constants and Magic Numbers

        /// <summary>
        ///     Delay after sending a blind command, in milliseconds.
        /// </summary>
        internal const int CSleepNoReply = 200; // ms to sleep after sending a blind command

        /// <summary>
        ///     The maximum amount of time that the Rx thread will wait for a reply to a transacted command
        ///     before raising an exception, in milliseconds.
        /// </summary>
        internal const int CTransactTimeout = 250; // ms transaction timeout

        /// <summary>
        ///     ASCII carriage return character
        /// </summary>
        internal const string CarriageReturn = "\x0D";

        /// <summary>
        ///     ASCII line feed character
        /// </summary>
        internal const string LineFeed = "\x0A";

        #endregion

        #region Custom Actions

        public const string ActionNameDsrSwingoutState = "DomeSupportRingSwingoutState";
        public const string ActionNameControllerStatus = "ControllerStatus";

        #endregion Custom Actions
    }
}