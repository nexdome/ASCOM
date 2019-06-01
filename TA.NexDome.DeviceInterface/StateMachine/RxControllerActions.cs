// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: RxControllerActions.cs  Last modified: 2018-03-24@22:27 by Tim Long

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
            channel.Send(Constants.CmdHardStopRotator);
            channel.Send(Constants.CmdHardStopShutter);
            }

        public void RotateToAzimuth(int degreesClockwiseFromNorth)
            {
            var cmd = string.Format(Constants.CmdGotoAzimuthTemplate, degreesClockwiseFromNorth);
            channel.Send(cmd);
            }

        public void OpenShutter()
            {
            channel.Send(Constants.CmdOpenShutter);
            }

        public void CloseShutter()
            {
            channel.Send(Constants.CmdCloseShutter);
            }

        public void RotateToHomePosition()
            {
            channel.Send(Constants.CmdGotoHome);
            }

        /// <inheritdoc />
        public void RequestRotatorStatus()
            {
            channel.Send(Constants.RequestRotatorStatus);
            }

        /// <inheritdoc />
        public void RequestShutterStatus()
            {
            channel.Send(Constants.RequestShutterStatus);
            }
        }
    }