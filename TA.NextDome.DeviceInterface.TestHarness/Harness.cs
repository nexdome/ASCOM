using Ninject;
using TA.NextDome.DeviceInterface.TestHarness;

namespace TA.NexDome.DeviceInterface.TestHarness
    {
    public class Harness
        {
        public DeviceController BuildDeviceController()
            {
            CompositionRoot.BeginSessionScope();
            return CompositionRoot.Kernel.Get<DeviceController>();
            }
        }
    }
