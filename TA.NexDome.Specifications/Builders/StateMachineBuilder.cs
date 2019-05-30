using FakeItEasy;
using System;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.Builders
    {
    internal class StateMachineBuilder
        {
        private IControllerActions actions = A.Fake<IControllerActions>();

        private DeviceControllerOptions deviceControllerOptions = new DeviceControllerOptions
            {
            KeepAliveTimerInterval = TimeSpan.FromMinutes(3),
            MaximumFullRotationTime = TimeSpan.FromMinutes(1),
            MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
            PerformShutterRecovery = false,
            IgnoreHardwareShutterSensor = false,
            CurrentDrawDetectionThreshold = 10
            };

        private bool initializeRotatorStateMachine = false;
        private bool initializeShuttterStateMachine = false;
        private bool rotatorIsRotating = false;
        private Type rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
        private Type shutterStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState);
        SensorState shutterSensorState = SensorState.Indeterminate;

        internal StateMachineContext Build()
            {
            var machine = new ControllerStateMachine(actions, deviceControllerOptions, new SystemDateTimeUtcClock());
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
            return this;
            }

        public StateMachineBuilder WithShutterFullyClosed()
            {
            shutterStartType = typeof(ClosedState);
            initializeShuttterStateMachine = true;
            shutterSensorState = SensorState.Open;
            return this;
            }
        }
    }