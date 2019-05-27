using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine.Rotator;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    internal class a_stopped_rotator : state_machine_behaviour
        {
        It should_not_be_moving = () => Machine.AzimuthMotorActive.ShouldBeFalse();
        It should_not_have_a_rotation_direction =
            () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);
        It should_be_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeTrue();
        It should_be_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();
        }

    [Behaviors]
    internal class a_moving_rotator : state_machine_behaviour
        {
        It should_have_an_active_motor = () => Machine.AzimuthMotorActive.ShouldBeTrue();
        It should_be_moving = () => Machine.IsMoving.ShouldBeTrue();
        It should_not_be_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeFalse();
        }
    }