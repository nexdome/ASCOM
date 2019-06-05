// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: CommunicationSettingsControl.cs  Last modified: 2018-03-28@22:20 by Tim Long

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.Server.Properties;
using SerialPort = System.IO.Ports.SerialPort;

namespace TA.NexDome.Server
    {
    public partial class CommunicationSettingsControl : UserControl
        {
        public CommunicationSettingsControl()
            {
            InitializeComponent();
            }

            public void Save()
                {
                BuildConnectionString();
                Settings.Default.Save();
                }

        protected static void BuildConnectionString()
            {
            /*  Connection String syntax for serial channel
                     COMXXX or comXXX where XXX = 1 to 999 - required
                     :bbbbbbb where bbbbbbb=100 to 9999999 for baud rate - optional
                     ,string where string in (None| Even| Odd| Mark| Space) for parity - optional
                     ,d where d= 7 or 8 for databits - optional
                     ,string where string in (Zero| OnePointFive| One| Two) for stopbits - optional
                     ,string where string in (dtr| nodtr) for DTR bit state - optional
                     ,string where string in (rts| norts) for RTS bit state - optional
                     ,string where string in (None|XOnXOff|RequestToSend|RequestToSendXOnXOff) for handshake - optional
                    */
            var dtr = Properties.Settings.Default.SerialAssertDTR ? "dtr" : "nodtr";
            var rts = Settings.Default.SerialAssertRTS ? "rts" : "norts";
            var connection =
                $"{Settings.Default.CommPortName}:{Settings.Default.SerialBaudRate},{Settings.Default.SerialParity},{Settings.Default.SerialDataBits},{Settings.Default.SerialStopBits},{dtr},{rts},{Settings.Default.SerialHandshake}";
            Settings.Default.ConnectionString = connection;
            }

        private void CommunicationSettingsControl_Load(object sender, EventArgs e)
            {
            var currentSelection = Settings.Default.CommPortName;
            var ports = new SortedSet<string>(SerialPort.GetPortNames());
            if (!ports.Contains(currentSelection)) ports.Add(currentSelection);
            CommPortName.Items.Clear();
            CommPortName.Items.AddRange(ports.ToArray());
            var currentIndex = CommPortName.Items.IndexOf(currentSelection);
            CommPortName.SelectedIndex = currentIndex;
            BuildConnectionString();
            }


        private void CommPortName_SelectedIndexChanged(object sender, EventArgs e)
            {
            BuildConnectionString();
            }
        }
    }