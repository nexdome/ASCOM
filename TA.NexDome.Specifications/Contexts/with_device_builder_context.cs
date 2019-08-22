// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Contexts
    {
    using Machine.Specifications;

    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.DeviceInterface;
    using TA.NexDome.Specifications.Builders;
    using TA.NexDome.Specifications.Fakes;

    #region  Context base classes
    class with_device_controller_context
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