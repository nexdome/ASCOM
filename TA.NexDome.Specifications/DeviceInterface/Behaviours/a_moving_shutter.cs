// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    [Behaviors]
    class a_moving_shutter : state_machine_behaviour
        {
        It should_be_moving = () => Machine.ShutterMotorActive.ShouldBeTrue();

        It should_have_a_direction = () => Machine.ShutterMovementDirection.ShouldNotEqual(ShutterDirection.None);

        It should_not_signal_ready = () => Machine.ShutterInReadyState.WaitOne(0).ShouldBeFalse();

        It should_have_open_position = () => Machine.ShutterLimitSwitches.ShouldEqual(SensorState.Open);
        }
    }