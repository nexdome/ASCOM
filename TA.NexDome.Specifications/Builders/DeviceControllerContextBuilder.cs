// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: DeviceControllerContextBuilder.cs  Last modified: 2018-09-16@14:01 by Tim Long

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;
using TA.NexDome.Specifications.Fakes;

namespace TA.NexDome.Specifications.Builders
    {
    internal class DeviceControllerContextBuilder
        {
        public DeviceControllerContextBuilder()
            {
            channelFactory = new ChannelFactory();
            channelFactory.RegisterChannelType(
                p => p.StartsWith("Fake", StringComparison.InvariantCultureIgnoreCase),
                connection => new FakeEndpoint(),
                endpoint => new FakeCommunicationChannel(fakeResponseBuilder.ToString())
            );
            //channelFactory.RegisterChannelType(
            //    SimulatorEndpoint.IsConnectionStringValid,
            //    SimulatorEndpoint.FromConnectionString,
            //    endpoint => new SimulatorCommunicationsChannel(endpoint as SimulatorEndpoint)
            //);
            }

        bool channelShouldBeOpen;
        readonly StringBuilder fakeResponseBuilder = new StringBuilder();
        readonly IClock timeSource = new FakeClock(DateTime.MinValue.ToUniversalTime());
        readonly ChannelFactory channelFactory;
        string connectionString = "Fake";
        readonly DeviceControllerOptions controllerOptions = new DeviceControllerOptions
            {
            KeepAliveTimerInterval = TimeSpan.FromMinutes(3),
            MaximumFullRotationTime = TimeSpan.FromMinutes(1),
            MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
            PerformShutterRecovery = true,
            CurrentDrawDetectionThreshold = 10,
            IgnoreHardwareShutterSensor = false,
            ShutterTickTimeout = TimeSpan.FromSeconds(5)
            };
        PropertyChangedEventHandler propertyChangedAction;
        List<Tuple<string, Action>> propertyChangeObservers = new List<Tuple<string, Action>>();
        SensorState initialShutterState;
        bool startInReadyState;

        public DeviceControllerContext Build()
            {
            // Build the communications channel
            var channel = channelFactory.FromConnectionString(connectionString);
            if (channelShouldBeOpen)
                channel.Open();

            // Build the ControllerStatusFactory
            var statusFactory = new ControllerStatusFactory(timeSource);

            var controllerActions = new RxControllerActions(channel);
            var controllerStateMachine = new ControllerStateMachine(controllerActions, controllerOptions, timeSource);
            controllerStateMachine.ShutterPosition = initialShutterState;
            if (startInReadyState)
                controllerStateMachine.Initialize(new Ready(controllerStateMachine));

            // Build the device controller
            var controller = new DeviceController(channel, statusFactory, controllerStateMachine, controllerOptions);

            // Assemble the device controller test context
            var context = new DeviceControllerContext
                {
                Channel = channel,
                Controller = controller,
                StateMachine = controllerStateMachine,
                Actions = controllerActions
                };

            // Wire up any Property Changed notifications
            if (propertyChangedAction != null) controller.PropertyChanged += propertyChangedAction;

            return context;
            }

        public DeviceControllerContextBuilder WithOpenConnection(string connection)
            {
            connectionString = connection;
            channelShouldBeOpen = true;
            return this;
            }

        /// <summary>
        ///     Start with the state machine initialized and in the Ready state. Implies an open channel.
        /// </summary>
        /// <param name="connectionString">The connection string to use when creating and opening the channel.</param>
        public DeviceControllerContextBuilder WithStateMachineInitializedAndReady(string connectionString)
            {
            startInReadyState = true;
            return WithOpenConnection(connectionString);
            }

        public DeviceControllerContextBuilder WithFakeResponse(string fakeResponse)
            {
            fakeResponseBuilder.Append(fakeResponse);
            return this;
            }

        public DeviceControllerContextBuilder WithClosedConnection(string connection)
            {
            connectionString = connection;
            channelShouldBeOpen = false;
            return this;
            }

        public DeviceControllerContextBuilder WithClosedShutter()
            {
            initialShutterState = SensorState.Closed;
            return this;
            }

        public DeviceControllerContextBuilder WithOpenShutter()
            {
            initialShutterState = SensorState.Open;
            return this;
            }

        public DeviceControllerContextBuilder WithIndeterminateShutter()
            {
            initialShutterState = SensorState.Indeterminate;
            return this;
            }

        public DeviceControllerContextBuilder OnPropertyChanged(PropertyChangedEventHandler action)
            {
            propertyChangedAction = action;
            return this;
            }
        }
    }