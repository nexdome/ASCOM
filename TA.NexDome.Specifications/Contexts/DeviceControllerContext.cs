// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Contexts
    {
    using TA.Ascom.ReactiveCommunications;
    using TA.NexDome.DeviceInterface;
    using TA.NexDome.DeviceInterface.StateMachine;

    class DeviceControllerContext
        {
        public DeviceController Controller { get; set; }

        public ICommunicationChannel Channel { get; set; }

        public ControllerStateMachine StateMachine { get; set; }

        public RxControllerActions Actions { get; set; }
        }
    }