using System;
using System.Collections.Generic;
using System.Linq;
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

    internal StateMachineContext Build()
        {
        var machine = new ControllerStateMachine(actions, deviceControllerOptions, new SystemDateTimeUtcClock());
        var context = new StateMachineContext
            {
            Actions = actions,
            Machine = machine
            };
        return context;
        }
    }
}
