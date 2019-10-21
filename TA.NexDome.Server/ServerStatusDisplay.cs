// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.WinFormsControls;

namespace TA.NexDome.Server
    {
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading;
    using System.Windows.Forms;
    using JetBrains.Annotations;
    using TA.NexDome.Server.Properties;
    using TA.NexDome.SharedTypes;

    public partial class ServerStatusDisplay : Form
        {
        [NotNull]
        private readonly List<IDisposable> disposableSubscriptions = new List<IDisposable>();

        [NotNull]
        private List<ClickCommand> clickCommands = new List<ClickCommand>();

        private IDisposable clientStatusSubscription;

        public ServerStatusDisplay()
            {
            InitializeComponent();
            }

        private void frmMain_Load(object sender, EventArgs e)
            {
            ConfigureAnnunciators();
            var clientStatusObservable = Observable.FromEventPattern<EventHandler<EventArgs>, EventArgs>(
                handler => SharedResources.ConnectionManager.ClientStatusChanged += handler,
                handler => SharedResources.ConnectionManager.ClientStatusChanged -= handler);
            clientStatusSubscription = clientStatusObservable.ObserveOn(SynchronizationContext.Current)
                .Subscribe(ObserveClientStatusChanged);
            ObserveClientStatusChanged(null); // This sets the initial UI state before any notifications arrive
            SetupCommand.AttachCommand(ExecuteSetupDialog, CanSetup);
            this.EnsureVisible();
            }

        private void ConfigureAnnunciators()
            {
            var annunciators = new List<Annunciator>
                                   {
                                   AzimuthMotorAnnunciator,
                                   ClockwiseAnnunciator,
                                   CounterClockwiseAnnunciator,
                                   ShutterMotorAnnunciator,
                                   ShutterOpeningAnnunciator,
                                   ShutterClosingAnnunciator,
                                   ShutterDispositionAnnunciator,
                                   ShutterLinkStateAnnunciator,
                                   RainAnnunciator,
                                   AtHomeAnnunciator,
                                   batteryVoltsAnnunciator
                                   };
            annunciators.ForEach(p => p.Mute = false);
            annunciators.ForEach(p => p.Cadence = CadencePattern.SteadyOn);
            AzimuthMotorAnnunciator.Cadence = CadencePattern.BlinkAlarm;
            ShutterMotorAnnunciator.Cadence = CadencePattern.BlinkAlarm;
            ShutterDispositionAnnunciator.Cadence = CadencePattern.Wink;
            AtHomeAnnunciator.Cadence = CadencePattern.Wink;
            RainAnnunciator.Cadence = CadencePattern.BlinkFast;
            annunciators.ForEach(p => p.Mute = true);
            }

        private void ObserveClientStatusChanged(EventPattern<EventArgs> eventPattern)
            {
            SetUiDeviceConnectedState();
            var clientStatus = SharedResources.ConnectionManager.Clients;
            registeredClientCount.Text = clientStatus.Count().ToString();
            int onlineClientCount = clientStatus.Count(p => p.Online);
            OnlineClients.Text = onlineClientCount.ToString();
            ConfigureUiPropertyNotifications();
            if (onlineClientCount == 1)
                AttachCommands();
            if (onlineClientCount == 0)
                DetachCommands();
            }

        private bool CanSetup() =>
            CanMoveShutterOrDome() || !SharedResources.ConnectionManager.MaybeControllerInstance.Any();

        private bool CanMoveShutterOrDome()
            {
            var maybeController = SharedResources.ConnectionManager.MaybeControllerInstance;
            if (!maybeController.Any()) return false;
            var controller = maybeController.Single();
            return !controller.IsMoving;
            }

        /*
         * Experimental code!
         * Exploring the use of the Command pattern to wire up controls to
         * OnClick events, and have the controls enable/disable themselves using the
         * command's CanExecuteChanged() method.
         */
        private void AttachCommands()
            {
            var maybeController = SharedResources.ConnectionManager.MaybeControllerInstance;
            if (!maybeController.Any()) return;
            var controller = maybeController.Single();
            clickCommands = new List<ClickCommand>
                                {
                                OpenButton.AttachCommand(ExecuteOpenShutter, CanMoveShutterOrDome),
                                CloseButton.AttachCommand(ExecuteCloseShutter, CanMoveShutterOrDome),
                                StopCommand.AttachCommand(ExecuteStop, CanStop)
                                };
            }

        private bool CanStop()
            {
            var maybeController = SharedResources.ConnectionManager.MaybeControllerInstance;
            if (!maybeController.Any()) return false;
            var controller = maybeController.Single();
            return controller.IsMoving;
            }

        private void ExecuteStop()
            {
            if (SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                SharedResources.ConnectionManager.MaybeControllerInstance.Single().RequestEmergencyStop();
            }

        private void DetachCommands()
            {
            clickCommands.ForEach(p => p.Dispose());
            clickCommands.Clear();
            }

        /// <summary>
        ///     Enables each device activity annunciator if there are any clients connected to that
        ///     device.
        /// </summary>
        private void SetUiDeviceConnectedState()
            {
            var clients = SharedResources.ConnectionManager.Clients;
            if (clients.Count == 1)
                ConfigureAnnunciators();
            }

        /// <summary>
        ///     Begins or terminates UI updates depending on the number of online clients.
        /// </summary>
        private void ConfigureUiPropertyNotifications()
            {
            int clientsOnline = SharedResources.ConnectionManager.OnlineClientCount;
            if (clientsOnline > 0)
                SubscribePropertyChangeNotifications();
            else
                UnsubscribePropertyChangeNotifications();
            }

        /// <summary>
        ///     Stops observing the controller property change notifications.
        /// </summary>
        private void UnsubscribePropertyChangeNotifications()
            {
            disposableSubscriptions.ForEach(p => p.Dispose());
            disposableSubscriptions.Clear();
            }

        /// <summary>
        ///     Creates subscriptions that observe property change notifications and update the UI as changes occur.
        /// </summary>
        private void SubscribePropertyChangeNotifications()
            {
            if (!SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                return;
            var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();

            /* ToDo:
                         * Add subscriptions to PropertyChanged notifications using this pattern:
                         *  movingSubscription = controller
                         *      .GetObservableValueFor(m => m.IsMoving)
                         *      .ObserveOn(SynchronizationContext.Current)
                         *      .Subscribe(SetMotorMovingState);
                         */
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.AzimuthMotorActive).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(motorActive => AzimuthMotorAnnunciator.Mute = !motorActive));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.AzimuthDirection).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetRotationDirection));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.AzimuthDegrees).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetAzimuthPosition));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterMotorActive).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(motorActive => ShutterMotorAnnunciator.Mute = !motorActive));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterMovementDirection)
                    .ObserveOn(SynchronizationContext.Current).Subscribe(SetShutterDirection));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterDisposition).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterDisposition));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterPercentOpen).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterPercentOpen));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterLimitSwitches).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterLimitSwitches));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterLinkState).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterLinkState));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.AtHome).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(home => AtHomeAnnunciator.Mute = !home));
            disposableSubscriptions.Add(
                controller.GetPropertyChangedEvents().ObserveOn(SynchronizationContext.Current)
                    .Subscribe(p => clickCommands.ForEach(q => q.CanExecuteChanged())));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterBatteryVolts).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetBatteryVolts));
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.IsRaining).ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetRainAlarm));
            }

        private void SetRainAlarm(bool isRaining)
            {
            RainAnnunciator.Mute = !isRaining;
            }

        private void SetShutterLimitSwitches(SensorState position)
            {
            // var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();
            // var shutterMoving = controller?.ShutterMotorActive ?? false;
            // ShutterClosedAnnunciator.Mute = position != SensorState.Closed || shutterMoving;
            // ShutterIndeterminateAnnunciator.Mute = position != SensorState.Indeterminate || shutterMoving;
            // ShutterOpenAnnunciator.Mute = position != SensorState.Open || shutterMoving;
            }

        private void SetAzimuthPosition(float position)
            {
            string format = AzimuthPositionAnnunciator.Tag.ToString();
            string formattedPosition = string.Format(format, (int)position);
            AzimuthPositionAnnunciator.Text = formattedPosition;
            }

        private void SetShutterPercentOpen(int percent)
            {
            int safeValue = percent.Clip(0, 100);
            string format = ShutterPercentOpenAnnunciator.Tag.ToString();
            string formattedValue = string.Format(format, percent);
            ShutterPercentOpenAnnunciator.Text = formattedValue;
            ShutterPositionBar.Value = percent;
            var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();
            bool moving = controller?.ShutterMotorActive ?? false;
            ShutterPositionBar.MarqueeAnimationSpeed = moving ? 2000 : 0;
            }

        private void SetRotationDirection(RotationDirection direction)
            {
            CounterClockwiseAnnunciator.Mute = direction != RotationDirection.CounterClockwise;
            ClockwiseAnnunciator.Mute = direction != RotationDirection.Clockwise;
            }

        private void SetShutterDirection(ShutterDirection direction)
            {
            ShutterOpeningAnnunciator.Mute = direction != ShutterDirection.Opening;
            ShutterClosingAnnunciator.Mute = direction != ShutterDirection.Closing;
            }

        private void SetBatteryVolts(float volts)
            {
            /*
             * Annunciator:
             *      Steady/Green above 50% charge
             *      SlowFlash/Yellow under 50% charge but above flat
             *      Alarm/Red below flat voltage.
             * Progress bar:
             *      Clip to minimum 10V and maximum 15V
             *      Colours as for annunciator.
             */
            float annunciatorValue = volts.Clip(0.0f, 15.0f);
            string formatString = batteryVoltsAnnunciator.Tag.ToString();
            batteryVoltsAnnunciator.Text = string.Format(formatString, annunciatorValue);
            batteryVoltsAnnunciator.Mute = false;
            int barValue = (int)(volts * 10f);
            batteryVoltsBar.Value = barValue.Clip(100, 150);
            if (volts >= Constants.BatteryHalfChargedVolts)
                {
                batteryVoltsAnnunciator.ActiveColor = Color.DarkSeaGreen;
                batteryVoltsAnnunciator.Cadence = CadencePattern.SteadyOn;
                batteryVoltsBar.ForeColor = Color.DarkSeaGreen;
                return;
                }

            if (volts >= Constants.BatteryFullyDischargedVolts)
                {
                batteryVoltsAnnunciator.ActiveColor = Color.PaleGoldenrod;
                batteryVoltsAnnunciator.Cadence = CadencePattern.BlinkFast;
                batteryVoltsBar.ForeColor = Color.Orange;
                return;
                }

            batteryVoltsAnnunciator.ActiveColor = Color.Crimson;
            batteryVoltsAnnunciator.Cadence = CadencePattern.BlinkAlarm;
            batteryVoltsBar.ForeColor = Color.Crimson;
            if (volts <= Constants.BatteryFullyDischargedVolts) batteryVoltsBar.Value = batteryVoltsBar.Maximum;
            }

        private void SetShutterDisposition(ShutterDisposition disposition)
            {
            ShutterDispositionAnnunciator.Text = disposition.DisplayEquivalent();
            switch (disposition)
                {
                    case ShutterDisposition.Offline:
                        ShutterDispositionAnnunciator.ForeColor = Color.FromArgb(200, 4, 4);
                        ShutterDispositionAnnunciator.Cadence = CadencePattern.BlinkAlarm;
                        break;
                    case ShutterDisposition.Opening:
                    case ShutterDisposition.Closing:
                        ShutterDispositionAnnunciator.ForeColor = Color.LightGoldenrodYellow;
                        ShutterDispositionAnnunciator.Cadence = CadencePattern.BlinkFast;
                        break;
                    case ShutterDisposition.Open:
                        ShutterDispositionAnnunciator.ForeColor = Color.DarkSeaGreen;
                        ShutterDispositionAnnunciator.Cadence = CadencePattern.Wink;
                        break;
                    case ShutterDisposition.Closed:
                        ShutterDispositionAnnunciator.ForeColor = Color.DarkSeaGreen;
                        ShutterDispositionAnnunciator.Cadence = CadencePattern.SteadyOn;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(disposition), disposition, null);
                }
            ShutterDispositionAnnunciator.Mute = false;
            ShutterDispositionAnnunciator.Enabled = true;
            }

        private void SetShutterLinkState(ShutterLinkState disposition)
            {
            ShutterLinkStateAnnunciator.Text = disposition.DisplayEquivalent();
            switch (disposition)
                {
                    case ShutterLinkState.Start:
                        ShutterLinkStateAnnunciator.ForeColor = Color.FromArgb(200, 4, 4);
                        ShutterLinkStateAnnunciator.Cadence = CadencePattern.SteadyOn;
                        break;
                    case ShutterLinkState.WaitAT:
                    case ShutterLinkState.Config:
                        ShutterLinkStateAnnunciator.ForeColor = Color.FromArgb(200, 4, 4);
                        ShutterLinkStateAnnunciator.Cadence = CadencePattern.BlinkAlarm;
                        break;
                    case ShutterLinkState.Detect:
                        ShutterLinkStateAnnunciator.ForeColor = Color.PaleGoldenrod;
                        ShutterLinkStateAnnunciator.Cadence = CadencePattern.BlinkFast;
                        break;
                    case ShutterLinkState.Online:
                        ShutterLinkStateAnnunciator.ForeColor = Color.DarkSeaGreen;
                        ShutterLinkStateAnnunciator.Cadence = CadencePattern.SteadyOn;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(disposition), disposition, null);
                }
            ShutterLinkStateAnnunciator.Mute = false;
            ShutterLinkStateAnnunciator.Enabled = true;
            }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
            {
            clientStatusSubscription?.Dispose();
            UnsubscribePropertyChangeNotifications();
            var clients = SharedResources.ConnectionManager.Clients;
            foreach (var client in clients) SharedResources.ConnectionManager.GoOffline(client.ClientId);
            Application.Exit();
            }

        private void ExecuteSetupDialog()
            {
            SharedResources.DoSetupDialog(default);
            }

        private void frmMain_LocationChanged(object sender, EventArgs e)
            {
            Settings.Default.Save();
            }

        private void ServerStatusDisplay_VisibleChanged(object sender, EventArgs e)
            {
            BringToFront();
            }

        private void ExecuteOpenShutter()
            {
            if (SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                SharedResources.ConnectionManager.MaybeControllerInstance.Single().OpenShutter();
            }

        private void ExecuteCloseShutter()
            {
            if (SharedResources.ConnectionManager.MaybeControllerInstance.Any())
                SharedResources.ConnectionManager.MaybeControllerInstance.Single().CloseShutter();
            }

        private void ServerStatusDisplay_EnsureVisible(object sender, EventArgs e)
            {
            this.EnsureVisible();
            }
        }
    }