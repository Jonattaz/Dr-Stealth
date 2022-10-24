using System;
using System.Linq;
using Mz.TypeTools;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public class Metrics
    {
        public static NumericBaseType EmptyValue => NumericBaseType.NegativeInfinity;
        public static NumericBaseType[] EmptyValues => new [] { EmptyValue, EmptyValue, EmptyValue, EmptyValue };
        public static bool IsEmptyValue(NumericBaseType value) => value == EmptyValue;
        
        /*====================================================================*
         START: Static Methods
         *====================================================================*/
        /**
         * NOTE:
         *
         * Setting the isModifyOriginal parameter to false on these static methods has a negative performance impact.
         */
        public static NumericBaseType[] InitializeValues(params NumericBaseType[] values)
        {
            if (values == null) return Enumerable.Repeat(EmptyValue, 4).ToArray();
            
            var length = values.Length;

            switch (length)
            {
                case 1:
                {
                    length = (int) values[0];
                    if (!length.IsNumber() || length == 0) length = 4;
                    values = Enumerable.Repeat(EmptyValue, length).ToArray();
                    break;
                }

                case 0:
                {
                    length = 4;
                    values = Enumerable.Repeat(EmptyValue, length).ToArray();
                    break;
                }

                default:
                {
                    if (length > 4) length = 4;
                    break;
                }
            }

            var valuesNew = new NumericBaseType[length];
            
            for (var i = 0; i < length; i++)
            {
                valuesNew[i] = values[i];
            }

            return valuesNew;
        }

        public static NumericBaseType[] InitializeValues(params object[] values) => InitializeValues(values.Select(v => v).Cast<NumericBaseType>().ToArray());

        public static NumericBaseType[] ParseValues(object values)
        {
            NumericBaseType[] valuesNew = null;
            
            if (values is NumericBaseType[] numericBaseTypes) valuesNew = numericBaseTypes;
            else if (values is float[] floatTypes)
            {
                valuesNew = floatTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is double[] doubleTypes)
            {
                valuesNew = doubleTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is decimal[] decimalTypes)
            {
                valuesNew = decimalTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is byte[] byteTypes)
            {
                valuesNew = byteTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is short[] shortTypes)
            {
                valuesNew = shortTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is int[] intTypes)
            {
                valuesNew = intTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            else if (values is long[] longTypes)
            {
                valuesNew = longTypes.Select(v => (NumericBaseType) v).ToArray();
            }
            
            if (values is IMetric metricOther) valuesNew = metricOther.Values;
            return valuesNew;
        }

        /// <summary>
        /// Update the values of the given instance.
        /// </summary>
        public static NumericBaseType[] Set(NumericBaseType[] values, NumericBaseType[] valuesOther)
        {
            if (values == null) throw new Exception("values is null in Metrics.Set!!!!!!!!!!!!!!!!!!!!");
            if (valuesOther == null) throw new Exception("valuesOther is null in Metrics.Set!!!!!!!!!!!!!!!!!!!!");
            
            if (valuesOther == null) return values ?? new NumericBaseType[0];
            if (values == null) values = new NumericBaseType[valuesOther.Length];
            
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            for (var i = 0; i < length; i++)
            {
                values[i] = valuesOther[i];
            }

            if (values == null) throw new Exception("the returning values is null in Metrics.Set!!!!!!!!!!!!!!!!!!!!");
            return values;
        }

        public static bool IsEmpty(NumericBaseType[] values)
        {
            var isEmpty = values == null || values.Length == 0;
            if (isEmpty) return isEmpty;
            if (values.Any(IsEmptyValue)) isEmpty = true;
            return isEmpty;
        }

        public static bool IsEmpty(IMetric value)
        {
            return value == null || IsEmpty(value.Values);
        }

        public static string ToString(NumericBaseType[] values)
        {
            if (values == null) return "[]";

            var text = "[";

            for (var i = 0; i < values.Length; i++)
            {
                text += values[i].ToString();
                if (i < values.Length - 1) text += ", ";
            }

            text += "]";

            return text;
        }

        /// <summary>
        /// Check if two number arrays are equal, within a tolerance.
        /// </summary>
        public static bool Equals(NumericBaseType[] values, NumericBaseType[] valuesOther)
        {
            for (int i = 0, len = values.Length; i < len; i++)
            {
                if (Numbers.Abs(values[i] - valuesOther[i]) > Numbers.Epsilon) return false;
            }

            return true;
        }

        public static NumericBaseType MagnitudeSquared(NumericBaseType[] values)
        {
            return Dot(values, values);
        }

        public static NumericBaseType Magnitude(NumericBaseType[] values)
        {
            return Numbers.Sqrt(Dot(values, values));
        }

        /// <summary>
        /// Constrain to a magnitude of 1.
        /// </summary>
        public static NumericBaseType[] Normalize(NumericBaseType[] values, bool isModifyOriginal = false)
        {
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[values.Length];
            var magnitude = Magnitude(valuesOut);
            valuesOut = magnitude > Numbers.Epsilon
                ? DivideNumber(valuesOut, magnitude, isModifyOriginal)
                : ZeroOut(valuesOut);
            return valuesOut;
        }

        /// <summary>
        /// Set all elements to 0.
        /// </summary>
        public static NumericBaseType[] ZeroOut(NumericBaseType[] values)
        {
            for (var i = 0; i < values.Length; i++)
            {
                values[i] = 0;
            }

            return values;
        }

        public static NumericBaseType[] Abs(NumericBaseType[] values, bool isModifyOriginal = false)
        {
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[values.Length];
            var length = values.Length;
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Abs(values[i]);
            }

            return valuesOut;
        }

        /// <summary>
        /// Add `valuesOther` to `values`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Add(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = (values[i] + valuesOther[i]);
            }

            return valuesOut;
        }

        public static NumericBaseType[] Add(NumericBaseType[] values, object other, bool isModifyOriginal = true)
        {
            if (other.IsNumber())
            {
                return AddNumber(values, (NumericBaseType) other, isModifyOriginal);
            }

            var valuesOther = ParseValues(other);
            return Add(values, valuesOther, isModifyOriginal);
        }

        /// <summary>
        /// Add two sets of values.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new Array?</param>
        /// <returns>A new number array with the result or, if `isModifyOriginal` is true, a modified version of `values`.</returns>
        public static NumericBaseType[] AddNumber(NumericBaseType[] values, NumericBaseType value, bool isModifyOriginal = true)
        {
            var length = values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = (values[i] + value);
            }

            return valuesOut;
        }

        /// <summary>
        /// Subtract `valuesOther` from `values`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Subtract(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = (values[i] - valuesOther[i]);
            }

            return valuesOut;
        }

        public static NumericBaseType[] Subtract(NumericBaseType[] values, object other, bool isModifyOriginal = true)
        {
            if (other.IsNumber())
            {
                return SubtractNumber(values, (NumericBaseType) other, isModifyOriginal);
            }

            var valuesOther = ParseValues(other);
            return Subtract(values, valuesOther, isModifyOriginal);
        }

        /// <summary>
        /// Get the absolute difference between `valuesOther` and `values`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Difference(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Abs(values[i] - valuesOther[i]);
            }

            return valuesOut;
        }

        /// <summary>
        /// Subtract a numerical value from `values`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] SubtractNumber(NumericBaseType[] values, NumericBaseType value, bool isModifyOriginal = true)
        {
            var length = values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = (values[i] - value);
            }

            return valuesOut;
        }

        /// <summary>
        /// Multiply `valuesOther` with `values`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Multiply(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var c = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                c[i] = (values[i] * valuesOther[i]);
            }

            return c;
        }

        public static NumericBaseType[] Multiply(NumericBaseType[] values, object other, bool isModifyOriginal = true)
        {
            if (other.IsNumber())
            {
                return MultiplyNumber(values, (NumericBaseType) other, isModifyOriginal);
            }

            var valuesOther = ParseValues(other);
            return Multiply(values, valuesOther, isModifyOriginal);
        }

        /// <summary>
        /// Multiply a set of values by a number.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] MultiplyNumber(NumericBaseType[] values, NumericBaseType value, bool isModifyOriginal = true)
        {
            var length = values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = (values[i] * value);
            }

            return valuesOut;
        }

        /// <summary>
        /// Divide `values` by `valuesOther`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Divide(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                if (Numbers.Abs(valuesOther[i]) > Numbers.Epsilon && Numbers.Abs(valuesOther[i]) - 1 > Numbers.Epsilon)
                {
                    valuesOut[i] = (values[i] / valuesOther[i]);
                }
            }

            return valuesOut;
        }

        public static NumericBaseType[] Divide(NumericBaseType[] values, object other, bool isModifyOriginal = true)
        {
            if (other.IsNumber())
            {
                return DivideNumber(values, (NumericBaseType) other, isModifyOriginal);
            }

            var valuesOther = ParseValues(other);
            return Divide(values, valuesOther, isModifyOriginal);
        }

        /// <summary>
        /// Divide `values` by the given value.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] DivideNumber(NumericBaseType[] values, NumericBaseType value, bool isModifyOriginal = true)
        {
            var length = values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                if (Numbers.Abs(value) > Numbers.Epsilon && Numbers.Abs(value) - 1 > Numbers.Epsilon)
                    valuesOut[i] = (values[i] / value);
            }

            return valuesOut;
        }

        /// <summary>
        /// Get the scalar product of two arrays.
        /// </summary>
        public static NumericBaseType Dot(NumericBaseType[] values, NumericBaseType[] valuesOther)
        {
            NumericBaseType dotValue = 0;
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            for (var i = 0; i < length; i++) dotValue += values[i] * valuesOther[i];
            return dotValue;
        }

        /// <summary>
        /// Get the squared distance between `values` and `valuesOther`.
        /// </summary>
        public static NumericBaseType DistanceSquared(NumericBaseType[] values, NumericBaseType[] valuesOther)
        {
            var difference = Subtract(values, valuesOther);
            return MagnitudeSquared(difference);
        }

        /// <summary>
        /// The distance between two points.
        /// </summary>
        public static NumericBaseType Distance(NumericBaseType[] values, NumericBaseType[] valuesOther)
        {
            return Numbers.Sqrt(DistanceSquared(values, valuesOther));
        }

        /// <summary>
        /// Get a new NumericBaseType[] that has the minimum dimensional values of `values` and `valuesOther`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Min(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Min(values[i], valuesOther[i]);
            }

            return valuesOut;
        }

        /// <summary>
        /// Get a new NumericBaseType[] that has the maximum dimensional values of `values` and `valuesOther`.
        /// </summary>
        /// <param name="isModifyOriginal">Modify `values`, or return a new NumericBaseType[]?</param>
        public static NumericBaseType[] Max(NumericBaseType[] values, NumericBaseType[] valuesOther, bool isModifyOriginal = true)
        {
            var length = valuesOther.Length > values.Length ? values.Length : valuesOther.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Max(values[i], valuesOther[i]);
            }

            return valuesOut;
        }

        public static NumericBaseType[] Clamp(NumericBaseType[] values, NumericBaseType[] min, NumericBaseType[] max, bool isModifyOriginal = true)
        {
            var length = values.Length > max.Length ? max.Length : values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Min(values[i], max[i]);
            }

            length = valuesOut.Length > min.Length ? min.Length : valuesOut.Length;
            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Max(valuesOut[i], min[i]);
            }

            return valuesOut;
        }

        public static NumericBaseType[] Clamp(NumericBaseType[] values, NumericBaseType min, NumericBaseType max, bool isModifyOriginal = true)
        {
            var length = values.Length;
            var valuesOut = isModifyOriginal ? values : new NumericBaseType[length];

            for (var i = 0; i < length; i++)
            {
                valuesOut[i] = Numbers.Min(values[i], max);
                valuesOut[i] = Numbers.Max(valuesOut[i], min);
            }

            return valuesOut;
        }

        public static NumericBaseType LargestValue(NumericBaseType[] values)
        {
            if (values.Length < 1) return 0;

            var length = values.Length;
            var value = values[0];
            for (var i = 1; i < length; i++)
            {
                value = Numbers.Max(value, values[i]);
            }

            return value;
        }

        public static NumericBaseType SmallestValue(NumericBaseType[] values)
        {
            if (values.Length < 1) return 0;

            var length = values.Length;
            var value = values[0];
            for (var i = 1; i < length; i++)
            {
                value = Numbers.Min(value, values[i]);
            }

            return value;
        }
    }
}