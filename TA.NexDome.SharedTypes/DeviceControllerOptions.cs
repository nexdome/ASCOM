using System;

namespace TA.NexDome.SharedTypes
    {
    /// <summary>
    /// Data transfer object for passing configuration to the device controller
    /// </summary>
    public class DeviceControllerOptions
        {
        public TimeSpan MaximumShutterCloseTime { get; set; }
        public TimeSpan MaximumFullRotationTime { get; set; }
        public TimeSpan ShutterTickTimeout { get; set; }
        public TimeSpan RotatorTickTimeout { get; set; }
        public decimal HomeAzimuth { get; set; }
        public decimal ParkAzimuth { get; set; }
        public int RotatorMaximumSpeed { get; set; }
        public int ShutterMaximumSpeed { get; set; }
        public TimeSpan RotatorRampTime { get; set; }
        public TimeSpan ShutterRampTime { get; set; }
        public TimeSpan WaitForShutterOnConnect { get; set; }
        }
    }