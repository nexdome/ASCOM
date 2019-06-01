// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ControllerStateMachineSpecs.cs  Last modified: 2018-09-14@18:14 by Tim Long

using System;
using FakeItEasy;
using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Fakes;
using TA.NexDome.Specifications.Helpers;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    #region  Context base classes
    internal class with_default_controller_state_machine
        {
        Establish context = () =>
            {
            FakeControllerActions = A.Fake<IControllerActions>();
            var options = new DeviceControllerOptions
                {
                KeepAliveTimerInterval = TimeSpan.FromMinutes(3),
                MaximumFullRotationTime = TimeSpan.FromMinutes(1),
                MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
                PerformShutterRecovery = false,
                IgnoreHardwareShutterSensor = false,
                CurrentDrawDetectionThreshold = 10
                };
            Machine = new ControllerStateMachine(FakeControllerActions, options, new SystemDateTimeUtcClock());
            };
        Cleanup after = () =>
            {
            StatusRequested = false;
            Machine = null;
            };
        protected static Exception Exception;
        protected static IControllerActions FakeControllerActions;
        protected static ControllerStateMachine Machine;
        protected static bool StatusRequested;

        static void SimulateRequestStatus()
            {
            StatusRequested = true;
            }
        }

    internal class with_state_machine_that_infers_shutter_position
        {
        Establish context = () =>
            {
            FakeControllerActions = A.Fake<IControllerActions>();
            Clock = new FakeClock(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var options = new DeviceControllerOptions
                {
                KeepAliveTimerInterval = TimeSpan.FromMinutes(3),
                MaximumFullRotationTime = TimeSpan.FromMinutes(1),
                MaximumShutterCloseTime = TimeSpan.FromMinutes(1),
                PerformShutterRecovery = false,
                IgnoreHardwareShutterSensor = true,
                CurrentDrawDetectionThreshold = 10,
                ShutterTickTimeout = TimeSpan.MaxValue
                };
            Machine = new ControllerStateMachine(FakeControllerActions, options, Clock);
            };
        Cleanup after = () =>
            {
            StatusRequested = false;
            Machine = null;
            };
        protected static FakeClock Clock;
        protected static IControllerActions FakeControllerActions;
        protected static ControllerStateMachine Machine;
        protected static bool StatusRequested;

        static void SimulateRequestStatus()
            {
            StatusRequested = true;
            }
        }

    internal class with_controller_state_machine_in_ready_state : with_default_controller_state_machine
        {
        Establish context = () => Machine.Initialize(new Ready(Machine));
        }

    internal class with_controller_state_machine_in_rotating_state : with_default_controller_state_machine
        {
        Establish context = () => Machine.Initialize(new Rotating(Machine));

        static void SimulateRequestStatus() { }
        }
    #endregion
    }