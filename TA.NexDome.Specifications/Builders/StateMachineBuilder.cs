// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;
using TA.Utils.Logging.NLog;

namespace TA.NexDome.Specifications.Builders
    {
    using System;

    using FakeItEasy;

    using JetBrains.Annotations;

    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.DeviceInterface.StateMachine.Rotator;
    using TA.NexDome.DeviceInterface.StateMachine.Shutter;
    using TA.NexDome.Specifications.Contexts;

    using RequestStatusState = TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState;

    class StateMachineBuilder
        {
        const int shutterLimitOfTravel = 500;

        readonly IControllerActions actions = A.Fake<IControllerActions>();

        readonly DeviceControllerOptions deviceControllerOptions = new DeviceControllerOptions
                                                                       {
                                                                       MaximumFullRotationTime =
                                                                           TimeSpan.FromMinutes(1),
                                                                       MaximumShutterCloseTime =
                                                                           TimeSpan.FromMinutes(1),
                                                                       ShutterTickTimeout = TimeSpan.FromSeconds(5),
                                                                       RotatorTickTimeout = TimeSpan.FromSeconds(5),
                                                                       HomeAzimuth = 10.0m
                                                                       };

        bool initializeRotatorStateMachine;

        bool initializeShuttterStateMachine;

        bool rotatorIsRotating;

        Type rotatorStartType = typeof(ReadyState);

        Type shutterStartType = typeof(OfflineState);

        [UsedImplicitly]
        SensorState shutterSensorState = SensorState.Indeterminate;

        int shutterStepPosition;
        LoggingService logger = new LoggingService();

        internal StateMachineContext Build()
            {
            var machine = new ControllerStateMachine(actions, deviceControllerOptions, new SystemDateTimeUtcClock(), logger);
            machine.ShutterStepPosition = shutterStepPosition;
            machine.ShutterLimitSwitches = shutterSensorState;
            var context = new StateMachineContext { Actions = actions, Machine = machine };
            if (initializeRotatorStateMachine)
                {
                var rotatorState = Activator.CreateInstance(rotatorStartType, machine) as IRotatorState;
                machine.Initialize(rotatorState);
                }

            if (initializeShuttterStateMachine)
                {
                var shutterState = Activator.CreateInstance(shutterStartType, machine) as IShutterState;
                machine.Initialize(shutterState);
                }

            if (rotatorIsRotating)
                {
                machine.AzimuthMotorActive = true;
                machine.AtHome = false;
                machine.AzimuthDirection = RotationDirection.Clockwise; // Arbitrary choice
                machine.AzimuthEncoderPosition = 100; // Arbitrary choice
                }

            return context;
            }

        internal StateMachineBuilder WithReadyRotator()
            {
            rotatorStartType = typeof(ReadyState);
            initializeRotatorStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithReadyRotatorAndOfflineShutter()
            {
            rotatorStartType = typeof(ReadyState);
            shutterStartType = typeof(OfflineState);
            initializeRotatorStateMachine = true;
            initializeShuttterStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithRotatingRotator()
            {
            rotatorStartType = typeof(RotatingState);
            initializeRotatorStateMachine = true;
            rotatorIsRotating = true;
            return this;
            }

        internal StateMachineBuilder WithTimedOutRotator()
            {
            rotatorStartType = typeof(RequestStatusState);
            initializeRotatorStateMachine = true;
            rotatorIsRotating = true;
            return this;
            }

        internal StateMachineBuilder WithOfflineShutter()
            {
            shutterStartType = typeof(OfflineState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Indeterminate;
            return this;
            }

        public StateMachineBuilder WithShutterInRequestStatusState()
            {
            shutterStartType = typeof(NexDome.DeviceInterface.StateMachine.Shutter.RequestStatusState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Indeterminate;
            return this;
            }

        public StateMachineBuilder WithShutterFullyOpen()
            {
            shutterStartType = typeof(OpenState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Open;
            shutterStepPosition = shutterLimitOfTravel;
            return this;
            }

        public StateMachineBuilder WithShutterPartiallyOpen()
            {
            shutterStartType = typeof(OpenState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Indeterminate;
            shutterStepPosition = shutterLimitOfTravel / 2;
            return this;
            }

        public StateMachineBuilder WithShutterFullyClosed()
            {
            shutterStartType = typeof(ClosedState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Open;
            shutterStepPosition = 0;
            return this;
            }

        public StateMachineBuilder WithOpeningShutter()
            {
            shutterStartType = typeof(OpeningState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Indeterminate;
            shutterStepPosition = 50;
            return this;
            }

        public StateMachineBuilder WithClosingShutter()
            {
            shutterStartType = typeof(ClosingState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Indeterminate;
            shutterStepPosition = shutterLimitOfTravel / 2;
            return this;
            }
        }
    }