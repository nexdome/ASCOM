// This file is part of the TA.NexDome.AscomServer project
// Copyright © 2019-2019 Tigra Astronomy, all rights reserved.

namespace TA.NexDome.SharedTypes
    {
    using System;

    public static class MathExtensions
        {
        public static T Clip<T>(this T input, T minimum, T maximum)
            where T : struct, IComparable
            {
            if (input.CompareTo(maximum) > 0)
                return maximum;
            if (input.CompareTo(minimum) < 1)
                return minimum;
            return input;

            // if (input.CompareTo(minimum) < 0)
            // return minimum;
            }

        private const decimal MaxAdu = 1023.0M;
        private const decimal MaxVolts = 15.0M;
        public static uint VoltsToAdu(this decimal volts) => (uint)Math.Round(MaxAdu / MaxVolts * volts);

        public static decimal AduToVolts(this uint analogDigitalUnits) => analogDigitalUnits / MaxAdu * MaxVolts;
        }
    }