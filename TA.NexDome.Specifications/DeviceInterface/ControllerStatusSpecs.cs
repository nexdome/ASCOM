// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using Machine.Specifications;
using System;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Fakes;

#pragma warning disable 0169    // Field not used, triggers on Behaves_like<>

namespace TA.NexDome.Specifications.DeviceInterface
    {
    [Ignore("Deprecated code")]
    [Subject(typeof(ControllerStatusFactory), "creation")]
    internal class when_creating_a_rotator_status
        {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));

        Because of = () => actual = factory.FromRotatorStatusPacket(RealWorldStatusPacket);
        It should_have_circumference = () => actual.DomeCircumference.ShouldEqual(704);
        It should_have_home_position = () => actual.HomePosition.ShouldEqual(293);
        It should_have_azimuth = () => actual.Azimuth.ShouldEqual(289);
        It should_not_be_at_home = () => actual.AtHome.ShouldBeFalse();
        It should_have_dead_zone = () => actual.DeadZone.ShouldEqual(5);
        static IRotatorStatus actual;
        static ControllerStatusFactory factory;
        const string RealWorldStatusPacket = "SER,1530,0,55080,0,0#"; // Captured from real hardware
        }


    [Subject(typeof(IRotatorStatus), "Instance creation")]
    internal class when_creating_a_rotator_status_from_received_event_data
        {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));
        Because of = () => status = factory.FromRotatorStatusPacket(RealWorldStatusPacket);
        It should_set_the_azimuth = () => status.Azimuth.ShouldEqual(450);
        It should_set_at_home = () => status.AtHome.ShouldBeTrue();
        It should_set_circumference = () => status.DomeCircumference.ShouldEqual(55080);
        It should_set_home_offset = () => status.HomePosition.ShouldEqual(34567);
        It should_set_deadzone = () => status.DeadZone.ShouldEqual(0);
        static IRotatorStatus status;
        static ControllerStatusFactory factory;
        const string RealWorldStatusPacket = ":SER,450,1,55080,34567,0#";
        }

    [Subject(typeof(IShutterStatus), "Instance creation")]
    internal class when_creating_a_shutter_status_from_received_event_data
        {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));
        Because of = () => status = factory.FromShutterStatusPacket(RealWorldStatusPacket);
        It should_set_the_position = () => status.Position.ShouldEqual(-600);
        It should_be_open = () => status.OpenSensorActive.ShouldBeTrue();
        It should_not_be_closed = () => status.ClosedSensorActive.ShouldBeFalse();
        It should_have_expected_limit_of_travel = () => status.LimitOfTravel.ShouldEqual(46000);
        static IShutterStatus status;
        static ControllerStatusFactory factory;
        const string RealWorldStatusPacket = ":SES,-600,46000,1,0#";
        }
    }