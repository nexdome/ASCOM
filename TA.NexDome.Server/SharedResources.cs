// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: SharedResources.cs  Last modified: 2020-07-20@16:33 by Tim Long

using System;
using System.Windows.Forms;
using Ninject;
using TA.NexDome.Server.Properties;
using TA.NexDome.SharedTypes;
using TA.Utils.Core.Diagnostics;

namespace TA.NexDome.Server
    {
    /// <summary>The resources shared by all drivers and devices.</summary>
    public static class SharedResources
        {
        public const string DomeDriverId = "ASCOM.NexDome.Dome";

        public const string DomeDriverName = "NexDome";

        public static ILog Log => CompositionRoot.Kernel.Get<ILog>();

        /// <summary>
        /// Used by logging to correlate all logs within one process execution
        /// </summary>
        /// <value>The correlator.</value>
        public static Guid LogCorrelator { get; } = Guid.NewGuid();

        static SharedResources()
            {
            ConnectionManager = CreateConnectionManager();
            }

        /// <summary>Gets the connection manager.</summary>
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

        public static ILog GetLogger(Guid clientId, string clientName)
            {
            var log = CompositionRoot.Kernel.Get<ILog>();
            log.WithAmbientProperty("client", clientId)
                .WithAmbientProperty("clientName", clientName);
            return log;
            }
        }
    }