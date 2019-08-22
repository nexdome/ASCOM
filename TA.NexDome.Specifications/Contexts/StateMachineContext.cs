// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Contexts
    {
    using System;

    using TA.NexDome.DeviceInterface.StateMachine;

    class StateMachineContext
        {
        internal IControllerActions Actions { get; set; }

        internal ControllerStateMachine Machine { get; set; }

        internal Exception Exception { get; }
        }
    }