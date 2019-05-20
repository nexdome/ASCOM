using TA.Ascom.ReactiveCommunications;
using TA.NexDome.DeviceInterface;
using TA.NexDome.DeviceInterface.StateMachine;

namespace TA.NexDome.Specifications.Contexts {
    class DeviceControllerContext
        {
        public DeviceController Controller { get; set; }

        public ICommunicationChannel Channel { get; set; }

        public ControllerStateMachine StateMachine { get; set; }

        public RxControllerActions Actions { get; set; }
        }
    }