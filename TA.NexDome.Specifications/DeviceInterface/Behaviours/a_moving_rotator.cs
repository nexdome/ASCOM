// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    [Behaviors]
    class a_moving_rotator : state_machine_behaviour
        {
        It should_have_an_active_motor = () => Machine.AzimuthMotorActive.ShouldBeTrue();

        It should_be_moving = () => Machine.IsMoving.ShouldBeTrue();

        It should_not_be_ready = () => Machine.RotatorInReadyState.WaitOne(0).ShouldBeFalse();
        }
    }