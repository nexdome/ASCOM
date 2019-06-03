// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: SharedResources.cs  Last modified: 2018-03-28@22:20 by Tim Long

using System;
using System.Windows.Forms;
using NLog;

namespace TA.NexDome.Server
    {
    /// <summary>
    ///     The resources shared by all drivers and devices.
    /// </summary>
    public static class SharedResources
        {
        public const string DomeDriverId = "ASCOM.NexDome.Dome";
        public const string DomeDriverName = "NexDome";
        private static readonly ILogger Log;

        static SharedResources()
            {
            Log = LogManager.GetCurrentClassLogger();
            ConnectionManager = CreateConnectionManager();
            }

        /// <summary>
        ///     Gets the connection manager.
        /// </summary>
        /// <value>The connection manager.</value>
        public static ClientConnectionManager ConnectionManager { get; }


        public static void DoSetupDialog(Guid clientId)
            {
            var oldConnectionString = Properties.Settings.Default.ConnectionString;
            Log.Info($"SetupDialog requested by client {clientId}");
            using (var F = new SetupDialogForm())
                {
                var result = F.ShowDialog();
                switch (result)
                    {
                        case DialogResult.OK:
                            Log.Info($"SetupDialog successful, saving settings");
                            Properties.Settings.Default.Save();
                            var newConnectionString = Properties.Settings.Default.ConnectionString;
                            if (oldConnectionString != newConnectionString)
                                Log.Warn(
                                    $"Connection string has changed from {oldConnectionString} to {newConnectionString} - replacing the TansactionProcessorFactory");
                            break;
                        default:
                            Log.Warn("SetupDialog cancelled or failed - reverting to previous settings");
                            Properties.Settings.Default.Reload();
                            break;
                    }
                }
            }

        private static ClientConnectionManager CreateConnectionManager()
            {
            Log.Info("Creating ClientConnectionManager");
            return new ClientConnectionManager();
            }
        }
    }