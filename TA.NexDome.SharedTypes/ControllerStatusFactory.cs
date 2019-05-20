using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NLog;

namespace TA.NexDome.SharedTypes
{
    public sealed class ControllerStatusFactory
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private static readonly char[] fieldDelimiters = {','};
        private readonly IClock timeSource;

        public ControllerStatusFactory(IClock timeSource)
        {
            Contract.Requires(timeSource != null);
            this.timeSource = timeSource;
        }

        /// <summary>
        ///     Creates a <see cref="IHardwareStatus" /> object from the text of a status packet
        /// </summary>
        public IHardwareStatus FromStatusPacket(string packet)
        {
            Contract.Requires(!string.IsNullOrEmpty(packet));
            Contract.Ensures(Contract.Result<IHardwareStatus>() != null);
            var elements = packet.Split(fieldDelimiters);
            switch (elements[0])
            {
                case "V4":
                    return ParseV4StatusElements(elements);
                default:
                    throw new ApplicationException("Unsupported firmware version");
            }
        }

        private HardwareStatus ParseV4StatusElements(IReadOnlyList<string> elements)
        {
            log.Info("V4 status");
            var elementsLength = elements.Count;
            if (elementsLength != 23)
            {
                var message = $"V4 GINF packet had wrong number of elements: expected 23, found {elementsLength}";
                log.Error(message);
                var ex = new ArgumentException(message, "status");
                ex.Data["V4 Status Elements"] = elements;
                ex.Data["ExpectedElements"] = 23;
                ex.Data["ActualElements"] = elementsLength;
                throw ex;
            }

            try
            {
                var status = new HardwareStatus
                {
                    TimeStamp = timeSource.GetCurrentTime(),
                    FirmwareVersion = elements[0],
                    DomeCircumference = Convert.ToInt16(elements[1]),
                    HomePosition = Convert.ToInt16(elements[2]),
                    Coast = Convert.ToInt16(elements[3]),
                    CurrentAzimuth = Convert.ToInt16(elements[4]),
                    Slaved = elements[5] == "1" ? true : false,
                    ShutterSensor = (SensorState) Enum.Parse(typeof(SensorState), elements[6]),
                    DsrSensor = (SensorState) Enum.Parse(typeof(SensorState), elements[7]),
                    AtHome = elements[8] == "0" ? true : false,
                    HomeCounterClockwise = Convert.ToInt16(elements[9]),
                    HomeClockwise = Convert.ToInt16(elements[10]),
                    UserPins = Convert.ToByte(elements[11]),
                    WeatherAge = Convert.ToInt16(elements[12]),
                    WindDirection = Convert.ToInt16(elements[13]),
                    WindSpeed = Convert.ToInt16(elements[14]),
                    Temperature = Convert.ToInt16(elements[15]),
                    Humidity = Convert.ToInt16(elements[16]),
                    Wetness = Convert.ToInt16(elements[17]),
                    Snow = Convert.ToInt16(elements[18]),
                    WindPeak = Convert.ToInt16(elements[19]),
                    Lx200Azimuth = Convert.ToInt16(elements[20]),
                    DeadZone = Convert.ToInt16(elements[21]),
                    Offset = Convert.ToInt16(elements[22])
                };
                return status;
            }
            catch (Exception ex)
            {
                log.Error(ex, "Exception while parsing GINF packet");
                ex.Data["V4 Status Elements"] = elements;
                throw;
            }
        }
    }
}