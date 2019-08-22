// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.DeviceInterface.Behaviours
    {
    using System;

    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.Specifications.Contexts;

    class state_machine_behaviour
        {
        protected static StateMachineContext Context;

        protected static ControllerStateMachine Machine => Context.Machine;

        protected static IControllerActions Actions => Context.Actions;

        protected static Exception Exception => Context.Exception;
        }
    }