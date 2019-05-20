// This file is part of the TA.DigitalDomeworks project
// 
// Copyright © 2016-2018 Tigra Astronomy, all rights reserved.
// 
// File: with_device_builder_context.cs  Last modified: 2018-03-19@17:38 by Tim Long

using Machine.Specifications;
using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.Specifications.Builders;
using TA.NexDome.Specifications.Fakes;

namespace TA.NexDome.Specifications.Contexts
    {
    #region  Context base classes
    internal class with_device_controller_context
        {
        Establish context = () => DeviceControllerContextBuilder = new DeviceControllerContextBuilder();
        Cleanup after = () =>
            {
            DeviceControllerContextBuilder = null;
            Context = null;
            };
        protected static DeviceControllerContext Context;

        protected static DeviceControllerContextBuilder DeviceControllerContextBuilder;

        #region Convenience properties
        public static DeviceController Controller => Context.Controller;

        public static ICommunicationChannel Channel => Context.Channel;

        public static FakeCommunicationChannel FakeChannel => Context.Channel as FakeCommunicationChannel;
        #endregion Convenience properties
        }
    #endregion
    }