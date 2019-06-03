// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: AboutBox.cs  Last modified: 2018-09-03@17:00 by Tim Long

using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace TA.NexDome.Server
    {
    public partial class AboutBox : Form
        {
        public AboutBox()
            {
            InitializeComponent();
            }

        private void label1_Click(object sender, EventArgs e) { }

        private void AboutBox_Load(object sender, EventArgs e)
            {
            var me = Assembly.GetExecutingAssembly();
            var name = me.GetName();
            var driverVersion = name.Version;
            var productVersion = me.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
            DriverVersion.Text = driverVersion.ToString();
            InformationalVersion.Text = productVersion;
            }

        private void DriverVersion_Click(object sender, EventArgs e) { }

        private void NavigateToWebPage(object sender, EventArgs e)
            {
            var control = sender as Control;
            if (control == null)
                return;
            var url = control.Tag.ToString();
            if (!url.StartsWith("http:"))
                return;
            try
                {
                Process.Start(url);
                }
            catch (Exception)
                {
                // Just fail silently
                }
            }

        private void OkCommand_Click(object sender, EventArgs e)
            {
            Close();
            }
        }
    }