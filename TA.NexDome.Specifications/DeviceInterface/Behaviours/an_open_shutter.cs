using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    internal class an_open_shutter : state_machine_behaviour
        {
        It should_not_be_moving = () => Machine.ShutterMotorActive.ShouldBeFalse();
        It should_not_have_a_direction =
            () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_be_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeTrue();
        It should_be_in_open_state = () => Machine.ShutterState.ShouldBeOfExactType<OpenState>();
        It should_have_open_position = () => Machine.ShutterPosition.ShouldEqual(SensorState.Open);
        }
    }
