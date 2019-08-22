// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using Machine.Specifications;

    [Behaviors]
    class view_model_is_updated_from_rotator_status : state_machine_behaviour
        {
        protected static int ExpectedAzimuth;

        protected static int ExpectedCircumference;

        protected static int ExpectedHomePosition;

        protected static bool ExpectedAtHome;

        It should_update_the_view_model_azimuth = () => Machine.AzimuthEncoderPosition.ShouldEqual(ExpectedAzimuth);

        It should_update_the_view_model_at_home = () => Machine.AtHome.ShouldEqual(ExpectedAtHome);

        It should_update_the_view_model_circumference =
            () => Machine.DomeCircumference.ShouldEqual(ExpectedCircumference);

        It should_update_the_view_model_home_position = () => Machine.HomePosition.ShouldEqual(ExpectedHomePosition);
        }
    }