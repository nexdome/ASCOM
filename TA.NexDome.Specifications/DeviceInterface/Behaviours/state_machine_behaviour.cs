// This file is part of the TA.NexDome.AscomServer project
// Copyright © -2019 Tigra Astronomy, all rights reserved.

using System;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.Specifications.Contexts;

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours {
    internal class state_machine_behaviour
        {
        protected static StateMachineContext Context;

        protected static ControllerStateMachine Machine => Context.Machine;
        protected static IControllerActions Actions => Context.Actions;
        protected static Exception Exception => Context.Exception;
        }
    }