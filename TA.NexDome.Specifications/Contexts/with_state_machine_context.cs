// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.Specifications.Contexts
    {
    using Machine.Specifications;

    using TA.NexDome.DeviceInterface.StateMachine;
    using TA.NexDome.Specifications.Builders;

    #region  Context base classes

    /// <summary>
    ///     A test context for testing Device Controller state machine operation. Includes a context
    ///     class to hold all the test data and unit under test, plus a builder to create the
    ///     context.
    /// </summary>
    class with_state_machine_context
        {
        Establish context = () =>
            {
            ContextBuilder = new StateMachineBuilder();
            RotatorStatus = new RotatorStatusBuilder();
            ShutterStatus = new ShutterStatusBuilder();
            };

        Cleanup after = () =>
            {
            Context = null;
            ContextBuilder = null;
            };

        protected static StateMachineContext Context;

        protected static RotatorStatusBuilder RotatorStatus;

        protected static ShutterStatusBuilder ShutterStatus;

        protected static StateMachineBuilder ContextBuilder { get; set; }

        #region Convenience Properties
        protected static IControllerActions Actions => Context.Actions;

        protected static ControllerStateMachine Machine => Context.Machine;
        #endregion Convenience Properties
        }
    #endregion
    }