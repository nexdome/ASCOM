// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine.Shutter;
    using TA.NexDome.SharedTypes;

    [Behaviors]
    class an_open_shutter : state_machine_behaviour
        {
        It should_not_be_moving = () => Machine.ShutterMotorActive.ShouldBeFalse();

        It should_not_have_a_direction = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);

        It should_be_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeTrue();

        It should_be_in_open_state = () => Machine.ShutterState.ShouldBeOfExactType<OpenState>();

        It should_have_open_disposition = () => Machine.ShutterDisposition.ShouldEqual(ShutterDisposition.Open);

        It should_not_be_at_the_closed_limit = () => Machine.ShutterLimitSwitches.ShouldNotEqual(SensorState.Closed);
        }
    }