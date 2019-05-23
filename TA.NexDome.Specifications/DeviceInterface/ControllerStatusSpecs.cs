// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using System;
using Machine.Specifications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Contexts;
using TA.NexDome.Specifications.DeviceInterface.Behaviours;
using TA.NexDome.Specifications.Fakes;

#pragma warning disable 0169    // Field not used, triggers on Behaves_like<>

namespace TA.NexDome.Specifications.DeviceInterface
{
    [Ignore("Deprecated code")]
    [Subject(typeof(HardwareStatus), "creation")]
    internal class when_creating_a_status
    {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));

        Because of = () => actual = factory.FromStatusPacket(RealWorldStatusPacket);
        It should_be_v4 = () => actual.FirmwareVersion.ShouldEqual("V4");
        It should_have_circumference = () => actual.DomeCircumference.ShouldEqual(704);
        It should_have_home_position = () => actual.HomePosition.ShouldEqual(293);
        It should_have_coast = () => actual.Coast.ShouldEqual(1);
        It should_have_azimuth = () => actual.CurrentAzimuth.ShouldEqual(289);
        It should_not_be_slaved = () => actual.Slaved.ShouldBeFalse();
        It should_have_indeterminate_shutter = () => actual.ShutterSensor.ShouldEqual(SensorState.Indeterminate);
        It should_have_closed_support_ring = () => actual.DsrSensor.ShouldEqual(SensorState.Closed);
        It should_be_at_home = () => actual.AtHome.ShouldBeTrue();
        It should_have_home_ccw = () => actual.HomeCounterClockwise.ShouldEqual(287);
        It should_have_home_cw = () => actual.HomeClockwise.ShouldEqual(299);
        It should_have_user_pins = () => actual.UserPins.ShouldEqual(Octet.Zero);
        It should_have_weather_age = () => actual.WeatherAge.ShouldEqual(0);
        It should_have_wind_direction = () => actual.WindDirection.ShouldEqual(0);
        It should_have_wind_speed = () => actual.WindSpeed.ShouldEqual(0);
        It should_have_temperature = () => actual.Temperature.ShouldEqual(112);
        It should_have_humidity = () => actual.Humidity.ShouldEqual(50);
        It should_be_dry = () => actual.Wetness.ShouldEqual(0);
        It should_not_be_snowing = () => actual.Snow.ShouldEqual(0);
        It should_have_wind_peak = () => actual.WindPeak.ShouldEqual(0);
        It should_have_lx200_azimuth = () => actual.Lx200Azimuth.ShouldEqual(180);
        It should_have_dead_zone = () => actual.DeadZone.ShouldEqual(5);

        It should_have_offset = () => actual.Offset.ShouldEqual(5);

        //ToDo: fill in the other fields
        static IHardwareStatus actual;

        static ControllerStatusFactory factory;

        // This status packet was captured from real hardware.
        const string RealWorldStatusPacket = "V4,704,293,1,289,0,0,1,0,287,299,0,0,0,0,112,50,0,0,0,180,5,5";
    }

    [Subject(typeof(DeviceController), "property updates")]
    internal class when_a_status_packet_is_received : with_device_controller_context
    {
        Establish context = () => Context = DeviceControllerContextBuilder
            .WithClosedConnection("Simulator:Fast")
            .Build();

        Because of = () =>
        {
            Controller.Open(performOnConnectActions: false);
            Context.Actions.RequestHardwareStatus();
            Context.StateMachine.WaitForReady(TimeSpan.FromSeconds(5));
        };

        static IHardwareStatus receivedStatus;
        Behaves_like<a_stopped_dome> stopped_dome;
        const string RealWorldStatusPacket = "V4,704,293,1,289,0,0,1,0,287,299,0,0,0,0,112,50,0,0,0,180,5,5\n";
    }

[Subject(typeof(IRotatorStatus), "Instance creation")]
internal class when_creating_a_rotator_status_from_received_event_data
    {
    Because of = () => status = ControllerStatusFactory.FromRotatorStatusPacket(RealWorldStatusPacket) ;
    It should_set_the_azimuth = () => status.Azimuth.ShouldEqual(450);
    It should_set_at_home = () => status.AtHome.ShouldBeTrue();
    It should_set_circumference = () => status.DomeCircumference.ShouldEqual(55080);
    It should_set_home_offset = () => status.HomePosition.ShouldEqual(34567);
    It should_set_deadzone = () => status.DeadZone.ShouldEqual(0);
    static IRotatorStatus status;
    static ControllerStatusFactory factory;
    const string RealWorldStatusPacket = "SER,450,1,55080,34567,0#";
    }
}