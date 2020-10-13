// This file is part of the TA.NexDome.AscomServer project
//
// Copyright © 2015-2020 Tigra Astronomy, all rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so. The Software comes with no warranty of any kind.
// You make use of the Software entirely at your own risk and assume all liability arising from your use thereof.
//
// File: ShutterStateSpecs.cs  Last modified: 2020-07-24@13:17 by Tim Long

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
    class when_shutter_is_offline : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithOfflineShutter().Build();

        It should_have_shutter_in_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();

        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();

        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(OfflineState), "startup")]
    class when_shutter_is_offline_and_xbee_connection_established : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithOfflineShutter().Build();

        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Online);

        It should_transition_to_request_status_state =
            () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();

        It should_update_the_view_model = () => Machine.ShutterLinkState.ShouldEqual(ShutterLinkState.Online);

        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();
        }

    [Subject(typeof(ClosedState), "triggers")]
    class when_closed_and_shutter_opening_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyClosed().Build();

        Because of = () => Machine.ShutterDirectionReceived(ShutterDirection.Opening);

        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();

        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);

        Behaves_like<a_moving_shutter> _;
        }

    [Subject(typeof(ClosedState), "triggers")]
    class when_closed_and_shutter_position_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyClosed().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);

        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();

        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);

        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);

        Behaves_like<a_moving_shutter> _;

        const int ExpectedStepPosition = 213;
        }

    [Subject(typeof(ClosedState), "triggers")]
    class when_closed_and_shutter_open_requested : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyClosed().Build();

        Because of = () => Machine.OpenShutter();

        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();

        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);

        It should_invoke_open_shutter_action =
            () => A.CallTo(() => Actions.OpenShutter()).MustHaveHappenedOnceExactly();

        Behaves_like<a_moving_shutter> _;
        }

    [Subject(typeof(ClosedState), "triggers")]
    class when_closed_and_link_goes_down : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyClosed().Build();

        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Detect);

        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    class when_opening_and_encoder_tick_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithOpeningShutter().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(50);

        It should_be_opening = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();

        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(50);

        Behaves_like<a_moving_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    class when_opening_and_fully_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithOpeningShutter().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyOpen().Build());

        It should_set_the_sesnsor_state = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Open);

        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    class when_opening_and_partially_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithOpeningShutter().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.PartiallyOpen().Build());

        It should_set_the_sesnsor_state = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Indeterminate);

        It should_be_in_open_state = () => Machine.ShutterState.ShouldBeOfExactType<OpenState>();

        Behaves_like<a_stopped_shutter> _;
        }

    [Subject(typeof(OpeningState), "triggers")]
    class when_opening_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder.Build();
            testableState = new TestableOpeningState(Machine);
            Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_send_emergency_stop =
            () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();

        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();

        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        static TestableOpeningState testableState;

        class TestableOpeningState : OpeningState
            {
            /// <inheritdoc />
            public TestableOpeningState(ControllerStateMachine machine)
                : base(machine) { }

            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_in_shutter_request_status_state_and_timeout_occurs : with_state_machine_context
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

        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();

        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        static TestableRequestStatusState testableState;

        class TestableRequestStatusState : OpeningState
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

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_when_in_shutter_request_status_state_and_closed_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterInRequestStatusState().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyClosed().Build());

        Behaves_like<a_closed_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_in_shutter_request_status_state_and_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterInRequestStatusState().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyOpen().Build());

        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_when_in_shutter_request_status_state_and_partially_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterInRequestStatusState().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.PartiallyOpen().Build());

        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_when_in_shutter_request_status_state_and_link_goes_offline : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterInRequestStatusState().Build();

        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Start);

        Behaves_like<an_offline_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    class when_when_in_shutter_request_status_state_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder.Build();

            testableState = new TestableRequestStatusState(Machine);
            Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_transition_to_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();

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

    [Subject(typeof(OpenState), "triggers")]
    class when_open_and_shutter_closing_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyOpen().Build();

        Because of = () => Machine.ShutterDirectionReceived(ShutterDirection.Closing);

        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();

        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);

        Behaves_like<a_moving_shutter> _;
        }

    [Subject(typeof(OpenState), "triggers")]
    class when_open_and_shutter_position_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyOpen().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);

        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();

        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);

        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);

        Behaves_like<a_moving_shutter> _;

        const int ExpectedStepPosition = 213;
        }

    [Subject(typeof(OpenState), "triggers")]
    class when_partially_open_and_greater_shutter_position_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterPartiallyOpen().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);

        It should_be_in_opening_state = () => Machine.ShutterState.ShouldBeOfExactType<OpeningState>();

        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);

        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);

        Behaves_like<a_moving_shutter> _;

        const int ExpectedStepPosition = 400; // partially open is 250 steps
        }

    [Subject(typeof(OpenState), "triggers")]
    class when_partially_open_and_lesser_shutter_position_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterPartiallyOpen().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);

        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();

        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);

        It should_update_the_view_model = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);

        Behaves_like<a_moving_shutter> _;

        const int ExpectedStepPosition = 100; // partially open is 250 steps
        }

    [Subject(typeof(OpenState), "triggers")]
    class when_open_and_shutter_close_requested : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithShutterFullyOpen().Build();

        Because of = () => Machine.CloseShutter();

        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();

        It should_be_closing = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);

        It should_invoke_close_shutter_action =
            () => A.CallTo(() => Actions.CloseShutter()).MustHaveHappenedOnceExactly();

        Behaves_like<a_moving_shutter> _;
        }

    [Subject(typeof(ClosingState), "triggers")]
    class when_closing_and_encoder_tick_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithClosingShutter().Build();

        Because of = () => Machine.ShutterEncoderTickReceived(ExpectedStepPosition);

        It should_be_opening = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);

        It should_be_in_closing_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosingState>();

        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(ExpectedStepPosition);

        Behaves_like<a_moving_shutter> _;

        const int ExpectedStepPosition = 100;
        }

    [Subject(typeof(ClosingState), "triggers")]
    class when_closing_and_closed_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithClosingShutter().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyClosed().Build());

        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(0);

        It should_be_in_closed_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosedState>();

        It should_set_the_sensor_state = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Closed);

        Behaves_like<a_stopped_shutter> _;

        const int ExpectedStepPosition = 0;
        }

    [Subject(typeof(ClosingState), "triggers")]
    class when_closing_and_partially_open_status_received : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithClosingShutter().Build();

        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.PartiallyOpen().Build());

        It should_update_the_view_model_position = () => Machine.ShutterStepPosition.ShouldEqual(250);

        It should_be_in_open_state = () => Machine.ShutterState.ShouldBeOfExactType<OpenState>();

        It should_set_the_disposition = () => Machine.ShutterDisposition.ShouldEqual(ShutterDisposition.Open);

        It should_set_the_limit_switches = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Indeterminate);

        Behaves_like<a_stopped_shutter> _;

        const int ExpectedStepPosition = 250;
        }

    [Subject(typeof(ClosingState), "triggers")]
    class when_closing_and_link_goes_offline : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder.WithClosingShutter().Build();

        Because of = () => Machine.ShutterLinkStateChanged(ShutterLinkState.Detect);

        Behaves_like<an_offline_shutter> _;

        const int ExpectedStepPosition = 100;
        }

    [Subject(typeof(ClosingState), "triggers")]
    class when_closing_and_timeout_occurs : with_state_machine_context
        {
        Establish context = () =>
            {
            Context = ContextBuilder.Build();

            testableState = new TestableClosingState(Machine);
            Machine.Initialize(testableState);
            };

        Because of = () => testableState.TriggerWatchdogTimeout();

        It should_send_emergency_stop =
            () => A.CallTo(() => Actions.PerformEmergencyStop()).MustHaveHappenedOnceExactly();

        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();

        It should_be_in_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();

        static TestableClosingState testableState;

        class TestableClosingState : ClosingState
            {
            public TestableClosingState(ControllerStateMachine machine)
                : base(machine) { }
            internal void TriggerWatchdogTimeout()
                {
                CancelTimeout(); // Cancels any pending timeout which would interfere with the test in progress.
                HandleTimeout(); // Manually trigger the timeout.
                }
            }
        }
    }