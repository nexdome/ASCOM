using FakeItEasy;
using System;
using JetBrains.Annotations;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.Builders
    {
    internal class StateMachineBuilder
        {
        const int shutterLimitOfTravel = 500;
        private IControllerActions actions = A.Fake<IControllerActions>();

        private DeviceControllerOptions deviceControllerOptions = new DeviceControllerOptions
            {
            MaximumFullRotationTime = TimeSpan.FromMinutes(1),
            MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
            ShutterTickTimeout = TimeSpan.FromSeconds(5),
            RotatorTickTimeout = TimeSpan.FromSeconds(5),
            HomeSensorAzimuth = 10.0m
            };

        private bool initializeRotatorStateMachine = false;
        private bool initializeShuttterStateMachine = false;
        private bool rotatorIsRotating = false;
        private Type rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
        private Type shutterStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState);
        [UsedImplicitly] SensorState shutterSensorState = SensorState.Indeterminate;
        int shutterStepPosition = 0;

        internal StateMachineContext Build()
            {
            var machine = new ControllerStateMachine(actions, deviceControllerOptions, new SystemDateTimeUtcClock());
            machine.ShutterStepPosition = shutterStepPosition;
            machine.ShutterLimitSwitches = shutterSensorState;
            var context = new StateMachineContext
                {
                Actions = actions,
                Machine = machine
                };
            if (initializeRotatorStateMachine)
                {
                IRotatorState rotatorState = Activator.CreateInstance(rotatorStartType, machine) as IRotatorState;
                machine.Initialize(rotatorState);
                }
            if (initializeShuttterStateMachine)
                {
                IShutterState shutterState = Activator.CreateInstance(shutterStartType, machine) as IShutterState;
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
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
            initializeRotatorStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithReadyRotatorAndOfflineShutter()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
            shutterStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState);
            initializeRotatorStateMachine = true;
            initializeShuttterStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithRotatingRotator()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.RotatingState);
            initializeRotatorStateMachine = true;
            rotatorIsRotating = true;
            return this;
            }

        internal StateMachineBuilder WithTimedOutRotator()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState);
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
            shutterStartType = typeof(RequestStatusState);
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
            shutterStepPosition = shutterLimitOfTravel/2;
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
            shutterStepPosition = shutterLimitOfTravel/2;
            return this;
            }
        }
    }