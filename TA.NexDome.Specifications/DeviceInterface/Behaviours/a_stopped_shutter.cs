// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.

using System.Diagnostics;
using Machine.Specifications;
using Machine.Specifications.Controller;
using TA.NexDome.DeviceInterface.StateMachine.Shutter;
using TA.NexDome.SharedTypes;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours {
    [Behaviors]
    internal class a_stopped_shutter : state_machine_behaviour
        {
        It should_draw_no_shutter_current = () => Machine.ShutterMotorCurrent.ShouldEqual(0);
        It should_not_have_a_shutter_direction = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_have_a_stationary_shutter = () => Machine.ShutterMotorActive.ShouldBeFalse();
        It should_be_in_a_stopped_state = () => IsInStoppedState.ShouldBeTrue();
        static bool IsInStoppedState => Machine.ShutterState is OfflineState || Machine.ShutterState is OpenState ||
                                        Machine.ShutterState is ClosedState;
        }
    [Behaviors]
    internal class an_offline_shutter : state_machine_behaviour
        {
        It should_be_offline = () => Machine.ShutterState.ShouldBeOfExactType<OfflineState>();
        It should_not_have_a_shutter_direction = () => Machine.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_have_a_stationary_shutter = () => Machine.ShutterMotorActive.ShouldBeFalse();
        }
    }