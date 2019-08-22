// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine.Rotator;
    using TA.NexDome.SharedTypes;

    [Behaviors]
    class a_stopped_rotator : state_machine_behaviour
        {
        It should_not_be_moving = () => Machine.AzimuthMotorActive.ShouldBeFalse();

        It should_not_have_a_rotation_direction = () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);

        It should_be_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeTrue();

        It should_be_in_ready_state = () => Machine.RotatorState.ShouldBeOfExactType<ReadyState>();
        }
    }