using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.DeviceInterface.StateMachine.Rotator;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Subject(typeof(ControllerStateMachine), "startup")]
    internal class when_creating_the_controller_state_machine : with_state_machine_context
        {
        Establish context = () => Context = ContextBuilder
            .WithReadyRotatorAndOfflineShutter()
            .Build();
        It should_have_rotator_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();
        It should_have_shutter_in_offline_state = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();
        It should_not_be_at_home = () => Machine.AtHome.ShouldBeFalse();
        It should_be_at_azimuth_zero = () => Machine.AzimuthEncoderPosition.ShouldEqual(0);
        }
    }