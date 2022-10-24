using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics 
{
    using NumericBaseType = System.Single;
    
    public struct MinMax : IMetric, ICloneable, IComparable, ISerializable, IComparable<MinMax>, IEquatable<MinMax> {
        public MinMax(params NumericBaseType[] values) {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }
        
        IMetric IMetric.Empty => Empty;
        public static MinMax Empty => new MinMax(new NumericBaseType[0]);
        
        //===== Common

        public MinMax Set(params NumericBaseType[] values) {
            Metrics.Set(Values, values);
            return this;
        }
        
        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty { get { return Metrics.IsEmpty(Values); } }
        public override string ToString() { return Metrics.ToString(Values); }
        public object Clone() { return new MinMax(Values); }
        public TObject To<TObject>() { return TypeConverter.To<TObject>(this); }

        public MinMax Normalize(bool isModifyOriginal = true) {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MinMax(values);
        }

        public MinMax Negate(bool isModifyOriginal = true) {
            var values = Metrics.MultiplyNumber(Values, -1, isModifyOriginal);
            return isModifyOriginal ? this : new MinMax(values);
        }

        //===== Value Accessors

        public NumericBaseType Min {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set { if (Values.Length > 0) Values[0] = Clamp(value); }
        }

        public NumericBaseType Max {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set { if (Values.Length > 1) Values[1] = Clamp(value); }
        }

        //===== From
        
        public static MinMax From(object value) {
            switch (value) {
                case double[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }
                
                case float[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }
                
                case decimal[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }
                
                case int[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }
                
                case long[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }
                
                case byte[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MinMax(NumericBaseTypeArray);
                }

                default:
                    return new MinMax(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions
        
        //===== Operator overloading

        public static bool operator ==(MinMax a, MinMax b) { return Metrics.Equals(a.Values, b.Values); }
        public static bool operator ==(MinMax a, object b) { return Metrics.Equals(a.Values, b); }
        public static bool operator ==(object a, MinMax b) { return Metrics.Equals(b.Values, a); }

        public static bool operator !=(MinMax a, MinMax b) { return !Metrics.Equals(a.Values, b.Values); }
        public static bool operator !=(MinMax a, object b) { return !Metrics.Equals(a.Values, b); }
        public static bool operator !=(object a, MinMax b) { return !Metrics.Equals(b.Values, a); }
        
        public static MinMax operator *(MinMax a, MinMax b) { return new MinMax(Metrics.Multiply(a.Values, b.Values, false)); }
        public static MinMax operator *(MinMax a, object b) { return new MinMax(Metrics.Multiply(a.Values, b, false)); }
        public static MinMax operator *(object a, MinMax b) { return new MinMax(Metrics.Multiply(b.Values, a, false)); }
        
        public static MinMax operator /(MinMax a, MinMax b) { return new MinMax(Metrics.Divide(a.Values, b.Values, false)); }
        public static MinMax operator /(MinMax a, object b) { return new MinMax(Metrics.Divide(a.Values, b, false)); }
        public static MinMax operator /(object a, MinMax b) { return new MinMax(Metrics.Divide(b.Values, a, false)); }

        public static MinMax operator +(MinMax a, MinMax b) { return new MinMax(Metrics.Add(a.Values, b.Values, false)); }
        public static MinMax operator +(MinMax a, object b) { return new MinMax(Metrics.Add(a.Values, b, false)); }
        public static MinMax operator +(object a, MinMax b) { return new MinMax(Metrics.Add(b.Values, a, false)); }
        
        public static MinMax operator -(MinMax a, MinMax b) { return new MinMax(Metrics.Subtract(a.Values, b.Values, false)); }
        public static MinMax operator -(MinMax a, object b) { return new MinMax(Metrics.Subtract(a.Values, b, false)); }
        public static MinMax operator -(object a, MinMax b) { return new MinMax(Metrics.Subtract(b.Values, a, false)); }

        public static MinMax operator -(MinMax vector) {
            var vectorNew = (MinMax)vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }
        
        //===== IEquatable
        
        public override bool Equals(object value) {
            switch (value) {
                case MinMax vector: 
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }
        
        public bool Equals(MinMax value) { return Metrics.Equals(Values, value.Values); }
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
            if (!(value is MinMax)) throw new ArgumentException("Object is not a MinMax");
            if (MagnitudeSquared > ((MinMax)value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MinMax)value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MinMax value) {
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
        
        //===== Helpers
        
        public float RandomValue => UnityEngine.Random.Range(Min, Max);
        public float Clamp(float value) => Numbers.Clamp(value, Min, Max);
    }
}