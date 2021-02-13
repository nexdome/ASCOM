// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine.Shutter;

    [Behaviors]
    class a_closed_shutter : state_machine_behaviour
        {
        It should_not_be_moving = () => Machine.ShutterMotorActive.ShouldBeFalse();

        It should_not_have_a_direction = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);

        It should_be_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeTrue();

        It should_be_in_closed_state = () => Machine.ShutterState.ShouldBeOfExactType<ClosedState>();

        It should_have_closed_position = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Closed);
        }
    }