using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FakeItEasy;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.Builders
{
    class StateMachineBuilder
        {
    IControllerActions actions = A.Fake<IControllerActions>();
    DeviceControllerOptions deviceControllerOptions = new DeviceControllerOptions
        {
        KeepAliveTimerInterval = TimeSpan.FromMinutes(3),
        MaximumFullRotationTime = TimeSpan.FromMinutes(1),
        MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
        PerformShutterRecovery = false,
        IgnoreHardwareShutterSensor = false,
        CurrentDrawDetectionThreshold = 10
        };
        Type rotatorStartType= typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
        Type shutterStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState);
        bool initializeRotatorStateMachine = false;
        bool initializeShuttterStateMachine = false;
        bool rotatorIsRotating = false;

        internal StateMachineBuilder WithReadyRotatorAndOfflineShutter()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
            shutterStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Shutter.OfflineState);
            initializeRotatorStateMachine = true;
            initializeShuttterStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithReadyRotator()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.ReadyState);
            initializeRotatorStateMachine = true;
            return this;
            }

        internal StateMachineBuilder WithRotatingRotator()
            {
            rotatorStartType = typeof(TA.NexDome.DeviceInterface.StateMachine.Rotator.RotatingState);
            initializeRotatorStateMachine = true;
            rotatorIsRotating = true;
            return this;
            }

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
    }
}
