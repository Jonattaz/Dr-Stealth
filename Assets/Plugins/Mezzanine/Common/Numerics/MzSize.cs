using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics 
{
    using NumericBaseType = System.Single;
    
    public struct MzSize : IMetric, ICloneable, IComparable, ISerializable, IComparable<MzSize>, IEquatable<MzSize> {
        public MzSize(params NumericBaseType[] values) {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }
        
        IMetric IMetric.Empty => Empty;
        public static MzSize Empty => new MzSize(new NumericBaseType[0]);
        
        //===== Common

        public MzSize Set(params NumericBaseType[] values) {
            Metrics.Set(Values, values);
            return this;
        }
        
        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty { get { return Metrics.IsEmpty(Values); } }
        public override string ToString() { return Metrics.ToString(Values); }
        public object Clone() { return new MzSize(Values); }
        public TObject To<TObject>() { return TypeConverter.To<TObject>(this); }

        public MzSize Normalize(bool isModifyOriginal = true) {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MzSize(values);
        }

        public MzSize Negate(bool isModifyOriginal = true) {
            var values = Metrics.MultiplyNumber(Values, -1, isModifyOriginal);
            return isModifyOriginal ? this : new MzSize(values);
        }

        //===== Value Accessors

        public NumericBaseType X {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set { if (Values.Length > 0) Values[0] = value; }
        }

        public NumericBaseType Y {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set { if (Values.Length > 1) Values[1] = value; }
        }

        public NumericBaseType Z {
            get => Values.Length > 2 ? Values[2] : Metrics.EmptyValue;
            set { if (Values.Length > 2) Values[2] = value; }
        }

        public NumericBaseType W {
            get => Values.Length > 3 ? Values[3] : Metrics.EmptyValue;
            set { if (Values.Length > 3) Values[3]= value; }
        }

        //===== From
        
        public static MzSize From(object value) {
            switch (value) {
                case double[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }
                
                case float[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }
                
                case decimal[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }
                
                case int[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }
                
                case long[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }
                
                case byte[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzSize(NumericBaseTypeArray);
                }

                default:
                    return new MzSize(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions
        
        //===== Operator overloading

        public static bool operator ==(MzSize a, MzSize b) { return Metrics.Equals(a.Values, b.Values); }
        public static bool operator ==(MzSize a, object b) { return Metrics.Equals(a.Values, b); }
        public static bool operator ==(object a, MzSize b) { return Metrics.Equals(b.Values, a); }

        public static bool operator !=(MzSize a, MzSize b) { return !Metrics.Equals(a.Values, b.Values); }
        public static bool operator !=(MzSize a, object b) { return !Metrics.Equals(a.Values, b); }
        public static bool operator !=(object a, MzSize b) { return !Metrics.Equals(b.Values, a); }
        
        public static MzSize operator *(MzSize a, MzSize b) { return new MzSize(Metrics.Multiply(a.Values, b.Values, false)); }
        public static MzSize operator *(MzSize a, object b) { return new MzSize(Metrics.Multiply(a.Values, b, false)); }
        public static MzSize operator *(object a, MzSize b) { return new MzSize(Metrics.Multiply(b.Values, a, false)); }
        
        public static MzSize operator /(MzSize a, MzSize b) { return new MzSize(Metrics.Divide(a.Values, b.Values, false)); }
        public static MzSize operator /(MzSize a, object b) { return new MzSize(Metrics.Divide(a.Values, b, false)); }
        public static MzSize operator /(object a, MzSize b) { return new MzSize(Metrics.Divide(b.Values, a, false)); }

        public static MzSize operator +(MzSize a, MzSize b) { return new MzSize(Metrics.Add(a.Values, b.Values, false)); }
        public static MzSize operator +(MzSize a, object b) { return new MzSize(Metrics.Add(a.Values, b, false)); }
        public static MzSize operator +(object a, MzSize b) { return new MzSize(Metrics.Add(b.Values, a, false)); }
        
        public static MzSize operator -(MzSize a, MzSize b) { return new MzSize(Metrics.Subtract(a.Values, b.Values, false)); }
        public static MzSize operator -(MzSize a, object b) { return new MzSize(Metrics.Subtract(a.Values, b, false)); }
        public static MzSize operator -(object a, MzSize b) { return new MzSize(Metrics.Subtract(b.Values, a, false)); }

        public static MzSize operator -(MzSize vector) {
            var vectorNew = (MzSize)vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }
        
        //===== IEquatable
        
        public override bool Equals(object value) {
            switch (value) {
                case MzSize vector: 
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }
        
        public bool Equals(MzSize value) { return Metrics.Equals(Values, value.Values); }
        public override int GetHashCode() { return base.GetHashCode(); }
        
        //===== IComparable
        
        // Compares this DateTime to a given object. This method provides an
        // implementation of the IComparable interface. The object
        // argument must be another DateTime, or otherwise an exception
        // occurs.  Null is considered less than any instance.
        //
        // Returns a value less than zero if this  object
        public int CompareTo(object value) {
            if (value == null) return 1;
            if (!(value is MzSize)) throw new ArgumentException("Object is not a MzSize");
            if (MagnitudeSquared > ((MzSize)value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MzSize)value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MzSize value) {
            if (MagnitudeSquared > value.MagnitudeSquared) return 1;
            if (MagnitudeSquared < value.MagnitudeSquared) return -1;
            return 0;
        }
        
        //===== ISerializable
        
        [System.Security.SecurityCritical /*auto-generated_required*/]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info==null) throw new ArgumentNullException("info");
            info.AddValue("Values", Values);
        }
    }
}