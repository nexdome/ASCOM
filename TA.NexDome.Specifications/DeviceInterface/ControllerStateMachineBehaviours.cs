// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

#pragma warning disable CS0649

namespace TA.NexDome.Specifications.DeviceInterface
    {
    using JetBrains.Annotations;

    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine;

    [Behaviors]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class ShutterMoving
        {
        protected static ControllerStateMachine Machine;

        It should_indicate_shutter_motor_active = () => Machine.ShutterMotorActive.ShouldBeTrue();

        It should_not_indicate_azimuth_movement = () => Machine.AzimuthMotorActive.ShouldBeFalse();

        It should_not_indicate_rotation_direction = () => Machine.AzimuthDirection.ShouldEqual(RotationDirection.None);
        }

    [Behaviors]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    class AzimuthRotation
        {
        [UsedImplicitly]
        protected static ControllerStateMachine Machine;

        It should_not_indicate_shutter_motor_active = () => Machine.ShutterMotorActive.ShouldBeFalse();

        It should_not_indicate_shutter_direction =
            () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);

        It should_not_indicate_any_shutter_motor_current = () => Machine.ShutterMotorCurrent.ShouldEqual(0);

        It should_indicate_azimuth_movement = () => Machine.AzimuthMotorActive.ShouldBeTrue();
        }
    }