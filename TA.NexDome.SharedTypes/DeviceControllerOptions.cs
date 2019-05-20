using System;

namespace TA.NexDome.SharedTypes
{
    public class DeviceControllerOptions
    {
        public bool PerformShutterRecovery { get; set; } = true;

        public TimeSpan MaximumShutterCloseTime { get; set; }

        public TimeSpan MaximumFullRotationTime { get; set; }

        public TimeSpan KeepAliveTimerInterval { get; set; }

        public int CurrentDrawDetectionThreshold { get; set; }

        public bool IgnoreHardwareShutterSensor { get; set; }

        public TimeSpan ShutterTickTimeout { get; set; }
    }
}