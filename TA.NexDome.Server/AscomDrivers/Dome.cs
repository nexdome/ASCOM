// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2016-2019 Tigra Astronomy, all rights reserved.
//
// File: Dome.cs  Last modified: 2019-10-07@19:37 by Tim Long

using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ASCOM;
using ASCOM.DeviceInterface;
using TA.NexDome.Common;
using TA.NexDome.DeviceInterface;
using TA.PostSharp.Aspects;
using TA.Utils.Core;
using TA.Utils.Core.Diagnostics;
using InvalidOperationException = ASCOM.InvalidOperationException;

namespace TA.NexDome.Server.AscomDrivers
    {
    using static LogHelper;

    [ProgId(SharedResources.DomeDriverId)]
    [Guid("32f049d8-fac4-45df-84af-077f01d0d4e1")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [DeviceId(SharedResources.DomeDriverId, DeviceName = SharedResources.DomeDriverName)]
    [ServedClassName(SharedResources.DomeDriverName)]
    [TraceWithArguments]
    public class Dome : ReferenceCountedObject, IDomeV2, IDisposable, IAscomDriver
        {
        private readonly Guid clientId;

        private DeviceController controller;

        private bool disposed;
        private readonly ILog log;

        public Dome()
            {
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.DomeDriverId);
            log = SharedResources.GetLogger(clientId, SharedResources.DomeDriverId);
            }

        /// <inheritdoc />
        ~Dome() => ReleaseManagedResources();

        /// <inheritdoc />
        public double Altitude => controller?.ShutterPercentOpen * 90.0 ?? 0.0;

        /// <inheritdoc />
        public bool AtHome => controller?.AtHome ?? false;

        /// <inheritdoc />
        public bool AtPark => controller?.AtPark ?? false;

        /// <inheritdoc />
        [MustBeConnected]
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
        public bool CanSyncAzimuth => true;

        /// <inheritdoc />
        public bool Connected
            {
            get => controller?.IsConnected ?? false;
            set
                {
                if (value) controller = SharedResources.ConnectionManager.GoOnline(clientId).SingleOrDefault();
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
                builder.AppendLine($"Build {GitVersion.GitInformationalVersion}");
                return builder.ToString();
                }
            }

        /// <inheritdoc />
        public string DriverVersion => $"{GitVersion.GitMajorVersion}.{GitVersion.GitMinorVersion}";

        /// <inheritdoc />
        public short InterfaceVersion => 2;

        /// <inheritdoc />
        public string Name => SharedResources.DomeDriverName;

        /// <inheritdoc />
        [MustBeConnected]
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
            set =>
                throw new PropertyNotImplementedException(
                    "Slaving is not supported in this driver. Use POTH or ASCOM Dome Control Panel.",
                    true);
            }

        /// <inheritdoc />
        public bool Slewing => controller?.IsMoving ?? false;

        /// <inheritdoc />
        public ArrayList SupportedActions => new ArrayList();

        private bool IsShutterAvailable => controller?.IsShutterOperational ?? false;

        /// <inheritdoc />
        public void AbortSlew() => controller.RequestEmergencyStop();

        /// <inheritdoc />
        public string Action(string ActionName, string ActionParameters) => null;

        /// <inheritdoc />
        [MustBeConnected]
        public void CloseShutter()
            {
            if (!IsShutterAvailable)
                LogAndThrow<InvalidOperationException>(log,
                    "Attempt to close the shutter while the shutter is not available");
            controller?.CloseShutter();
            }

        /// <inheritdoc />
        public void CommandBlind(string Command, bool Raw = false) => throw MethodNotImplemented();

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
                ReleaseManagedResources();
                GC.SuppressFinalize(this);
                }
            finally
                {
                disposed = true;
                }
            }

        /// <inheritdoc />
        [MustBeConnected]
        public void FindHome() => controller?.RotateToHomePosition();

        /// <inheritdoc />
        [MustBeConnected]
        public void OpenShutter()
            {
            if (!IsShutterAvailable)
                LogAndThrow<InvalidOperationException>(log,
                    "Attempt to open the shutter while the shutter not available");
            controller?.OpenShutter();
            }

        /// <inheritdoc />
        [MustBeConnected]
        public void Park()
            {
            if (!Connected)
                LogAndThrow<NotConnectedException>(log,
                    "The driver must be connected in order to park");

            // It is important to catch any and all exceptions because we are using async void
            // and any unhandled exception would crash the process.
            try
                {
                Task.Run(() => controller.Park()).ContinueOnAnyThread();
                }
            // ReSharper disable once CatchAllClause
            catch (Exception e)
                {
                log.Error().Exception(e)
                    .Message("Error during asynchronous park: {message}", e.Message)
                    .Write();
                }
            }

        /// <inheritdoc />
        [MustBeConnected]
        public void SetPark() => SharedResources.SetParkPosition((decimal)controller.AzimuthDegrees);

        /// <inheritdoc />
        public void SetupDialog() => SharedResources.DoSetupDialog(clientId);

        /// <inheritdoc />
        [MustBeConnected]
        public void SlewToAzimuth(double azimuth)
            {
            if (azimuth < 0.0 || azimuth >= 360.0)
                {
                var exception = new InvalidValueException(
                    nameof(SlewToAzimuth),
                    azimuth.ToString(),
                    "Expected 0.0 <= azimuth < 360.0");
                log.Error().Message("Azimuth {azimuth} was outside the allowed range", azimuth)
                    .Exception(exception)
                    .Write();
                throw exception;
                }
            controller?.SlewToAzimuth(azimuth);
            }

        /// <inheritdoc />
        public void SlewToAltitude(double altitude) => throw MethodNotImplemented();

        /// <inheritdoc />
        [MustBeConnected]
        public void SyncToAzimuth(double azimuth)
            {
            if (azimuth < 0.0 || azimuth >= 360.0)
                throw new InvalidValueException(
                    nameof(SyncToAzimuth),
                    azimuth.ToString(),
                    "Expected 0.0 <= azimuth < 360.0");
            controller?.SyncAzimuth(azimuth);
            }

        private Exception MethodNotImplemented([CallerMemberName] string method = "")
            {
            var ex = new MethodNotImplementedException(method);
            log.Warn().Exception(ex).Message("Method {method} is not implemented", method).Write();
            return ex;
            }

        private void ReleaseManagedResources()
            {
            if (disposed) return;
            log?.Debug().Message("Releasing managed resources");
            SharedResources.ConnectionManager.GoOffline(clientId);
            SharedResources.ConnectionManager.UnregisterClient(clientId);
            }
        }
    }