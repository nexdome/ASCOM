// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    [Behaviors]
    class a_directionless_rotating_dome : device_controller_behaviour
        {
        It should_be_rotating = () => Controller.AzimuthMotorActive.ShouldBeTrue();

        It should_have_a_stationary_shutter = () => Controller.ShutterMotorActive.ShouldBeFalse();

        It should_indicate_that_something_is_moving = () => Controller.IsMoving.ShouldBeTrue();
        }
    }