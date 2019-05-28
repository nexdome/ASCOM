
using FakeItEasy;
using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Rotator;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;
using TA.NexDome.Specifications.DeviceInterface.Behaviours;
using RequestStatusState = TA.NexDome.DeviceInterface.StateMachine.Rotator.RequestStatusState;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Subject(typeof(ControllerStateMachine), "startup")]
    internal class when_creating_the_controller_state_machine : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithReadyRotatorAndOfflineShutter()
            .Build();
        Behaves_like<a_stopped_rotator> rotator;
        Behaves_like<an_offline_shutter> shutter;
        It should_have_rotator_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();
        It should_have_shutter_in_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();
        It should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();
        It should_be_at_azimuth_zero = () => Machine.AzimuthEncoderPosition.ShouldEqual(0);
        It should_signal_rotator_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeTrue();
        It should_not_signal_shutter_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();
        It should_have_stopped_rotator_motor = () => Machine.AzimuthMotorActive.ShouldBeFalse();
        It should_have_stopped_shutter_motor = () => Machine.ShutterMotorActive.ShouldBeFalse();
        It should_have_no_rotation_direction = () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);
        }

    [Subject(typeof(ReadyState), "triggers")]
    internal class when_in_ready_state_and_rotation_is_detected : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithReadyRotatorAndOfflineShutter()
            .Build();
        Because of = () => Machine.AzimuthEncoderTickReceived(100);
        Behaves_like<a_moving_rotator> _;
        It should_transition_to_rotating_state = () => Machine.RotatorState.ShouldBeOfExactType<RotatingState>();
        It should_be_rotating = () => Machine.AzimuthMotorActive.ShouldBeTrue();
        It should_be_at_the_reported_position = () => Machine.AzimuthEncoderPosition.ShouldEqual(100);
        It should_should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();
        }

    [Subject(typeof(ReadyState), "triggers")]
    internal class when_in_ready_state_and_rotation_is_requested : with_state_machine_context
        {
        const double targetAzimuth = 299.9;
        Establish context = () => Context = ContextBuilder
            .WithReadyRotatorAndOfflineShutter()
            .Build();
        Because of = () => Machine.RotateToAzimuthDegrees(targetAzimuth);
        It should_invoke_the_rotate_action =
            () => A.CallTo(() => Actions.RotateToAzimuth((int)targetAzimuth)).MustHaveHappened(1, Times.Exactly);
        It should_transition_to_rotating_state = () => Machine.RotatorState.ShouldBeOfExactType<RotatingState>();
        It should_be_rotating = () => Machine.AzimuthMotorActive.ShouldBeTrue();
        It should_should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();
        }

    [Subject(typeof(RotatingState), "triggers")]
    internal class when_in_rotating_state_and_status_is_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithRotatingRotator()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(A.Fake<IRotatorStatus>());
        Behaves_like<a_stopped_rotator> _;
        }

    [Subject(typeof(RotatingState), "triggers")]
    internal class when_in_rotating_state_and_watchdog_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
                Context = ContextBuilder
                    .Build();
                testableRotatingState = new TestableRotatingState(Machine);
                Machine.Initialize(testableRotatingState);
            };
        Because of = () => testableRotatingState.TriggerWatchdogTimeout();

        It should_transition_to_request_status_state =
            () => Machine.RotatorState.ShouldBeOfExactType<RequestStatusState>();
        It should_send_a_status_request = () => A.CallTo(() => Actions.RequestRotatorStatus()).MustHaveHappenedOnceExactly();
        static TestableRotatingState testableRotatingState;

        private class TestableRotatingState : RotatingState
            {
            /// <inheritdoc />
            public TestableRotatingState(ControllerStateMachine machine) : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout();    // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout();    // Manually trigger the timeout.
                }
            }
        }
    }