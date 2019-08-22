// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

#pragma warning disable 0649    // Context never assigned

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using JetBrains.Annotations;

    using TA.NexDome.DeviceInterface;
    using TA.NexDome.Specifications.Contexts;

    [UsedImplicitly]
    class device_controller_behaviour
        {
        protected static DeviceControllerContext Context;

        protected static DeviceController Controller => Context.Controller;
        }
    }