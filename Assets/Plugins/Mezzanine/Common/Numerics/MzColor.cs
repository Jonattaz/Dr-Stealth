using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public struct MzColor : IMetric, ICloneable, IComparable, ISerializable, IComparable<MzColor>, IEquatable<MzColor>
    {
        /// <summary>
        /// All values are represented as a normalized NumericBaseType (0.0 to  1.0).
        /// </summary>
        /// <param name="values"></param>
        public MzColor(params NumericBaseType[] values)
        {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }

        /// <summary>
        /// Byte values will be converted to normalized NumericBaseTypes (0.0 to 1.0).
        /// </summary>
        /// <param name="values"></param>
        public MzColor(params byte[] values)
        {
            var valuesDouble = new NumericBaseType[values.Length];
            for (var i = 0; i < values.Length; i++)
            {
                valuesDouble[i] = values[i] / (NumericBaseType)255;
            }

            _values = Metrics.InitializeValues(valuesDouble);
        }

        IMetric IMetric.Empty => Empty;
        public static MzColor Empty => new MzColor();

        //===== Common

        public MzColor Set(params NumericBaseType[] values)
        {
            Metrics.Set(Values, values);
            return this;
        }

        public MzColor Set(MzColor mzColor)
        {
            return Set(mzColor.Values);
        }

        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public bool IsEmpty => Metrics.IsEmpty(Values);

        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);

        public override string ToString()
        {
            return Metrics.ToString(Values);
        }

        public object Clone()
        {
            return new MzColor(Values);
        }

        public TObject To<TObject>()
        {
            return TypeConverter.To<TObject>(this);
        }

        public MzColor Normalize(bool isModifyOriginal = true)
        {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MzColor(values);
        }

        //===== Value Accessors

        public NumericBaseType R
        {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 0) Values[0] = value;
            }
        }

        public byte RByte => (byte) (R * 255);

        public NumericBaseType G
        {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 1) Values[1] = value;
            }
        }
        
        public byte GByte => (byte) (G * 255);

        public NumericBaseType B
        {
            get => Values.Length > 2 ? Values[2] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 2) Values[2] = value;
            }
        }
        
        public byte BByte => (byte) (B * 255);

        public NumericBaseType A
        {
            get => Values.Length > 3 ? Values[3] : Metrics.EmptyValue;
            set
            {
                if (Values.Length > 3) Values[3] = value;
            }
        }

        public NumericBaseType Alpha => A;
        
        public byte AByte => (byte) (A * 255);

        public byte[] GetBytes()
        {
            return new[] {RByte, GByte, BByte, AByte};
        }

        //===== From

        public static MzColor From(object value)
        {
            switch (value)
            {
                case UnityEngine.Color color:
                {
                    return new MzColor(color.r, color.g, color.b, color.a);
                }
                
                case UnityEngine.Color32 color: {
                    return new MzColor(color.r / 255f, color.g / 255f, color.b / 255f, color.a / 255f);
                }
                
                case double[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzColor(NumericBaseTypeArray);
                }

                case float[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzColor(NumericBaseTypeArray);
                }

                case decimal[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzColor(NumericBaseTypeArray);
                }

                case int[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzColor(NumericBaseTypeArray);
                }
                
                case long[] array:
                {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType) x);
                    return new MzColor(NumericBaseTypeArray);
                }

                case byte[] array: {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)(x / 255f));
                    return new MzColor(numericBaseTypeArray);
                }

                default:
                    return new MzColor(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions
        
        public static implicit operator UnityEngine.Color(MzColor value) {
            return new UnityEngine.Color(value.R, value.G, value.B, value.A);
        }

        public static implicit operator UnityEngine.Color32(MzColor value) {
            return new UnityEngine.Color32((byte)(value.R * 255f), (byte)(value.G * 255f), (byte)(value.B * 255f), (byte)(value.A * 255f));
        }

        //===== Operator overloading

        public static bool operator ==(MzColor a, MzColor b)
        {
            return Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator ==(MzColor a, object b)
        {
            return Metrics.Equals(a.Values, b);
        }

        public static bool operator ==(object a, MzColor b)
        {
            return Metrics.Equals(b.Values, a);
        }

        public static bool operator !=(MzColor a, MzColor b)
        {
            return !Metrics.Equals(a.Values, b.Values);
        }

        public static bool operator !=(MzColor a, object b)
        {
            return !Metrics.Equals(a.Values, b);
        }

        public static bool operator !=(object a, MzColor b)
        {
            return !Metrics.Equals(b.Values, a);
        }

        public static MzColor operator *(MzColor a, MzColor b)
        {
            return new MzColor(Metrics.Multiply(a.Values, b.Values, false));
        }

        public static MzColor operator *(MzColor a, object b)
        {
            return new MzColor(Metrics.Multiply(a.Values, b, false));
        }

        public static MzColor operator *(object a, MzColor b)
        {
            return new MzColor(Metrics.Multiply(b.Values, a, false));
        }

        public static MzColor operator /(MzColor a, MzColor b)
        {
            return new MzColor(Metrics.Divide(a.Values, b.Values, false));
        }

        public static MzColor operator /(MzColor a, object b)
        {
            return new MzColor(Metrics.Divide(a.Values, b, false));
        }

        public static MzColor operator /(object a, MzColor b)
        {
            return new MzColor(Metrics.Divide(b.Values, a, false));
        }

        public static MzColor operator +(MzColor a, MzColor b)
        {
            return new MzColor(Metrics.Add(a.Values, b.Values, false));
        }

        public static MzColor operator +(MzColor a, object b)
        {
            return new MzColor(Metrics.Add(a.Values, b, false));
        }

        public static MzColor operator +(object a, MzColor b)
        {
            return new MzColor(Metrics.Add(b.Values, a, false));
        }

        public static MzColor operator -(MzColor a, MzColor b)
        {
            return new MzColor(Metrics.Subtract(a.Values, b.Values, false));
        }

        public static MzColor operator -(MzColor a, object b)
        {
            return new MzColor(Metrics.Subtract(a.Values, b, false));
        }

        public static MzColor operator -(object a, MzColor b)
        {
            return new MzColor(Metrics.Subtract(b.Values, a, false));
        }

        public static MzColor operator -(MzColor vector)
        {
            var vectorNew = (MzColor) vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }

        //===== IEquatable

        public override bool Equals(object value)
        {
            switch (value)
            {
                case MzColor vector:
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                case byte[] bytes:
                    var values = new float[bytes.Length];
                    for (var i = 0; i < bytes.Length; i++)
                    {
                        values[i] = bytes[i] / 255f;
                    }

                    return Metrics.Equals(Values, values);
                default:
                    return false;
            }
        }

        public bool Equals(MzColor value)
        {
            return Metrics.Equals(Values, value.Values);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
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
            if (!(value is MzColor)) throw new ArgumentException("Object is not a MzColor");
            if (MagnitudeSquared > ((MzColor) value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MzColor) value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MzColor value)
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