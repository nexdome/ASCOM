// Copyright © Tigra Astronomy, all rights reserved.
using System;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.SharedTypes;
using TA.NexDome.Specifications.Builders;

namespace TA.NexDome.Specifications.Contexts
    {
    class StateMachineContext
        {
        internal IControllerActions Actions { get; set; }

        internal ControllerStateMachine Machine { get; set; }

        internal Exception Exception { get; }
        }
    }