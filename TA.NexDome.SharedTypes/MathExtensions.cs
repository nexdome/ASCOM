using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TA.NexDome.SharedTypes
    {
    public static class MathExtensions
        {
        public static T Clip<T>(this T input, T minimum, T maximum) where T : struct, IComparable
            {
            if (input.CompareTo(maximum) > 0)
                return maximum;
            if (input.CompareTo(minimum) < 1)
                return minimum;
            return input;
            //if (input.CompareTo(minimum) < 0)
            //    return minimum;

            }
        }
    }
