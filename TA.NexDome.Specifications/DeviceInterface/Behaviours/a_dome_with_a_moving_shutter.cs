// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    using TA.NexDome.SharedTypes;

    [Behaviors]
    class a_dome_with_a_moving_shutter : device_controller_behaviour
        {
        It should_not_have_rotation_direction = () => Controller.AzimuthDirection.ShouldEqual(RotationDirection.None);

        It should_not_indicate_that_the_azimuth_motor_is_active = () => Controller.AzimuthMotorActive.ShouldBeFalse();

        It should_indicate_that_the_shutter_motor_is_active = () => Controller.ShutterMotorActive.ShouldBeTrue();

        It should_indicate_that_something_is_moving = () => Controller.IsMoving.ShouldBeTrue();
        }
    }