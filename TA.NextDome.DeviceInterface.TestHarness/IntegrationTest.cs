using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using Ninject;
using TA.NextDome.DeviceInterface.TestHarness;

namespace TA.NexDome.DeviceInterface.TestHarness
    {
    [Subject(typeof(DeviceController), "connection")]
    internal class when_connecting_to_the_hardware
        {
        private Cleanup after = () => Controller.Close();
        private Establish context = () =>
            {
            Controller = CompositionRoot.Kernel.Get<DeviceController>();
            };
        private Because of = () =>
            {
            Controller.Open();
            Controller.SlewToAzimuth(90.0);
            while (Controller.AzimuthDegrees < 90.0)
                {
                Task.Delay(1000).Wait();
                }
            };
        It should_be_at_90_degrees = () => Controller.AzimuthDegrees.ShouldEqual(90);
        private static DeviceController Controller;
        }
    }
