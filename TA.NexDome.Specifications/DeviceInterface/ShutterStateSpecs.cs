using FakeItEasy;
using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;
using TA.NexDome.Specifications.DeviceInterface.Behaviours;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Subject(typeof(OfflineState), "startup")]
    internal class when_shutter_is_offline : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithOfflineShutter()
            .Build();
        Behaves_like<an_offline_shutter> _;
        It should_have_shutter_in_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();
        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();
        }

    [Subject(typeof(OfflineState), "startup")]
    internal class when_shutter_is_offline_and_xbee_connection_established : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithOfflineShutter()
            .Build();
        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Online);
        It should_transition_to_request_status_state =
            () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();
        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();
        It should_update_the_view_model = () => Machine.ShutterLinkState.ShouldEqual(ShutterLinkState.Online);
        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();
        }

    [Subject(typeof(ClosedState), "triggers")]
    internal class when_closed_and_shutter_opening_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyClosed()
            .Build();
        Because of = () => Machine.ShutterDirectionReceived(ShutterDirection.Opening);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();
        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        }

    [Subject(typeof(ClosedState), "triggers")]
    internal class when_closed_and_shutter_position_received : with_state_machine_context
        {
        const int ExpectedStepPosition = 213;
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyClosed()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();
        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);
        }

    [Subject(typeof(ClosedState), "triggers")]
    internal class when_closed_and_shutter_open_requested : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyClosed()
            .Build();
        Because of = () => Machine.OpenShutter();
        Behaves_like<a_moving_shutter> _;
        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();
        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        It should_invoke_open_shutter_action =
            () => A.CallTo(() => Actions.OpenShutter()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(ClosedState), "triggers")]
    internal class when_closed_and_link_goes_down : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyClosed()
            .Build();
        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Detect);
        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    internal class when_opening_and_encoder_tick_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithOpeningShutter()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(50);
        Behaves_like<a_moving_shutter> _;
        It should_be_opening = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();
        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(50);
        }

    [Subject(typeof(OpeningState), "triggers")]
    internal class when_opening_and_fully_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithOpeningShutter()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyOpen().Build());
        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    internal class when_opening_and_partially_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithOpeningShutter()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.PartiallyOpen().Build());
        Behaves_like<a_stopped_shutter> _;
        }


    [Subject(typeof(OpeningState), "triggers")]
    internal class when_opening_and_timeout_occurs : with_state_machine_context
        {
        private static TestableOpeningState testableState;
        Establish context = () =>
            {
                Context = ContextBuilder
                    .Build();
                testableState = new TestableOpeningState(Machine);
                Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_send_emergency_stop =
            () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();
        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();
        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        private class TestableOpeningState : OpeningState
            {
            /// <inheritdoc />
            public TestableOpeningState(ControllerStateMachine machine) : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_in_shutter_request_status_state_and_timeout_occurs : with_state_machine_context
        {
        private static TestableRequestStatusState testableState;
        Establish context = () =>
            {
                Context = ContextBuilder
                    .Build();
                testableState = new TestableRequestStatusState(Machine);
                Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_send_emergency_stop =
            () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();
        It should_request_status =
            () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();
        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        private class TestableRequestStatusState : OpeningState
            {
            /// <inheritdoc />
            public TestableRequestStatusState(ControllerStateMachine machine) : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_when_in_shutter_request_status_state_and_closed_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyClosed().Build());
        Behaves_like<a_closed_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_when_in_shutter_request_status_state_and_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyOpen().Build());
        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_when_in_shutter_request_status_state_and_partially_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.PartiallyOpen().Build());
        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_when_in_shutter_request_status_state_and_link_goes_offline : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Start);
        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_when_in_shutter_request_status_state_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder
                .Build();

            testableState = new TestableRequestStatusState(Machine);
            Machine.Initialize(testableState);
            };
        Because of = () => testableState.TriggerWatchdogTimeout();
        It should_perform_emergency_stop = () => A.CallTo(()=> Machine.ControllerActions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();
        It should_request_status = () => A.CallTo(()=>Machine.ControllerActions.RequestShutterStatus()).MustHaveHappenedTwiceExactly();
        static TestableRequestStatusState testableState;

        private class TestableRequestStatusState : RequestStatusState {
            /// <inheritdoc />
            public TestableRequestStatusState(ControllerStateMachine machine) : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }

            }
        }

    [Subject(typeof(OpenState), "triggers")]
    internal class when_open_and_shutter_closing_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyOpen()
            .Build();
        Because of = () => Machine.ShutterDirectionReceived(ShutterDirection.Closing);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();
        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        }

    [Subject(typeof(OpenState), "triggers")]
    internal class when_open_and_shutter_position_received : with_state_machine_context
        {
        const int ExpectedStepPosition = 213;
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyOpen()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();
        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);
        }

    [Subject(typeof(OpenState), "triggers")]
    internal class when_partially_open_and_greater_shutter_position_received : with_state_machine_context
        {
        const int ExpectedStepPosition = 400;   // partially open is 250 steps
        Establish context = () => Context = ContextBuilder
            .WithShutterPartiallyOpen()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();
        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);
        }

    [Subject(typeof(OpenState), "triggers")]
    internal class when_partially_open_and_lesser_shutter_position_received : with_state_machine_context
        {
        const int ExpectedStepPosition = 100;   // partially open is 250 steps
        Establish context = () => Context = ContextBuilder
            .WithShutterPartiallyOpen()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);
        Behaves_like<a_moving_shutter> _;
        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();
        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);
        }

    [Subject(typeof(OpenState), "triggers")]
    internal class when_open_and_shutter_close_requested : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterFullyOpen()
            .Build();
        Because of = () => Machine.CloseShutter();
        Behaves_like<a_moving_shutter> _;
        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();
        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        It should_invoke_close_shutter_action =
            () => A.CallTo(() => Actions.CloseShutter()).MustHaveHappenedOnceExactly();
        }

    [Subject(typeof(ClosingState), "triggers")]
    internal class when_closing_and_encoder_tick_received : with_state_machine_context
        {
        const int ExpectedStepPosition = 100;
        Establish context = () => Context = ContextBuilder
            .WithClosingShutter()
            .Build();
        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);
        Behaves_like<a_moving_shutter> _;
        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();
        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);
        }

    [Subject(typeof(ClosingState), "triggers")]
    internal class when_closing_and_link_goes_offline : with_state_machine_context
        {
        const int ExpectedStepPosition = 100;
        Establish context = () => Context = ContextBuilder
            .WithClosingShutter()
            .Build();
        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Detect);
        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(ClosingState), "triggers")]
    internal class when_closing_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
                Context = ContextBuilder
                    .Build();

                testableState = new TestableClosingState(Machine);
                Machine.Initialize(testableState);
            };
        Because of = () => testableState.TriggerWatchdogTimeout();
        It should_send_emergency_stop = () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();
        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();
        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        static TestableClosingState testableState;

        private class TestableClosingState : ClosingState
            {
            public TestableClosingState(ControllerStateMachine machine) : base(machine) { }
            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }
    }