// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    ///     Constants that apply to hardware, software, firmware and settings. This struct includes
    ///     all of the commands used by the communications protocol plus other data and magic
    ///     numbers.
    /// </summary>
    public class Constants
        {
        #region NexDome Responses
        public const string RotatorStatusReply = ":SER,";

        public const string ShutterStatusReply = ":SES,";

        public const string BatteryVoltageNotification = ":BV";
        #endregion

        #region NexDome Command Codes
        public const string CommandPrefix = "@";

        public const string RequestRotatorStatus = "@SRR";

        public const string RequestShutterStatus = "@SRS";

        public const string CmdHardStopShutter = "SWS";

        public const string CmdHardStopRotator = "SWR";

        public const string CmdOpenShutter = "OPS";

        public const string CmdCloseShutter = "CLS";

        public const string CmdSaveShutterSettings = "@ZWS";

        public const string CmdSaveRotatorSettings = "@ZWR";

        public const string CmdSetHomeSensorAzimuthTemplate = "@HWR,{0:D}";

        public const string CmdGetRotatorVersion = "@FRR";

        public const string CmdGetShutterVersion = "@FRS";

        public const string CmdSetMotorSpeedTemplate = "@VW{0},{1:0000}";

        public const string CmdSetRampTimeTemplate = "@AW{0},{1:0000}";

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

        #region Magic Numbers
        public const float BatteryFullyChargedVolts = 12.72f;

        public const float BatteryHalfChargedVolts = 12.18f;

        public const float BatteryFullyDischargedVolts = 10.2f;
        #endregion
        }
    }