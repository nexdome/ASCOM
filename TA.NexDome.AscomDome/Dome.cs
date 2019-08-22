// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.AscomDome
    {
    using System;
    using System.Collections;
    using System.Diagnostics.Contracts;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using ASCOM;
    using ASCOM.DeviceInterface;
    using JetBrains.Annotations;
    using Machine.Specifications.Runner.Impl;
    using NLog.Fluent;
    using static SharedTypes.LogHelper;
    using TA.NexDome.DeviceInterface;
    using TA.NexDome.Server;
    using TA.NexDome.SharedTypes;
    using InvalidOperationException = ASCOM.InvalidOperationException;
    using NotImplementedException = ASCOM.NotImplementedException;

    [ProgId(SharedResources.DomeDriverId)]
    [Guid("32f049d8-fac4-45df-84af-077f01d0d4e1")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [DeviceId(SharedResources.DomeDriverId, DeviceName = SharedResources.DomeDriverName)]
    [ServedClassName(SharedResources.DomeDriverName)]
    public class Dome : ReferenceCountedObject, IDomeV2, IDisposable
        {
        private readonly Guid clientId;

        private DeviceController controller;

        private bool disposed;

        public Dome()
            {
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.DomeDriverId);
            }

        private bool IsShutterAvailable => controller?.IsShutterOperational ?? false;

        /// <inheritdoc />
        public void SetupDialog() => SharedResources.DoSetupDialog(clientId);

        /// <inheritdoc />
        public string Action(string ActionName, string ActionParameters) => null;

        /// <inheritdoc />
        public void CommandBlind(string Command, bool Raw = false) => throw MethodNotImplemented();

        private Exception MethodNotImplemented([CallerMemberName] string method = "") =>
            LogAndBuild<MethodNotImplementedException>("Method {method} is not implemented", method);

        /// <inheritdoc />
        public bool CommandBool(string Command, bool Raw = false) => throw MethodNotImplemented();

        /// <inheritdoc />
        public string CommandString(string Command, bool Raw = false) => throw MethodNotImplemented();

        /// <inheritdoc />
        public void Dispose()
            {
            if (disposed) return;
            try
                {
                ReleaseUnmanagedResources();
                GC.SuppressFinalize(this);
                }
            finally
                {
                disposed = true;
                }
            }

        /// <inheritdoc />
        public void AbortSlew() => controller.RequestEmergencyStop();

        /// <inheritdoc />
        public void CloseShutter()
            {
            if (!IsShutterAvailable)
                LogAndThrow<InvalidOperationException>(
                    "Attempt to close the shutter while the shutter is not available");
            controller?.CloseShutter();
            }

        /// <inheritdoc />
        public void FindHome() => controller?.RotateToHomePosition();

        /// <inheritdoc />
        public void OpenShutter()
            {
            if (!IsShutterAvailable)
                LogAndThrow<InvalidOperationException>("Attempt to open the shutter while the shutter not available");
            controller?.OpenShutter();
            }

        /// <inheritdoc />
        public async void Park()
            {
            if (!Connected) 
                LogAndThrow<NotConnectedException>("The driver must be connected in order to park");

            // It is important to catch any and all exceptions because we are using async void
            // and any unhandled exception would crash the process.
            try
                {
                await controller.Park().ContinueOnAnyThread();
                }
            // ReSharper disable once CatchAllClause
            catch (Exception e)
                {
                Log.Error().Exception(e)
                    .Message("Error during asynchronous park: {message}", e.Message)
                    .Write();
                }
            }

        /// <inheritdoc />
        public void SetPark() => SharedResources.SetParkPosition((decimal)controller.AzimuthDegrees);

        /// <inheritdoc />
        public void SlewToAltitude(double altitude) => throw MethodNotImplemented();

        /// <inheritdoc />
        public void SlewToAzimuth(double Azimuth) => controller?.SlewToAzimuth(Azimuth);

        /// <inheritdoc />
        public void SyncToAzimuth(double Azimuth) => throw MethodNotImplemented();

        /// <inheritdoc />
        public bool Connected
            {
            get => controller?.IsConnected ?? false;
            set
                {
                if (value) controller = SharedResources.ConnectionManager.GoOnline(clientId);
                else
                    {
                    SharedResources.ConnectionManager.GoOffline(clientId);
                    controller = null; // [Sentinel]
                    }
                }
            }

        /// <inheritdoc />
        public string Description => "NexDome Control System";

        /// <inheritdoc />
        public string DriverInfo
            {
            get
                {
                var builder = new StringBuilder();
                builder.AppendLine(Description);
                builder.AppendLine("Professionally produced for NexDome by Tigra Astronomy");
                builder.AppendLine($"Build {GitVersionExtensions.GitInformationalVersion}");
                return builder.ToString();
                }
            }

        /// <inheritdoc />
        public string DriverVersion => $"{GitVersionExtensions.GitMajorVersion}.{GitVersionExtensions.GitMinorVersion}";

        /// <inheritdoc />
        public short InterfaceVersion => 2;

        /// <inheritdoc />
        public string Name => SharedResources.DomeDriverName;

        /// <inheritdoc />
        public ArrayList SupportedActions => new ArrayList();

        /// <inheritdoc />
        public double Altitude => controller?.ShutterPercentOpen * 90.0 ?? 0.0;

        /// <inheritdoc />
        public bool AtHome => controller?.AtHome ?? false;

        /// <inheritdoc />
        public bool AtPark => controller?.AtPark ?? false;

        /// <inheritdoc />
        public double Azimuth => controller.AzimuthDegrees;

        /// <inheritdoc />
        public bool CanFindHome => true;

        /// <inheritdoc />
        public bool CanPark => true;

        /// <inheritdoc />
        public bool CanSetAltitude => false;

        /// <inheritdoc />
        public bool CanSetAzimuth => true;

        /// <inheritdoc />
        public bool CanSetPark => true;

        /// <inheritdoc />
        public bool CanSetShutter => IsShutterAvailable;

        /// <inheritdoc />
        public bool CanSlave => false;

        /// <inheritdoc />
        public bool CanSyncAzimuth => false;

        /// <inheritdoc />
        public ShutterState ShutterStatus
            {
            get
                {
                switch (controller?.ShutterDisposition ?? ShutterDisposition.Offline)
                    {
                        case ShutterDisposition.Offline:
                            return ShutterState.shutterError;
                        case ShutterDisposition.Opening:
                            return ShutterState.shutterOpening;
                        case ShutterDisposition.Open:
                            return ShutterState.shutterOpen;
                        case ShutterDisposition.Closing:
                            return ShutterState.shutterClosing;
                        case ShutterDisposition.Closed:
                            return ShutterState.shutterClosed;
                        default:
                            return ShutterState.shutterError;
                    }
                }
            }

        /// <inheritdoc />
        public bool Slaved
            {
            get => false;
            set
                {
                throw new PropertyNotImplementedException(
                    "Slaving is not supported in this driver. Use POTH or ASCOM Dome Control Panel.",
                    true);
                }
            }

        /// <inheritdoc />
        public bool Slewing => controller?.IsMoving ?? false;

        private void ReleaseUnmanagedResources()
            {
            if (disposed) return;
            SharedResources.ConnectionManager.GoOffline(clientId);
            SharedResources.ConnectionManager.UnregisterClient(clientId);
            }

        /// <inheritdoc />
        ~Dome() => ReleaseUnmanagedResources();
        }
    }