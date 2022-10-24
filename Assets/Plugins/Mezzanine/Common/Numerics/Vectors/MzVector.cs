using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;

    public struct MzVector : IMetric, ICloneable, IComparable, ISerializable, IComparable<MzVector>,
        IEquatable<MzVector>
    {
        public MzVector(params NumericBaseType[] values)
        {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }

        IMetric IMetric.Empty => Empty;
        public static MzVector Empty => new MzVector(new NumericBaseType[0]);

        //===== Common

        public MzVector Set(params NumericBaseType[] values)
        {
            Metrics.Set(Values, values);
            return this;
        }

        public MzVector Set(MzVector value)
        {
            return Set(value.Values);
        }

        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;

        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty => Metrics.IsEmpty(Values);
        public MzVector Normalized => Normalize(false);
        public MzVector Negated => Negate(false);

        public override string ToString()
        {
            return Metrics.ToString(Values);
        }

        public object Clone()
        {
            return new MzVector(Values);
        }

        public TObject To<TObject>()
        {
            return TypeConverter.To<TObject>(this);
        }

        /// <summary>
        /// Calculate and return the direction unit vector from this point to another given point.
        /// </summary>
        /// <returns>The normalized direction unit vector between point a and point b.</returns>
        public MzVector Direction(MzVector other)
        {
            return Vectors.Direction(this, other);
        }

        public NumericBaseType Pitch => Vectors.Pitch(this);
        public NumericBaseType Yaw => Vectors.Yaw(this);

        public MzVector Normalize(bool isModifyOriginal = true)
        {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MzVector(values);
        }

        public MzVector Negate(bool isModifyOriginal = true)
        {
            var values = Metrics.MultiplyNumber(Values, -1, isModifyOriginal);
            return isModifyOriginal ? this : new MzVector(values);
        }

        //===== Value Accessors

        public NumericBaseType X
        {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 0) Values[0] = value;
            }
        }

        public NumericBaseType Y
        {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 1) Values[1] = value;
            }
        }

        public NumericBaseType Z
        {
            get => Values.Length > 2 ? Values[2] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 2) Values[2] = value;
            }
        }

        public NumericBaseType W
        {
            get => Values.Length > 3 ? Values[3] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 3) Values[3] = value;
            }
        }

        //===== From

        public static MzVector From(object value)
        {
            switch (value)
            {
                case System.Numerics.Vector2 vector2:
                {
                    return new MzVector(vector2.X, vector2.Y);
                }

                case System.Numerics.Vector3 vector3:
                {
                    return new MzVector(vector3.X, vector3.Y, vector3.Z);
                }

                case System.Numerics.Vector4 vector4:
                {
                    return new MzVector(vector4.X, vector4.Y, vector4.Z, vector4.W);
                }

                case UnityEngine.Vector2 vector2:
                {
                    return new MzVector(vector2.x, vector2.y);
                }

                case UnityEngine.Vector3 vector3:
                {
                    return new MzVector(vector3.x, vector3.y, vector3.z);
                }

                case UnityEngine.Vector4 vector4:
                {
                    return new MzVector(vector4.x, vector4.y, vector4.z, vector4.w);
                }

                case double[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                case float[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                case decimal[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                case int[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                case long[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                case byte[] array:
                {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzVector(numericBaseTypeArray);
                }

                default:
                    return new MzVector(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions

        public static implicit operator System.Numerics.Vector2(MzVector value)
        {
            return value.To<System.Numerics.Vector2>();
        }

        public static implicit operator System.Numerics.Vector3(MzVector value)
        {
            return value.To<System.Numerics.Vector3>();
        }

        public static implicit operator System.Numerics.Vector4(MzVector value)
        {
            return value.To<System.Numerics.Vector4>();
        }

        public static implicit operator UnityEngine.Vector2(MzVector value)
        {
            return value.To<UnityEngine.Vector2>();
        }

        public static implicit operator UnityEngine.Vector3(MzVector value)
        {
            return value.To<UnityEngine.Vector3>();
        }

        public static implicit operator UnityEngine.Vector4(MzVector value)
        {
            return value.To<UnityEngine.Vector4>();
        }

        //===== Operator overloading

        public static bool operator ==(MzVector a, MzVector b)
        {
            return Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator ==(MzVector a, object b)
        {
            return Metrics.Equals(a.Values, Metrics.ParseValues(b));
        }

        public static bool operator ==(object a, MzVector b)
        {
            return Metrics.Equals(b.Values, Metrics.ParseValues(a));
        }

        public static bool operator !=(MzVector a, MzVector b)
        {
            return !Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator !=(MzVector a, object b)
        {
            return !Metrics.Equals(a.Values, Metrics.ParseValues(b));
        }

        public static bool operator !=(object a, MzVector b)
        {
            return !Metrics.Equals(b.Values, Metrics.ParseValues(a));
        }

        public static MzVector operator *(MzVector a, MzVector b)
        {
            return new MzVector(Metrics.Multiply(a.Values, b.Values, false));
        }

        public static MzVector operator *(MzVector a, object b)
        {
            return new MzVector(Metrics.Multiply(a.Values, b, false));
        }

        public static MzVector operator *(object a, MzVector b)
        {
            return new MzVector(Metrics.Multiply(b.Values, a, false));
        }

        public static MzVector operator /(MzVector a, MzVector b)
        {
            return new MzVector(Metrics.Divide(a.Values, b.Values, false));
        }

        public static MzVector operator /(MzVector a, object b)
        {
            return new MzVector(Metrics.Divide(a.Values, b, false));
        }

        public static MzVector operator /(object a, MzVector b)
        {
            return new MzVector(Metrics.Divide(b.Values, a, false));
        }

        public static MzVector operator +(MzVector a, MzVector b)
        {
            return new MzVector(Metrics.Add(a.Values, b.Values, false));
        }

        public static MzVector operator +(MzVector a, object b)
        {
            return new MzVector(Metrics.Add(a.Values, b, false));
        }

        public static MzVector operator +(object a, MzVector b)
        {
            return new MzVector(Metrics.Add(b.Values, a, false));
        }

        public static MzVector operator -(MzVector a, MzVector b)
        {
            return new MzVector(Metrics.Subtract(a.Values, b.Values, false));
        }

        public static MzVector operator -(MzVector a, object b)
        {
            return new MzVector(Metrics.Subtract(a.Values, b, false));
        }

        public static MzVector operator -(object a, MzVector b)
        {
            return new MzVector(Metrics.Subtract(b.Values, a, false));
        }

        public static MzVector operator -(MzVector mzVector)
        {
            var vectorNew = (MzVector) mzVector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }

        //===== Transforms
        // public void TransformP(MzMatrix matrix)
        // {
        //     float[] matrixArray = matrix.Matrix;
        //     var x = X;
        //     var y = Y;
        //     X = x * matrixArray[0] + y * matrixArray[2];
        //     Y = x * matrixArray[1] + y * matrixArray[3];
        // }

        //===== IEquatable

        public override bool Equals(object value)
        {
            switch (value)
            {
                case MzVector vector:
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }

        // Needed to implement IEquatable
        public bool Equals(MzVector value)
        {
            return Metrics.Equals(Values, value.Values);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        public bool EqualsApproximate(MzVector other, float epsilon = -1) 
        {
            var isApproximatelyEqual = true;
            if (epsilon < 0) epsilon = Numbers.Epsilon;
            if (Numbers.Abs(X - other.X) > epsilon) isApproximatelyEqual = false;
            else if (Numbers.Abs(Y - other.Y) > epsilon) isApproximatelyEqual = false;
            else if (Numbers.Abs(Z - other.Z) > epsilon) isApproximatelyEqual = false;
            return isApproximatelyEqual;
        }

        //===== IComparable
        
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is MzVector)) throw new ArgumentException("Object is not a MzVector");
            if (MagnitudeSquared > ((MzVector) value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MzVector) value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MzVector value)
        {
            if (MagnitudeSquared > value.MagnitudeSquared) return 1;
            if (MagnitudeSquared < value.MagnitudeSquared) return -1;
            return 0;
        }

        //===== ISerializable

        [System.Security.SecurityCritical /*auto-generated_required*/]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null) throw new ArgumentNullException("info");
            info.AddValue("Values", Values);
        }
    }
}