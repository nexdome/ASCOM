namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    ///     Constants that apply to hardware, software, firmware and settings. This struct includes
    ///     all of the commands used by the communications protocol plus other data and magic numbers.
    /// </summary>
    public class Constants
        {
        public const string RotatorStatusReply = ":SER,";

        public const string ShutterStatusReply =":SES,";

        #region NexDome Responses

        public const string RequestRotatorStatus = "@SRR";
        public const string RequestShutterStatus = "@SRS";
        #endregion

        #region NexDome Command Codes
        public const string CmdHardStopShutter = "SWS";
        public const string CmdHardStopRotator = "SWR";
        public const string CmdOpenShutter = "OPS";
        public const string CmdCloseShutter = "CLS";

        /// <summary>
        ///     Format string used with <see cref="string.Format(string,object)" /> for building a "GoTo Azimuth" command.
        ///     Format Gxxx, 3 digits packed with leading zeroes as necessary.
        ///     <seealso cref="string.Format(string,object)" />
        ///     Response is a series of Pxxx position update packets, followed by a GINF status packet when
        ///     movement has ceased.
        /// </summary>
        public const string CmdGotoAzimuthTemplate = "GAR,{0:000}";

        /// <summary>
        ///     Command to move the dome to the home position.
        ///     Response is a series of Pxxx position update packets, followed by a SER status packet.
        /// </summary>
        public const string CmdGotoHome = "GHR";

        #endregion

        #region Custom Actions
        #endregion Custom Actions

        }
    }