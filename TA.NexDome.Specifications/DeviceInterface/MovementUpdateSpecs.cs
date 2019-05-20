// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: MovementUpdateSpecs.cs  Last modified: 2018-09-16@15:47 by Tim Long

using Machine.Specifications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;
using TA.NexDome.Specifications.DeviceInterface.Behaviours;

#pragma warning disable 0169    // Field not used, triggers on Behaves_like<>

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Subject(typeof(DeviceController), "Encoder Ticks")]
    internal class when_an_encoder_tick_is_received : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("P99\n")
            .Build();

        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_update_the_position_property = () => Controller.AzimuthEncoderPosition.ShouldEqual(99);
        Behaves_like<a_directionless_rotating_dome> _;
        }

    [Subject(typeof(DeviceController), "Direction")]
    internal class when_the_dome_begins_to_rotate_counterclockwise : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("L")
            .Build();
        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_be_rotating_counter_clockwise =
            () => Controller.AzimuthDirection.ShouldEqual(RotationDirection.CounterClockwise);
        Behaves_like<a_rotating_dome> _;
        }

    [Subject(typeof(DeviceController), "Direction")]
    internal class when_the_dome_begins_to_rotate_clockwise : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("R")
            .Build();

        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_be_rotating_clockwise = () => Controller.AzimuthDirection.ShouldEqual(RotationDirection.Clockwise);
        Behaves_like<a_rotating_dome> _;
        }

    [Subject(typeof(DeviceController), "Shutter Current")]
    internal class when_the_device_sends_a_shutter_current_reading : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("Z15\n")
            .Build();

        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_update_the_shutter_current_property = () => Controller.ShutterMotorCurrent.ShouldEqual(15);
        Behaves_like<a_dome_with_a_moving_shutter> _;
        }

    [Subject(typeof(DeviceController), "Shutter Direction")]
    internal class when_the_shutter_begins_to_close : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("C")
            .Build();
        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_be_closing = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        Behaves_like<a_dome_with_a_moving_shutter> _;
        }

    [Subject(typeof(DeviceController), "Shutter redundant operations")]
    internal class when_shutter_close_received_and_shutter_not_closed : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithStateMachineInitializedAndReady("Fake")
            .WithIndeterminateShutter()
            .Build();
        Because of = () => Controller.CloseShutter();
        It should_be_closing = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.Closing);
        It should_send_the_close_command = () => FakeChannel.SendLog.ShouldEqual(Constants.CmdClose);
        }

    [Subject(typeof(DeviceController), "Shutter redundant operations")]
    internal class when_shutter_open_received_and_shutter_not_open : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithStateMachineInitializedAndReady("Fake")
            .WithIndeterminateShutter()
            .Build();
        Because of = () => Controller.OpenShutter();
        It should_be_opening = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        It should_send_the_open_command = () => FakeChannel.SendLog.ShouldEqual(Constants.CmdOpen);
        }

    [Subject(typeof(DeviceController), "Shutter redundant operations")]
    internal class when_shutter_open_received_and_shutter_already_open : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithStateMachineInitializedAndReady("Fake")
            .WithOpenShutter()
            .Build();
        Because of = () => { Controller.OpenShutter(); };
        It should_not_move = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_be_open = () => Controller.ShutterPosition.ShouldEqual(SensorState.Open);
        It should_not_send_any_command = () => FakeChannel.SendLog.ShouldBeEmpty();
        }

    [Subject(typeof(DeviceController), "Shutter redundant operations")]
    internal class when_shutter_close_received_and_shutter_already_closed : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithStateMachineInitializedAndReady("Fake")
            .WithClosedShutter()
            .Build();
        Because of = () => { Controller.CloseShutter(); };
        It should_not_move = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.None);
        It should_be_closed = () => Controller.ShutterPosition.ShouldEqual(SensorState.Closed);
        It should_not_send_any_command = () => FakeChannel.SendLog.ShouldBeEmpty();
        }

    [Subject(typeof(DeviceController), "Shutter Direction")]
    internal class when_the_shutter_begins_to_open : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Fake")
            .WithFakeResponse("O")
            .Build();
        Because of = () =>
            {
            Controller.Open(performOnConnectActions: false);
            Channel.Send(string.Empty);
            };
        It should_be_opening = () => Controller.ShutterMovementDirection.ShouldEqual(ShutterDirection.Opening);
        Behaves_like<a_dome_with_a_moving_shutter> _;
        }

    [Subject(typeof(DeviceController), "emergency stop")]
    internal class when_the_client_requests_an_emergency_stop : with_device_controller_context
        {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithOpenConnection("Fake")
            .Build();
        Because of = () => Controller.RequestEmergencyStop();
        It should_send_the_emergency_stop_command_three_times =
            () => FakeChannel.SendLog.ShouldEqual("STOP\nSTOP\nSTOP\n");
        }
    }