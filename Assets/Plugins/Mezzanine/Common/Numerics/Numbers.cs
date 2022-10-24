using System;
using System.Collections.Generic;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    using MathBase = System.Math;

    public static partial class Numbers
    {
        public static int GetIndexOfHighestValue(NumericBaseType[] values)
        {
            var index = 0;
            NumericBaseType value = -10000;

            for (var i = 0; i < values.Length; i++)
            {
                if (!(values[i] > value)) continue;
                value = values[i];
                index = i;
            }

            return index;
        }

        /// <summary>
        /// Return the item with the largest value in the list.
        /// </summary>
        public static T Max<T>(this IEnumerable<T> source, Func<T, NumericBaseType> selector)
        {
            if (ReferenceEquals(null, source)) throw new ArgumentNullException("source");
            if (ReferenceEquals(null, selector)) throw new ArgumentNullException("selector");

            var maxValueItem = default(T);
            NumericBaseType maxValue = 0;
            var isAssigned = false;

            foreach (var item in source)
            {
                var value = selector(item);

                if (!(value > maxValue) && isAssigned) continue;
                isAssigned = true;
                maxValue = value;
                maxValueItem = item;
            }

            return maxValueItem;
        }

        public static NumericBaseType Max(IEnumerable<NumericBaseType> source)
        {
            return source.Max((NumericBaseType item) => item);
        }

        /// <summary>
        /// Return the index of the largest value in the list.
        /// </summary>
        public static int ArgMax<T>(this IEnumerable<T> source, Func<T, NumericBaseType> selector)
        {
            if (ReferenceEquals(null, source)) throw new ArgumentNullException(nameof(source));
            if (ReferenceEquals(null, selector)) throw new ArgumentNullException(nameof(selector));

            var maxValueItemIndex = 0;
            NumericBaseType maxValue = 0;
            var isAssigned = false;
            var indexCurrent = 0;

            foreach (var item in source)
            {
                var value = selector(item);

                if ((value > maxValue) || (!isAssigned))
                {
                    isAssigned = true;
                    maxValue = value;
                    maxValueItemIndex = indexCurrent;
                }

                indexCurrent++;
            }

            return maxValueItemIndex;
        }

        public static int ArgMax(IEnumerable<NumericBaseType> source)
        {
            return source.ArgMax((NumericBaseType item) => item);
        }

        /**
		 * Wrapping these, so the implementation can be changed.
		 */

        // Returns the sine of angle /f/ in radians.
        public static NumericBaseType Sin(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Sin(f);
        }

        // Returns the hyperbolic sine of angle /f/ in radians.
        public static NumericBaseType Sinh(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Sinh(f);
        }

        // Returns the cosine of angle /f/ in radians.
        public static NumericBaseType Cos(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Cos(f);
        }

        // Returns the hyperbolic cosine of angle /f/ in radians.
        public static NumericBaseType Cosh(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Cosh(f);
        }

        // Returns the tangent of angle /f/ in radians.
        public static NumericBaseType Tan(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Tan(f);
        }

        // Returns the arc-sine of /f/ - the angle in radians whose sine is /f/.
        public static NumericBaseType Asin(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Asin(f);
        }

        // Returns the arc-cosine of /f/ - the angle in radians whose cosine is /f/.
        public static NumericBaseType Acos(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Acos(f);
        }

        // Returns the arc-tangent of /f/ - the angle in radians whose tangent is /f/.
        public static NumericBaseType Atan(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Atan(f);
        }

        // Returns the angle in radians whose ::ref::Tan is @@y/x@@.
        public static NumericBaseType Atan2(NumericBaseType y, NumericBaseType x)
        {
            return (NumericBaseType) MathBase.Atan2(y, x);
        }

        // Returns square root of /f/.
        public static NumericBaseType Sqrt(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Sqrt(f);
        }

        // Returns the absolute value of /f/.
        public static NumericBaseType Abs(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Abs(f);
        }

        // Returns the absolute value of /value/.
        public static int Abs(int value)
        {
            return MathBase.Abs(value);
        }

        /// *listonly*
        public static NumericBaseType Min(NumericBaseType a, NumericBaseType b)
        {
            return a < b ? a : b;
        }

        // Returns the smallest of two or more values.
        public static NumericBaseType Min(params NumericBaseType[] values)
        {
            var len = values.Length;
            if (len == 0) return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] < m) m = values[i];
            }

            return m;
        }

        /// *listonly*
        public static int Min(int a, int b)
        {
            return a < b ? a : b;
        }

        // Returns the smallest of two or more values.
        public static int Min(params int[] values)
        {
            var len = values.Length;
            if (len == 0) return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] < m) m = values[i];
            }

            return m;
        }

        /// *listonly*
        public static NumericBaseType Max(NumericBaseType a, NumericBaseType b)
        {
            return a > b ? a : b;
        }

        // Returns largest of two or more values.
        public static NumericBaseType Max(params NumericBaseType[] values)
        {
            var len = values.Length;
            if (len == 0) return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] > m) m = values[i];
            }

            return m;
        }

        /// *listonly*
        public static int Max(int a, int b)
        {
            return a > b ? a : b;
        }

        // Returns the largest of two or more values.
        public static int Max(params int[] values)
        {
            var len = values.Length;
            if (len == 0) return 0;
            var m = values[0];
            for (var i = 1; i < len; i++)
            {
                if (values[i] > m) m = values[i];
            }

            return m;
        }

        // Returns /f/ raised to power /p/.
        public static NumericBaseType Pow(NumericBaseType f, NumericBaseType p)
        {
            return (NumericBaseType) MathBase.Pow(f, p);
        }

        // Returns e raised to the specified power.
        public static NumericBaseType Exp(NumericBaseType power)
        {
            return (NumericBaseType) MathBase.Exp(power);
        }

        // Returns the logarithm of a specified number in a specified base.
        public static NumericBaseType Log(NumericBaseType f, NumericBaseType p)
        {
            return (NumericBaseType) MathBase.Log(f, p);
        }

        // Returns the natural (base e) logarithm of a specified number.
        public static NumericBaseType Log(NumericBaseType f)
        {
            return (NumericBaseType) MathBase.Log(f);
        }

        public static bool IsPowerOfTwo(int x) => (x & (x - 1)) == 0;

        // Returns the base 2 logarithm of a specified number.
        public static int Log2(int value)
        {
            if (value <= 0) throw new Exception("Number must be greater than or equal 0.");
            if (value == 1) return 0;
            if (!IsPowerOfTwo(value)) throw new Exception("Number must be power of 2.");

            var logarithm = 0;
            while (!Equals((NumericBaseType) MathBase.Pow(2, logarithm), value)) logarithm++;
            return logarithm;
        }

        // Returns the base 10 logarithm of a specified number.
        public static NumericBaseType Log10(NumericBaseType value)
        {
            return (NumericBaseType) MathBase.Log10(value);
        }

        // Returns the smallest integer greater to or equal to /value/.
        public static NumericBaseType Ceil(NumericBaseType value)
        {
            return (NumericBaseType) MathBase.Ceiling(value);
        }

        // Returns the largest integer smaller to or equal to /value/.
        public static NumericBaseType Floor(NumericBaseType value)
        {
            return (NumericBaseType) MathBase.Floor(value);
        }

        // Returns /f/ rounded to the nearest integer.
        public static NumericBaseType Round(NumericBaseType value)
        {
            return (NumericBaseType) MathBase.Round(value);
        }

        // Returns the smallest integer greater to or equal to /value/.
        public static int CeilToInt(NumericBaseType value)
        {
            return (int) MathBase.Ceiling(value);
        }

        // Returns the largest integer smaller to or equal to /value/.
        public static int FloorToInt(NumericBaseType value)
        {
            return (int) MathBase.Floor(value);
        }

        // Returns /value/ rounded to the nearest integer.
        public static int RoundToInt(NumericBaseType value)
        {
            return (int) MathBase.Round(value);
        }

        // Returns the sign of /value/.
        public static NumericBaseType Sign(NumericBaseType value)
        {
            return value >= 0 ? 1 : -1;
        }

        // The infamous ''3.14159265358979...'' value (RO).
        public const NumericBaseType Pi = (NumericBaseType) MathBase.PI;
        public const NumericBaseType TwoPi = (NumericBaseType) MathBase.PI * 2;
        public const NumericBaseType PiOver4 = (NumericBaseType) MathBase.PI / 4;
        public const NumericBaseType HalfPi = (NumericBaseType) (MathBase.PI / 2);

        public static NumericBaseType MinValue => NumericBaseType.MinValue;
        public static NumericBaseType MaxValue => NumericBaseType.MaxValue;

        // A representation of positive infinity (RO).
        public const NumericBaseType Infinity = NumericBaseType.PositiveInfinity;

        public static bool IsInfinity(NumericBaseType value)
        {
            return NumericBaseType.IsPositiveInfinity(value);
        }

        // A representation of negative infinity (RO).
        public const NumericBaseType NegativeInfinity = NumericBaseType.NegativeInfinity;

        public static bool IsNegativeInfinity(NumericBaseType value)
        {
            return NumericBaseType.IsNegativeInfinity(value);
        }

        // A tiny NumericBaseType point value (RO).
        public const NumericBaseType Epsilon = 1.401298E-45f;

        public static bool Equals(NumericBaseType lhs, NumericBaseType rhs,
            NumericBaseType epsilon = NumericBaseType.NegativeInfinity)
        {
            if (epsilon < Epsilon) epsilon = Epsilon;
            return Abs(lhs - rhs) <= epsilon;
        }

        // Clamps a value between a minimum NumericBaseType and maximum NumericBaseType value.
        public static NumericBaseType Clamp(NumericBaseType value, NumericBaseType min, NumericBaseType max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }

        // Clamps value between min and max and returns value.
        // Set the position of the transform to be that of the time
        // but never less than 1 or more than 3
        public static int Clamp(int value, int min, int max)
        {
            if (value < min) value = min;
            else if (value > max) value = max;
            return value;
        }

        // Clamps value between 0 and 1 and returns value
        public static NumericBaseType Clamp0To1(NumericBaseType value)
        {
            if (value < 0F) return 0F;
            return value > 1F ? 1F : value;
        }

        // Interpolates between /a/ and /b/ by /t/. /t/ is clamped between 0 and 1.
        public static NumericBaseType Lerp(NumericBaseType a, NumericBaseType b, NumericBaseType t)
        {
            return a + (b - a) * Clamp0To1(t);
        }

        // Interpolates between /a/ and /b/ by /t/ without clamping the interpolant.
        public static NumericBaseType LerpUnclamped(NumericBaseType a, NumericBaseType b, NumericBaseType t)
        {
            return a + (b - a) * t;
        }

        // Moves a value /current/ towards /target/.
        public static NumericBaseType MoveTowards(NumericBaseType current, NumericBaseType target,
            NumericBaseType maxDelta)
        {
            if (Abs(target - current) <= maxDelta) return target;
            return current + Sign(target - current) * maxDelta;
        }

        // Interpolates between /min/ and /max/ with smoothing at the limits.
        public static NumericBaseType SmoothStep(NumericBaseType from, NumericBaseType to, NumericBaseType t)
        {
            t = Clamp0To1(t);
            t = -2 * t * t * t + 3 * t * t;
            return to * t + from * (1 - t);
        }

        //*undocumented
        public static NumericBaseType Gamma(NumericBaseType value, NumericBaseType absmax, NumericBaseType gamma)
        {
            var isNegative = value < 0;
            var valueAbs = Abs(value);
            if (valueAbs > absmax) return isNegative ? -valueAbs : valueAbs;

            var result = Pow(valueAbs / absmax, gamma) * absmax;
            return isNegative ? -result : result;
        }

        // Compares two values to see if they are close.
        public static bool Approximately(NumericBaseType a, NumericBaseType b)
        {
            // If a or b is zero, compare that the other is less or equal to epsilon.
            // If neither a or b are 0, then find an epsilon that is good for
            // comparing numbers at the maximum magnitude of a and b.
            // Floating points have about 7 significant digits, so
            // 1.000001f can be represented while 1.0000001f is rounded to zero,
            // thus we could use an epsilon of 0.000001f for comparing values close to 1.
            // We multiply this epsilon by the biggest magnitude of a and b.
            return Abs(b - a) < Max(0.000001f * Max(Abs(a), Abs(b)), Epsilon * 8);
        }

        // Gradually changes a value towards a desired goal over time.
        public static NumericBaseType SmoothDamp(NumericBaseType current, NumericBaseType target,
            ref NumericBaseType currentVelocity, NumericBaseType smoothTime, NumericBaseType deltaTime,
            NumericBaseType maxSpeed = Numbers.Infinity)
        {
            // Based on Game Programming Gems 4 Chapter 1.10
            smoothTime = Numbers.Max(0.0001f, smoothTime);
            var omega = 2F / smoothTime;

            var x = omega * deltaTime;
            var exp = 1 / (1 + x + 0.48f * x * x + 0.235f * x * x * x);
            var change = current - target;
            var originalTo = target;

            // Clamp maximum speed
            var maxChange = maxSpeed * smoothTime;
            change = Numbers.Clamp(change, -maxChange, maxChange);
            target = current - change;

            var temp = (currentVelocity + omega * change) * deltaTime;
            currentVelocity = (currentVelocity - omega * temp) * exp;
            var output = target + (change + temp) * exp;

            // Prevent overshooting
            if (originalTo - current > 0.0f != output > originalTo) return output;
            output = originalTo;
            currentVelocity = (output - originalTo) / deltaTime;

            return output;
        }

        // Loops the value t, so that it is never larger than length and never smaller than 0.
        public static NumericBaseType Repeat(NumericBaseType t, NumericBaseType length)
        {
            return Clamp(t - Floor(t / length) * length, 0.0f, length);
        }

        // PingPongs the value t, so that it is never larger than length and never smaller than 0.
        public static NumericBaseType PingPong(NumericBaseType t, NumericBaseType length)
        {
            t = Repeat(t, length * 2f);
            return length - Abs(t - length);
        }

        // Calculates the ::ref::Lerp parameter between of two values.
        public static NumericBaseType InverseLerp(NumericBaseType a, NumericBaseType b, NumericBaseType value)
        {
            return a != b ? Clamp0To1((value - a) / (b - a)) : 0.0f;
        }
    }
}