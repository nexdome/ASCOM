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
            Settings.Default.Save();
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
            }

        private void BuildConnectionString()
            {
                BuildSerialConnectionString();
            }

        private void BuildSerialConnectionString()
            {
            var candidate = new SerialDeviceEndpoint(CommPortName.Text);
            Settings.Default.ConnectionString = candidate.ToString();
            }

        private void CommPortName_SelectedIndexChanged(object sender, EventArgs e)
            {
            BuildConnectionString();
            }
        }
    }