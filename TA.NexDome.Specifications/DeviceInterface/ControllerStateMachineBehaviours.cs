// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: ControllerStateMachineBehaviours.cs  Last modified: 2018-08-30@02:45 by Tim Long

using JetBrains.Annotations;
using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;

#pragma warning disable CS0649

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Behaviors]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class ShutterMoving
        {
        protected static ControllerStateMachine Machine;
        It should_indicate_shutter_motor_active = () => Machine.ShutterMotorActive.ShouldBeTrue();
        It should_not_indicate_azimuth_movement = () => Machine.AzimuthMotorActive.ShouldBeFalse();
        It should_not_indicate_rotation_direction = () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);
        }

    [Behaviors]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    internal class AzimuthRotation
        {
        [UsedImplicitly] protected static ControllerStateMachine Machine;
        It should_not_indicate_shutter_motor_active = () => Machine.ShutterMotorActive.ShouldBeFalse();
        It should_not_indicate_shutter_direction =
            () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_not_indicate_any_shutter_motor_current = () => Machine.ShutterMotorCurrent.ShouldEqual(0);
        It should_indicate_azimuth_movement = () => Machine.AzimuthMotorActive.ShouldBeTrue();
        }
    }