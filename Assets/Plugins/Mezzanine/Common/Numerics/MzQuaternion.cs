using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    /// <summary>
    /// A structure encapsulating a four-dimensional mzVector (x,y,z,w), 
    /// which is used to efficiently rotate an object about the (x,y,z) mzVector by the angle theta, where w = cos(theta/2).
    /// </summary>
    public struct MzQuaternion : IMetric, ICloneable, IComparable, ISerializable, IComparable<MzQuaternion>,
        IEquatable<MzQuaternion>
    {
        public MzQuaternion(params NumericBaseType[] values)
        {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }
        
        IMetric IMetric.Empty => Empty;
        public static MzQuaternion Empty => new MzQuaternion(new NumericBaseType[0]);

        //===== Common

        public MzQuaternion Set(params NumericBaseType[] values)
        {
            Metrics.Set(Values, values);
            return this;
        }

        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty => Metrics.IsEmpty(Values);

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

        public MzQuaternion Normalize(bool isModifyOriginal = true)
        {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MzQuaternion(values);
        }

        public MzQuaternion Negate(bool isModifyOriginal = true)
        {
            var values = Metrics.MultiplyNumber(Values, -1, isModifyOriginal);
            return isModifyOriginal ? this : new MzQuaternion(values);
        }

        //===== Type-specific

        /// <summary>
        ///   <para>Returns the euler angle representation of the rotation.</para>
        /// </summary>
        public MzVector EulerAngles
        {
            get => new MzVector(Metrics.MultiplyNumber(Quaternions.ToEulerRadians(Values), Numbers.Rad2Deg));
            set => this = new MzQuaternion(Quaternions.FromEulerRadians(Metrics.MultiplyNumber(Values, Numbers.Deg2Rad)));
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

        public static MzQuaternion From(object value)
        {
            switch (value)
            {
                case System.Numerics.Quaternion quaternion:
                {
                    return new MzQuaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);
                }

                case UnityEngine.Quaternion quaternion:
                {
                    return new MzQuaternion(quaternion.x, quaternion.y, quaternion.z, quaternion.w);
                }

                case double[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzQuaternion(NumericBaseTypeArray);
                }

                case float[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzQuaternion(NumericBaseTypeArray);
                }

                case decimal[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzQuaternion(NumericBaseTypeArray);
                }

                case int[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzQuaternion(NumericBaseTypeArray);
                }
                
                case long[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzQuaternion(NumericBaseTypeArray);
                }
                
                case byte[] array: {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzQuaternion(numericBaseTypeArray);
                }

                case MzVector vector:
                {
                    return new MzQuaternion(vector.Values);
                }

                default:
                    return new MzQuaternion(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions

        public static implicit operator System.Numerics.Quaternion(MzQuaternion value)
        {
            return value.To<System.Numerics.Quaternion>();
        }

        public static implicit operator UnityEngine.Quaternion(MzQuaternion value)
        {
            return value.To<UnityEngine.Quaternion>();
        }

        //===== Operator overloading

        public static bool operator ==(MzQuaternion a, MzQuaternion b)
        {
            return Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator ==(MzQuaternion a, object b)
        {
            return Metrics.Equals(a.Values, b);
        }

        public static bool operator ==(object a, MzQuaternion b)
        {
            return Metrics.Equals(b.Values, a);
        }

        public static bool operator !=(MzQuaternion a, MzQuaternion b)
        {
            return !Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator !=(MzQuaternion a, object b)
        {
            return !Metrics.Equals(a.Values, b);
        }

        public static bool operator !=(object a, MzQuaternion b)
        {
            return !Metrics.Equals(b.Values, a);
        }

        public static MzQuaternion operator *(MzQuaternion lhs, MzQuaternion rhs)
        {
            return new MzQuaternion(
                lhs.W * rhs.X + lhs.X * rhs.W + lhs.Y * rhs.Z - lhs.Z * rhs.Y,
                lhs.W * rhs.Y + lhs.Y * rhs.W + lhs.Z * rhs.X - lhs.X * rhs.Z,
                lhs.W * rhs.Z + lhs.Z * rhs.W + lhs.X * rhs.Y - lhs.Y * rhs.X,
                lhs.W * rhs.W - lhs.X * rhs.X - lhs.Y * rhs.Y - lhs.Z * rhs.Z
            );
        }

        public static MzVector operator *(MzQuaternion rotation, MzVector point)
        {
            return _Multiply(rotation, point);
        }

        public static MzVector operator *(MzVector point, MzQuaternion rotation)
        {
            return _Multiply(rotation, point);
        }

        private static MzVector _Multiply(MzQuaternion rotation, MzVector point)
        {
            var num = rotation.X * 2;
            var num2 = rotation.Y * 2;
            var num3 = rotation.Z * 2;
            var num4 = rotation.X * num;
            var num5 = rotation.Y * num2;
            var num6 = rotation.Z * num3;
            var num7 = rotation.X * num2;
            var num8 = rotation.X * num3;
            var num9 = rotation.Y * num3;
            var num10 = rotation.W * num;
            var num11 = rotation.W * num2;
            var num12 = rotation.W * num3;
            var result = Vectors.Identity;
            result.X = (1 - (num5 + num6)) * point.X + (num7 - num12) * point.Y + (num8 + num11) * point.Z;
            result.Y = (num7 + num12) * point.X + (1f - (num4 + num6)) * point.Y + (num9 - num10) * point.Z;
            result.Z = (num8 - num11) * point.X + (num9 + num10) * point.Y + (1f - (num4 + num5)) * point.Z;
            return result;
        }

        public static MzQuaternion operator /(MzQuaternion a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Divide(a.Values, b.Values, false));
        }

        public static MzQuaternion operator /(MzQuaternion a, object b)
        {
            return new MzQuaternion(Metrics.Divide(a.Values, b, false));
        }

        public static MzQuaternion operator /(object a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Divide(b.Values, a, false));
        }

        public static MzQuaternion operator +(MzQuaternion a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Add(a.Values, b.Values, false));
        }

        public static MzQuaternion operator +(MzQuaternion a, object b)
        {
            return new MzQuaternion(Metrics.Add(a.Values, b, false));
        }

        public static MzQuaternion operator +(object a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Add(b.Values, a, false));
        }

        public static MzQuaternion operator -(MzQuaternion a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Subtract(a.Values, b.Values, false));
        }

        public static MzQuaternion operator -(MzQuaternion a, object b)
        {
            return new MzQuaternion(Metrics.Subtract(a.Values, b, false));
        }

        public static MzQuaternion operator -(object a, MzQuaternion b)
        {
            return new MzQuaternion(Metrics.Subtract(b.Values, a, false));
        }

        public static MzQuaternion operator -(MzQuaternion vector)
        {
            var vectorNew = (MzQuaternion) vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }

        //===== IEquatable

        // Needed to override Equals.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object value)
        {
            switch (value)
            {
                case MzQuaternion quaternion:
                    return Metrics.Equals(Values, quaternion.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }

        // Needed to implement IEquatable
        public bool Equals(MzQuaternion value)
        {
            return Metrics.Equals(Values, value.Values);
        }
        
        //===== IComparable

        // Compares this DateTime to a given object. This method provides an
        // implementation of the IComparable interface. The object
        // argument must be another DateTime, or otherwise an exception
        // occurs.  Null is considered less than any instance.
        //
        // Returns a value less than zero if this  object
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is MzVector)) throw new ArgumentException("Object is not a MzVector");
            if (MagnitudeSquared > ((MzVector) value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MzVector) value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MzQuaternion value)
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