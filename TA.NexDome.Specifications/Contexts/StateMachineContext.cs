using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TA.NexDome.DeviceInterface.StateMachine;

namespace TA.NexDome.Specifications.Contexts
    {
    class StateMachineContext
        {
        internal IControllerActions Actions { get; set; }

        internal ControllerStateMachine Machine { get; set; }

        internal Exception Exception { get; }
        }
    }
