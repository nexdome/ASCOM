// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.Utils.Logging.NLog;

namespace TA.NexDome.Specifications.Builders
    {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.DeviceInterface;
    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.SharedTypes;
    using TA.NexDome.Specifications.Contexts;
    using TA.NexDome.Specifications.Fakes;

    class DeviceControllerContextBuilder
        {
        public DeviceControllerContextBuilder()
            {
            channelFactory = new ChannelFactory();
            channelFactory.RegisterChannelType(
                p => p.StartsWith("Fake", StringComparison.InvariantCultureIgnoreCase),
                connection => new FakeEndpoint(),
                endpoint => new FakeCommunicationChannel(fakeResponseBuilder.ToString()));

            // channelFactory.RegisterChannelType(
            // SimulatorEndpoint.IsConnectionStringValid,
            // SimulatorEndpoint.FromConnectionString,
            // endpoint => new SimulatorCommunicationsChannel(endpoint as SimulatorEndpoint)
            // );
            }

        bool channelShouldBeOpen;

        readonly StringBuilder fakeResponseBuilder = new StringBuilder();

        readonly IClock timeSource = new FakeClock(DateTime.MinValue.ToUniversalTime());

        readonly ChannelFactory channelFactory;

        string connectionString = "Fake";

        readonly DeviceControllerOptions controllerOptions = new DeviceControllerOptions
                                                                 {
                                                                 MaximumFullRotationTime = TimeSpan.FromMinutes(1),
                                                                 MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
                                                                 ShutterTickTimeout = TimeSpan.FromSeconds(5),
                                                                 RotatorTickTimeout = TimeSpan.FromSeconds(5),
                                                                 HomeAzimuth = 10.0m
                                                                 };

        PropertyChangedEventHandler propertyChangedAction;

        List<Tuple<string, Action>> propertyChangeObservers = new List<Tuple<string, Action>>();

        SensorState initialShutterState;

        public DeviceControllerContext Build()
            {
            // Build the communications channel
            var channel = channelFactory.FromConnectionString(connectionString);
            if (channelShouldBeOpen)
                channel.Open();

            // Build the ControllerStatusFactory
            var statusFactory = new ControllerStatusFactory(timeSource);

            var fakeTransactionProcessor = new FakeTransactionProcessor(Enumerable.Empty<string>());
            var controllerActions = new RxControllerActions(channel, fakeTransactionProcessor);
            var controllerStateMachine = new ControllerStateMachine(controllerActions, controllerOptions, timeSource);
            controllerStateMachine.ShutterLimitSwitches = initialShutterState;
            var logger = new LoggingService();

            // Build the device controller
            var controller = new DeviceController(
                channel,
                statusFactory,
                controllerStateMachine,
                controllerOptions,
                fakeTransactionProcessor, logger);

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
        ///     Start with the state machine initialized and in the Ready state. Implies an open
        ///     channel.
        /// </summary>
        /// <param name="connectionString">
        ///     The connection string to use when creating and opening the channel.
        /// </param>
        public DeviceControllerContextBuilder WithStateMachineInitializedAndReady(string connectionString)
            {
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