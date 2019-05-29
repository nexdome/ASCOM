// Copyright © Tigra Astronomy, all rights reserved.
using Machine.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    [Behaviors]
    class view_model_is_updated_from_rotator_status : state_machine_behaviour
        {
        protected static int ExpectedAzimuth;
        protected static int ExpectedCircumference;
        protected static int ExpectedHomePosition;
        protected static bool ExpectedAtHome;
        It should_update_the_view_model_azimuth = () => Machine.AzimuthEncoderPosition.ShouldEqual(ExpectedAzimuth);
        It should_update_the_view_model_at_home = () => Machine.AtHome.ShouldEqual(ExpectedAtHome);
        It should_update_the_view_model_circumference = () => Machine.DomeCircumference.ShouldEqual(ExpectedCircumference);
        It should_update_the_view_model_home_position = () => Machine.HomePosition.ShouldEqual(ExpectedHomePosition);
        }
    }