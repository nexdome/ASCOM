using FakeItEasy;
using Machine.Specifications;
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
        It should_transition_to_request_status_state = () => Machine.ShutterState.ShouldBeOfExactType<RequestStatusState>();
        It should_request_status = () => A.CallTo(() => Actions.RequestShutterStatus()).MustHaveHappenedOnceExactly();
        It should_update_the_view_model = () => Machine.ShutterLinkState.ShouldEqual(ShutterLinkState.Online);
        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_requesting_status_and_status_received_and_shutter_open : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyOpen().Build());
        Behaves_like<an_open_shutter> _;
        }

    [Subject(typeof(RequestStatusState), "triggers")]
    internal class when_requesting_status_and_status_received_and_shutter_closed : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithShutterInRequestStatusState()
            .Build();
        Behaves_like<a_closed_shutter> _;
        Because of = () => Machine.HardwareStatusReceived(ShutterStatus.FullyClosed().Build());
        }

    [Subject(typeof(RequestStatusState), "triggers")]
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
    }