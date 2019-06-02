// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: RxControllerActions.cs  Last modified: 2018-03-24@22:27 by Tim Long

using System;
using System.Linq;
using System.Text;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.DeviceInterface.StateMachine
    {
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
            var cmd = string.Format(Constants.CmdGotoAzimuthTemplate, degreesClockwiseFromNorth);
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

        private void SendCommand(string command)
            {
            channel.Send(EnsureCommandEncapsulation(command));
            }

        private string EnsureCommandEncapsulation(string command)
            {
            const string lineTerminators = "\r\n";
            StringBuilder builder = new StringBuilder();
            if (!command.StartsWith("@"))
                builder.Append('@');
            builder.Append(command);
            if (!lineTerminators.Contains(command.Last()))
                builder.Append(Environment.NewLine);
            return builder.ToString();
            }
        }
    }