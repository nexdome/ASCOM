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

    [Subject(typeof(ControllerStateMachine), "construction")]
    internal class when_the_state_machine_is_constructed : with_default_controller_state_machine
        {
        It should_start_in_the_uninitialized_state = () => Machine.CurrentState.Name.ShouldEqual(nameof(Uninitialized));
        }

    [Subject(typeof(ControllerStateMachine), "initialization")]
    internal class when_the_user_fails_to_initialize_the_state_machine : with_default_controller_state_machine
        {
        Because of = () => Exception = Catch.Exception(() => Machine.AzimuthEncoderTickReceived(0));
        It should_throw = () => Exception.ShouldBeOfExactType<InvalidOperationException>();
        }

    [Subject(typeof(ControllerStateMachine), "startup")]
    internal class when_the_state_machine_starts : with_default_controller_state_machine
        {
        Because of = () =>
            {
            Machine.Initialize(new RequestStatus(Machine));
            var factory = new ControllerStatusFactory(new SystemDateTimeUtcClock());
            var newStatus = factory.FromStatusPacket(Constants.StrSimulatedStatusResponse);
            Machine.HardwareStatusReceived(newStatus);
            };
        It should_request_the_hardware_status = () =>
            A.CallTo(() => FakeControllerActions.RequestHardwareStatus()).MustHaveHappened();
        It should_finish_in_the_ready_state = () => Machine.CurrentState.Name.ShouldEqual(nameof(Ready));
        }

    [Subject(typeof(ControllerStateMachine), "local operations")]
    internal class when_idle_and_an_azimuth_encoder_tick_is_received : with_controller_state_machine_in_ready_state
        {
        Because of = () => Machine.AzimuthEncoderTickReceived(100);
        It should_transition_to_rotating_state = () => Machine.CurrentState.Name.ShouldEqual(nameof(Rotating));
        It should_update_the_azimuth_property = () => Machine.AzimuthEncoderPosition.ShouldEqual(100);
        }

    [Subject(typeof(ControllerStateMachine), "local operations")]
    internal class
        when_idle_and_a_shutter_current_measurement_is_received : with_controller_state_machine_in_ready_state
        {
        Because of = () => Machine.ShutterMotorCurrentReceived(15);
        It should_transition_to_shutter_moving_state =
            () => Machine.CurrentState.Name.ShouldEqual(nameof(ShutterMoving));
        It should_update_the_shutter_current_property = () => Machine.ShutterMotorCurrent.ShouldEqual(15);
        It should_not_set_a_shutter_direction =
            () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        Behaves_like<ShutterMoving> the_shutter_is_moving = () => { };
        }

    [Subject(typeof(ControllerStateMachine), "inferred shutter position")]
    internal class when_the_shutter_moves_for_a_sufficiently_long_time : with_state_machine_that_infers_shutter_position
        {
        Because of = () =>
            {
            var minimumRequiredMoveTime = Machine.Options.MaximumShutterCloseTime.TotalSeconds / 2;
            Machine.Initialize(new Ready(Machine));
            var factory = new ControllerStatusFactory(Clock);
            var newStatus = factory.FromStatusPacket(Constants.StrSimulatedStatusResponse);
            Machine.ShutterDirectionReceived(ShutterDirection.Closing);
            Clock.AdvanceBy(TimeSpan.FromSeconds(minimumRequiredMoveTime));
            Machine.ShutterMotorCurrentReceived(Machine.Options.CurrentDrawDetectionThreshold);
            var indeterminateShutter =
                factory.FromStatusPacket(TestData.FromEmbeddedResource("StatusWithIndeterminateShutter.txt"));
            Machine.HardwareStatusReceived(indeterminateShutter);
            };
        It should_finish_with_shutter_closed = () => Machine.ShutterPosition.ShouldEqual(SensorState.Closed);
        }

    [Subject(typeof(ControllerStateMachine), "inferred shutter position")]
    internal class
        when_the_shutter_moves_for_a_sufficiently_long_time_but_current_draw_is_too_low :
            with_state_machine_that_infers_shutter_position
        {
        Because of = () =>
            {
            var minimumRequiredMoveTime = Machine.Options.MaximumShutterCloseTime.TotalSeconds / 2;
            Machine.Initialize(new Ready(Machine));
            var factory = new ControllerStatusFactory(Clock);
            var newStatus = factory.FromStatusPacket(Constants.StrSimulatedStatusResponse);
            Machine.ShutterDirectionReceived(ShutterDirection.Closing);
            Clock.AdvanceBy(TimeSpan.FromSeconds(minimumRequiredMoveTime));
            Machine.ShutterMotorCurrentReceived(Machine.Options.CurrentDrawDetectionThreshold - 1);
            var indeterminateShutter =
                factory.FromStatusPacket(TestData.FromEmbeddedResource("StatusWithIndeterminateShutter.txt"));
            Machine.HardwareStatusReceived(indeterminateShutter);
            };
        It should_finish_with_shutter_indeterminate =
            () => Machine.ShutterPosition.ShouldEqual(SensorState.Indeterminate);
        }

    [Subject(typeof(ControllerStateMachine), "inferred shutter position")]
    internal class when_the_shutter_move_is_insufficient : with_state_machine_that_infers_shutter_position
        {
        Because of = () =>
            {
            var minimumRequiredMoveTime = Machine.Options.MaximumShutterCloseTime.TotalSeconds / 2;
            Machine.Initialize(new Ready(Machine));
            var factory = new ControllerStatusFactory(Clock);
            var newStatus = factory.FromStatusPacket(Constants.StrSimulatedStatusResponse);
            Machine.ShutterDirectionReceived(ShutterDirection.Closing);
            Clock.AdvanceBy(TimeSpan.FromSeconds(minimumRequiredMoveTime) - TimeSpan.FromTicks(1));
            Machine.ShutterMotorCurrentReceived(Machine.Options.CurrentDrawDetectionThreshold);
            var indeterminateShutter =
                factory.FromStatusPacket(TestData.FromEmbeddedResource("StatusWithIndeterminateShutter.txt"));
            Machine.HardwareStatusReceived(indeterminateShutter);
            };
        It should_finish_with_shutter_indeterminate =
            () => Machine.ShutterPosition.ShouldEqual(SensorState.Indeterminate);
        }
    }