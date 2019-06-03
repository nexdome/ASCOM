// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ServerStatusDisplay.cs  Last modified: 2018-08-30@09:12 by Tim Long

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Forms;
using ASCOM.Controls;
using JetBrains.Annotations;
using TA.NexDome.Server.Properties;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Server
    {
    public partial class ServerStatusDisplay : Form
        {
        [NotNull] private readonly List<IDisposable> disposableSubscriptions = new List<IDisposable>();
        [NotNull] private List<ClickCommand> clickCommands = new List<ClickCommand>();
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
            clientStatusSubscription = clientStatusObservable
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(ObserveClientStatusChanged);
            ObserveClientStatusChanged(null); // This sets the initial UI state before any notifications arrive
            SetupCommand.AttachCommand(ExecuteSetupDialog, CanSetup);
            }

        private void ConfigureAnnunciators()
            {
            var annunciators = new List<Annunciator>
                {
                AzimuthMotorAnnunciator, ClockwiseAnnunciator, CounterClockwiseAnnunciator,
                ShutterMotorAnnunciator, ShutterOpeningAnnunciator, ShutterClosingAnnunciator,
                ShutterOpenAnnunciator, ShutterClosedAnnunciator, ShutterIndeterminateAnnunciator,
                UserPin1Annunciator, UserPin2Annunciator, UserPin3Annunciator, UserPin4Annunciator,
                AtHomeAnnunciator
                };
            annunciators.ForEach(p => p.Mute = false);
            annunciators.ForEach(p => p.Cadence = CadencePattern.SteadyOn);
            AzimuthMotorAnnunciator.Cadence = CadencePattern.BlinkAlarm;
            ShutterMotorAnnunciator.Cadence = CadencePattern.BlinkAlarm;
            ShutterOpenAnnunciator.Cadence = CadencePattern.Wink;
            AtHomeAnnunciator.Cadence = CadencePattern.Wink;
            annunciators.ForEach(p => p.Mute = true);
            }

        private void ObserveClientStatusChanged(EventPattern<EventArgs> eventPattern)
            {
            SetUiDeviceConnectedState();
            var clientStatus = SharedResources.ConnectionManager.Clients;
            registeredClientCount.Text = clientStatus.Count().ToString();
            var onlineClientCount = clientStatus.Count(p => p.Online);
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
                CloseButton.AttachCommand(ExecuteCloseShutter, CanMoveShutterOrDome)
                };
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
            var clientsOnline = SharedResources.ConnectionManager.OnlineClientCount;
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
                controller
                    .GetObservableValueFor(p => p.AzimuthMotorActive)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(motorActive => AzimuthMotorAnnunciator.Mute = !motorActive)
            );
            disposableSubscriptions.Add(
                controller
                    .GetObservableValueFor(p => p.AzimuthDirection)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetRotationDirection)
            );
            disposableSubscriptions.Add(
                controller
                    .GetObservableValueFor(p => p.AzimuthDegrees)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetAzimuthPosition)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterMotorActive)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(motorActive => ShutterMotorAnnunciator.Mute = !motorActive)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterMovementDirection)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterDirection)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterMotorCurrent)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterMotorCurrent)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.ShutterLimitSwitches)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetShutterPosition)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.AtHome)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(home => AtHomeAnnunciator.Mute = !home)
            );
            disposableSubscriptions.Add(
                controller.GetObservableValueFor(p => p.UserPins)
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(SetUserPins)
            );
            disposableSubscriptions.Add(
                controller.GetPropertyChangedEvents()
                    .ObserveOn(SynchronizationContext.Current)
                    .Subscribe(p => clickCommands.ForEach(q => q.CanExecuteChanged()))
            );
            }

        private void SetUserPins(Octet pinState)
            {
            UserPin1Annunciator.Mute = !pinState[0];
            UserPin2Annunciator.Mute = !pinState[1];
            UserPin3Annunciator.Mute = !pinState[2];
            UserPin4Annunciator.Mute = !pinState[3];
            }

        private void SetShutterPosition(SensorState position)
            {
            var controller = SharedResources.ConnectionManager.MaybeControllerInstance.Single();
            var shutterMoving = controller?.ShutterMotorActive ?? false;
            ShutterClosedAnnunciator.Mute = position != SensorState.Closed || shutterMoving;
            ShutterIndeterminateAnnunciator.Mute = position != SensorState.Indeterminate || shutterMoving;
            ShutterOpenAnnunciator.Mute = position != SensorState.Open || shutterMoving;
            }

        private void SetAzimuthPosition(float position)
            {
            var format = AzimuthPositionAnnunciator.Tag.ToString();
            var formattedPosition = string.Format(format, (int) position);
            AzimuthPositionAnnunciator.Text = formattedPosition;
            }

        private void SetShutterMotorCurrent(int current)
            {
            var format = ShutterCurrentAnnunciator.Tag.ToString();
            var formattedValue = string.Format(format, current);
            ShutterCurrentAnnunciator.Text = formattedValue;
            // Auto-scale the progress bar
            ShutterCurrentBar.Maximum = Math.Max(ShutterCurrentBar.Maximum, current);
            ShutterCurrentBar.Value = current;
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
            SharedResources.DoSetupDialog(default(Guid));
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
        }
    }