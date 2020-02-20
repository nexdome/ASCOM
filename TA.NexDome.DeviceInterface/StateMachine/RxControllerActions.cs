// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.DeviceInterface.StateMachine
    {
    using System;
    using System.Linq;
    using System.Text;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.SharedTypes;

    public class RxControllerActions : IControllerActions
        {
        private readonly ICommunicationChannel channel;

        public RxControllerActions(ICommunicationChannel channel)
            {
            this.channel = channel;
            }

        public void RequestHardwareStatus()
            {
            RequestRotatorStatus();
            RequestShutterStatus();
            }

        public void PerformEmergencyStop()
            {
            SendCommand(Constants.CmdHardStopRotator);
            SendCommand(Constants.CmdHardStopShutter);
            }

        public void RotateToAzimuth(int degreesClockwiseFromNorth)
            {
            string cmd = string.Format(Constants.CmdGotoAzimuthTemplate, degreesClockwiseFromNorth);
            SendCommand(cmd);
            }

        public void RotateToStepPosition(int targetPosition)
            {
            string cmd = string.Format(Constants.CmdGotoStepPositionTemplate, targetPosition);
            SendCommand(cmd);
            }

        public void OpenShutter()
            {
            SendCommand(Constants.CmdOpenShutter);
            }

        public void CloseShutter()
            {
            SendCommand(Constants.CmdCloseShutter);
            }

        public void RotateToHomePosition()
            {
            SendCommand(Constants.CmdGotoHome);
            }

        /// <inheritdoc />
        public void RequestRotatorStatus()
            {
            SendCommand(Constants.RequestRotatorStatus);
            }

        /// <inheritdoc />
        public void RequestShutterStatus()
            {
            SendCommand(Constants.RequestShutterStatus);
            }

        /// <inheritdoc />
        public void SetHomeSensorPosition(int stepsFromTrueNorth)
            {
            string command = string.Format(Constants.CmdSetHomeSensorAzimuthTemplate, stepsFromTrueNorth);
            SendCommand(command);
            }

        /// <inheritdoc />
        public void SavePersistentSettings()
            {
            SendCommand(Constants.CmdSaveShutterSettings);
            SendCommand(Constants.CmdSaveRotatorSettings);
            }

        private void SendCommand(string command)
            {
            channel.Send(EnsureCommandEncapsulation(command));
            }

        private string EnsureCommandEncapsulation(string command)
            {
            const string lineTerminators = "\r\n";
            var builder = new StringBuilder();
            if (!command.StartsWith("@"))
                builder.Append('@');
            builder.Append(command);
            if (!lineTerminators.Contains(command.Last()))
                builder.Append(Environment.NewLine);
            return builder.ToString();
            }
        }
    }