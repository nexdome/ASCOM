// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Server
    {
    using System;
    using System.Windows.Forms;

    using Ninject;

    using NLog;

    using TA.NexDome.Server.Properties;
    using TA.NexDome.SharedTypes;

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

        public static void SetParkPosition(decimal azimuth)
            {
            Settings.Default.ParkAzimuth = azimuth;
            var currentConfig = CompositionRoot.Kernel.Get<DeviceControllerOptions>();
            currentConfig.ParkAzimuth = azimuth;
            }

        public static void DoSetupDialog(Guid clientId)
            {
            string oldConnectionString = Settings.Default.ConnectionString;
            Log.Info($"SetupDialog requested by client {clientId}");
            using (var F = new SetupDialogForm())
                {
                var result = F.ShowDialog();
                switch (result)
                    {
                        case DialogResult.OK:
                            Log.Info("SetupDialog successful, saving settings");
                            Settings.Default.Save();
                            string newConnectionString = Settings.Default.ConnectionString;
                            if (oldConnectionString != newConnectionString)
                                Log.Warn(
                                    $"Connection string has changed from {oldConnectionString} to {newConnectionString} - replacing the TansactionProcessorFactory");
                            break;
                        default:
                            Log.Warn("SetupDialog cancelled or failed - reverting to previous settings");
                            Settings.Default.Reload();
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