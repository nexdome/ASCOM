// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    using FakeItEasy;

    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.DeviceInterface.StateMachine.Rotator;
    using TA.NexDome.DeviceInterface.StateMachine.Shutter;
    using TA.NexDome.Specifications.Contexts;
    using TA.NexDome.Specifications.DeviceInterface.Behaviours;

    using RequestStatusState = TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState;

    [Subject(typeof(ControllerStateMachine), "startup")]
    class when_creating_the_controller_state_machine : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithReadyRotatorAndOfflineShutter().Build();

        It should_have_rotator_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();

        It should_have_shutter_in_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();

        It should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();

        It should_be_at_azimuth_zero = () => Machine.AzimuthEncoderPosition.ShouldEqual(0);

        It should_signal_rotator_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeTrue();

        It should_not_signal_shutter_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();

        It should_have_stopped_rotator_motor = () => Machine.AzimuthMotorActive.ShouldBeFalse();

        It should_have_stopped_shutter_motor = () => Machine.ShutterMotorActive.ShouldBeFalse();

        It should_have_no_rotation_direction = () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);

        Behaves_like<a_stopped_rotator> rotator;

        Behaves_like<an_offline_shutter> shutter;
        }

    [Subject(typeof(ReadyState), "triggers")]
    class when_in_ready_state_and_rotation_is_detected : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithReadyRotatorAndOfflineShutter().Build();

        Because of = () => Machine.AzimuthEncoderTickReceived(100);

        It should_transition_to_rotating_state = () => Machine.RotatorState.ShouldBeOfExactType<RotatingState>();

        It should_be_rotating = () => Machine.AzimuthMotorActive.ShouldBeTrue();

        It should_be_at_the_reported_position = () => Machine.AzimuthEncoderPosition.ShouldEqual(100);

        It should_should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();

        Behaves_like<a_moving_rotator> _;
        }

    [Subject(typeof(ReadyState), "triggers")]
    class when_in_ready_state_and_rotation_is_requested : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithReadyRotatorAndOfflineShutter().Build();

        Because of = () => Machine.RotateToAzimuthDegrees(targetAzimuth);

        It should_invoke_the_rotate_action = () =>
            A.CallTo(() => Actions.RotateToAzimuth((int)targetAzimuth)).MustHaveHappened(1, Times.Exactly);

        It should_transition_to_rotating_state = () => Machine.RotatorState.ShouldBeOfExactType<RotatingState>();

        It should_be_rotating = () => Machine.AzimuthMotorActive.ShouldBeTrue();

        It should_should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();

        const double targetAzimuth = 299.9;
        }

    [Subject(typeof(RotatingState), "triggers")]
    class when_in_rotating_state_and_status_is_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithRotatingRotator().Build();

        Because of = () =>
            Machine.HardwareStatusReceived(
                RotatorStatus.AtHome(ExpectedAzimuth).Circumference(ExpectedCircumference).Build());

        protected static bool ExpectedAtHome = true;

        protected static int ExpectedAzimuth = 213;

        protected static int ExpectedCircumference = 400;

        protected static int ExpectedHomePosition = 213;

        Behaves_like<a_stopped_rotator> _;

        Behaves_like<view_model_is_updated_from_rotator_status> model;
        }

    [Subject(typeof(RotatingState), "triggers")]
    class when_in_rotating_state_and_watchdog_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder.Build();
            testableRotatingState = new TestableRotatingState(Machine);
            Machine.Initialize(testableRotatingState);
            };

        Because of = () => testableRotatingState.TriggerWatchdogTimeout();

        It should_transition_to_request_status_state =
            () => Machine.RotatorState.ShouldBeOfExactType<RequestStatusState>();

        It should_send_a_status_request =
            () => A.CallTo(() => Actions.RequestRotatorStatus()).MustHaveHappenedOnceExactly();

        static TestableRotatingState testableRotatingState;

        class TestableRotatingState : RotatingState
            {
            /// <inheritdoc />
            public TestableRotatingState(ControllerStateMachine machine)
                : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_in_request_status_state_and_status_is_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithTimedOutRotator().Build();

        Because of = () => Machine.HardwareStatusReceived(RotatorStatus.Azimuth(ExpectedAzimuth).Build());

        It should_update_the_view_model_azimuth = () => Machine.AzimuthEncoderPosition.ShouldEqual(ExpectedAzimuth);

        Behaves_like<a_stopped_rotator> _;

        const int ExpectedAzimuth = 123;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_in_request_status_state_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder.Build();
            testableState = new TestableRequestStatusState(Machine);
            Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_send_emergency_stop =
            () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();

        It should_request_status = () => A.CallTo(() => Actions.RequestRotatorStatus()).MustHaveHappenedTwiceOrMore();

        static TestableRequestStatusState testableState;

        class TestableRequestStatusState : RequestStatusState
            {
            /// <inheritdoc />
            public TestableRequestStatusState(ControllerStateMachine machine)
                : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }
    }