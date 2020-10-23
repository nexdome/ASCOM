using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.NexDome.DeviceInterface
    {
    public static class ValueConverters
        {
        private const decimal MaxAdu = 1023.0M;
        private const decimal MaxVolts = 15.0M;
        public static uint VoltsToAdu(this decimal volts) => (uint)Math.Round(MaxAdu / MaxVolts * volts);
        public static decimal AduToVolts(this uint analogDigitalUnits) => analogDigitalUnits / MaxAdu * MaxVolts;
        }
    }
