// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: a_dome_with_a_moving_shutter.cs  Last modified: 2018-03-14@00:17 by Tim Long

using Machine.Specifications;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    internal class a_dome_with_a_moving_shutter : device_controller_behaviour
        {
        It should_not_have_rotation_direction = () => Controller.AzimuthDirection.ShouldEqual(RotationDirection.None);
        It should_not_indicate_that_the_azimuth_motor_is_active =
            () => Controller.AzimuthMotorActive.ShouldBeFalse();
        It should_indicate_that_the_shutter_motor_is_active = () => Controller.ShutterMotorActive.ShouldBeTrue();
        It should_indicate_that_something_is_moving = () => Controller.IsMoving.ShouldBeTrue();
        }
    }