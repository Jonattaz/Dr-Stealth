using System;

namespace Mz.Numerics
{
    public static class RollingRangeExtensions
    {
        /// <summary>
        /// Returns the value when between 0 and high. Otherwise it will return the difference rolled over the other end of the range.
        /// </summary>
        public static int RollingRange(this int value, int high) => value.RollingRange(0, high);

        /// <summary>
        /// Returns the value when between low and high. Otherwise it will return the difference rolled over the other end of the range.
        /// </summary>
        public static int RollingRange(this int value, int low, int high)
        {
            if (low >= high) throw new Exception($"{nameof(RollingRange)}: {nameof(low)} must be less than {nameof(high)}");
            if (value >= low && value <= high) return value;
            var rangeLength = (high - low) + 1;
            var rolloverAdjustment = value > high ? rangeLength * -1 : rangeLength;
            return (value + rolloverAdjustment).RollingRange(low, high);
        }
    }
}