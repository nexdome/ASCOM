using ASCOM;
using ASCOM.DeviceInterface;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using TA.NexDome.DeviceInterface;
using TA.NexDome.Server;
using TA.NexDome.SharedTypes;
using NotImplementedException = ASCOM.NotImplementedException;

namespace TA.NexDome.AscomDome
    {
    [ProgId(SharedResources.DomeDriverId)]
    [Guid("32f049d8-fac4-45df-84af-077f01d0d4e1")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    [DeviceId(SharedResources.DomeDriverId, DeviceName = SharedResources.DomeDriverName)]
    [ServedClassName(SharedResources.DomeDriverName)]
    public class Dome : ReferenceCountedObject, IDomeV2, IDisposable
        {
        private bool disposed = false;
        private readonly Guid clientId;
        private DeviceController controller;

        public Dome()
            {
            clientId = SharedResources.ConnectionManager.RegisterClient(SharedResources.DomeDriverId);
            }

        private void ReleaseUnmanagedResources()
            {
            if (disposed) return;
            SharedResources.ConnectionManager.GoOffline(clientId);
            SharedResources.ConnectionManager.UnregisterClient(clientId);
            }

        /// <inheritdoc />
        ~Dome() => ReleaseUnmanagedResources();

        /// <inheritdoc />
        public void SetupDialog() => SharedResources.DoSetupDialog(clientId);

        /// <inheritdoc />
        public string Action(string ActionName, string ActionParameters) => null;

        /// <inheritdoc />
        public void CommandBlind(string Command, bool Raw = false) { }

        /// <inheritdoc />
        public bool CommandBool(string Command, bool Raw = false) => false;

        /// <inheritdoc />
        public string CommandString(string Command, bool Raw = false) => null;

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
        public void CloseShutter() => controller.CloseShutter();

        /// <inheritdoc />
        public void FindHome() => controller.RotateToHomePosition();

        /// <inheritdoc />
        public void OpenShutter() => controller.OpenShutter();

        /// <inheritdoc />
        public async void Park() => await controller.Park();

        /// <inheritdoc />
        public void SetPark() => SharedResources.SetParkPosition((decimal) controller.AzimuthDegrees);

        /// <inheritdoc />
        public void SlewToAltitude(double altitude) => throw new NotImplementedException();

        /// <inheritdoc />
        public void SlewToAzimuth(double Azimuth) => controller.SlewToAzimuth(Azimuth);

        /// <inheritdoc />
        public void SyncToAzimuth(double Azimuth) { }

        /// <inheritdoc />
        public bool Connected
            {
            get => controller?.IsConnected ?? false;
            set
                {
                if (value)
                    {
                    controller = SharedResources.ConnectionManager.GoOnline(clientId);
                    }
                else
                    {
                    SharedResources.ConnectionManager.GoOffline(clientId);
                    controller = null; //[Sentinel]
                    }
                }
            }

        /// <inheritdoc />
        public string Description { get; }

        /// <inheritdoc />
        public string DriverInfo { get; }

        /// <inheritdoc />
        public string DriverVersion { get; }

        /// <inheritdoc />
        public short InterfaceVersion { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public ArrayList SupportedActions { get; }

        /// <inheritdoc />
        public double Altitude { get; }

        /// <inheritdoc />
        public bool AtHome => controller.AtHome;

        /// <inheritdoc />
        public bool AtPark => controller.AtPark;

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
        public bool CanSetShutter => true;

        /// <inheritdoc />
        public bool CanSlave => false;

        /// <inheritdoc />
        public bool CanSyncAzimuth => false;

        /// <inheritdoc />
        public ShutterState ShutterStatus
            {
            get
                {
                switch (controller.ShutterDisposition)
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
        public bool Slaved { get; set; }

        /// <inheritdoc />
        public bool Slewing => controller.IsMoving;
        }
    }
