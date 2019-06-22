using System;

namespace TA.NexDome.SharedTypes
    {
    public class DeviceControllerOptions
        {
        public TimeSpan MaximumShutterCloseTime { get; set; }

        public TimeSpan MaximumFullRotationTime { get; set; }

        public TimeSpan ShutterTickTimeout { get; set; }

        public TimeSpan RotatorTickTimeout { get; set; }

        public decimal HomeSensorAzimuth { get; set; }
        }
    }