// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: a_directionless_rotating_dome.cs  Last modified: 2018-03-14@00:27 by Tim Long

using Machine.Specifications;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    internal class a_directionless_rotating_dome : device_controller_behaviour
        {
        It should_be_rotating = () => Controller.AzimuthMotorActive.ShouldBeTrue();
        It should_have_a_stationary_shutter = () => Controller.ShutterMotorActive.ShouldBeFalse();
        It should_indicate_that_something_is_moving = () => Controller.IsMoving.ShouldBeTrue();
        }
    }