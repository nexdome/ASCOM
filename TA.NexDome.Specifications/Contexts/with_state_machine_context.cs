// Copyright © Tigra Astronomy, all rights reserved.
using Machine.Specifications;
using TA.NexDome.DeviceInterface.StateMachine;
using TA.NexDome.Specifications.Builders;

namespace TA.NexDome.Specifications.Contexts
    {
    /// <summary>
    /// A test context for testing Device Controller state machine operation. Includes a context
    /// class to hold all the test data and unit under test, plus a builder to create the context.
    /// </summary>
    internal class with_state_machine_context
        {
        protected static StateMachineContext Context;

        protected static StateMachineBuilder ContextBuilder { get; set; }
        protected static RotatorStatusBuilder RotatorStatus;
        protected static ShutterStatusBuilder ShutterStatus;

        Cleanup after = () =>
            {
                Context = null;
                ContextBuilder = null;
            };

        Establish context = () =>
        {
            ContextBuilder = new StateMachineBuilder();
            RotatorStatus = new RotatorStatusBuilder();
            ShutterStatus = new ShutterStatusBuilder();
        };

        #region Convenience Properties
        protected static IControllerActions Actions => Context.Actions;

        protected static ControllerStateMachine Machine => Context.Machine;

        #endregion Convenience Properties
        }
    }