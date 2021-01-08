// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

using TA.NexDome.Common;

#pragma warning disable 0169    // Field not used, triggers on Behaves_like<>

namespace TA.NexDome.Specifications.DeviceInterface
    {
    using System;

    using Machine.Specifications;
    using TA.NexDome.Specifications.Fakes;


    [Subject(typeof(IRotatorStatus), "Instance creation")]
    class when_creating_a_rotator_status_from_received_event_data
        {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));

        Because of = () => status = factory.FromRotatorStatusPacket(RealWorldStatusPacket);

        It should_set_the_azimuth = () => status.Azimuth.ShouldEqual(450);

        It should_set_at_home = () => status.AtHome.ShouldBeTrue();

        It should_set_circumference = () => status.DomeCircumference.ShouldEqual(55080);

        It should_set_home_offset = () => status.HomePosition.ShouldEqual(34567);

        It should_set_deadzone = () => status.DeadZone.ShouldEqual(75);

        static ControllerStatusFactory factory;

        static IRotatorStatus status;

        const string RealWorldStatusPacket = ":SER,450,1,55080,34567,75#";
        }

    [Subject(typeof(IShutterStatus), "Instance creation")]
    class when_creating_a_shutter_status_from_received_event_data
        {
        Establish context = () =>
            factory = new ControllerStatusFactory(new FakeClock(DateTime.MinValue.ToUniversalTime()));

        Because of = () => status = factory.FromShutterStatusPacket(RealWorldStatusPacket);

        It should_set_the_position = () => status.Position.ShouldEqual(-600);

        It should_be_open = () => status.OpenSensorActive.ShouldBeTrue();

        It should_not_be_closed = () => status.ClosedSensorActive.ShouldBeFalse();

        It should_have_expected_limit_of_travel = () => status.LimitOfTravel.ShouldEqual(46000);

        static ControllerStatusFactory factory;

        static IShutterStatus status;

        const string RealWorldStatusPacket = ":SES,-600,46000,1,0#";
        }
    }