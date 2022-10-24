using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public struct Fraction : IMetric, ICloneable, IComparable, ISerializable, IComparable<Fraction>, IEquatable<Fraction> 
    {
        public Fraction(NumericBaseType numerator, NumericBaseType denominator)
            : this(new NumericBaseType[] { numerator, denominator })
        {}
        
        private Fraction(params NumericBaseType[] values) {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }
        
        IMetric IMetric.Empty => Empty;
        public static Fraction Empty => new Fraction(new NumericBaseType[0]);
        
        //===== Common

        public Fraction Set(params NumericBaseType[] values) {
            Metrics.Set(Values, values);
            return this;
        }
        
        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty { get { return Metrics.IsEmpty(Values); } }
        public override string ToString() { return Metrics.ToString(Values); }
        public object Clone() { return new Fraction(Values); }
        public TObject To<TObject>() { return TypeConverter.To<TObject>(this); }

        public Fraction Normalize(bool isModifyOriginal = true) {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new Fraction(values);
        }

        //===== Value Accessors

        public NumericBaseType Numerator {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set { if (Values.Length > 0) Values[0] = value; }
        }

        public NumericBaseType Denominator {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set { if (Values.Length > 1) Values[1] = value; }
        }

        //===== From
        
        public static Fraction From(object value) {
            switch (value) {
                case double[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }
                
                case float[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }
                
                case decimal[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }
                
                case int[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }
                
                case long[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }
                
                case byte[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, v => (NumericBaseType)v);
                    return new Fraction(NumericBaseTypeArray);
                }

                default:
                    return new Fraction(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions
        
        //===== Operator overloading

        public static bool operator ==(Fraction a, Fraction b) { return Metrics.Equals(a.Values, b.Values); }
        public static bool operator ==(Fraction a, object b) { return Metrics.Equals(a.Values, b); }
        public static bool operator ==(object a, Fraction b) { return Metrics.Equals(b.Values, a); }

        public static bool operator !=(Fraction a, Fraction b) { return !Metrics.Equals(a.Values, b.Values); }
        public static bool operator !=(Fraction a, object b) { return !Metrics.Equals(a.Values, b); }
        public static bool operator !=(object a, Fraction b) { return !Metrics.Equals(b.Values, a); }
        
        public static Fraction operator *(Fraction a, Fraction b) { return new Fraction(Metrics.Multiply(a.Values, b.Values, false)); }
        public static Fraction operator *(Fraction a, object b) { return new Fraction(Metrics.Multiply(a.Values, b, false)); }
        public static Fraction operator *(object a, Fraction b) { return new Fraction(Metrics.Multiply(b.Values, a, false)); }
        
        public static Fraction operator /(Fraction a, Fraction b) { return new Fraction(Metrics.Divide(a.Values, b.Values, false)); }
        public static Fraction operator /(Fraction a, object b) { return new Fraction(Metrics.Divide(a.Values, b, false)); }
        public static Fraction operator /(object a, Fraction b) { return new Fraction(Metrics.Divide(b.Values, a, false)); }

        public static Fraction operator +(Fraction a, Fraction b) { return new Fraction(Metrics.Add(a.Values, b.Values, false)); }
        public static Fraction operator +(Fraction a, object b) { return new Fraction(Metrics.Add(a.Values, b, false)); }
        public static Fraction operator +(object a, Fraction b) { return new Fraction(Metrics.Add(b.Values, a, false)); }
        
        public static Fraction operator -(Fraction a, Fraction b) { return new Fraction(Metrics.Subtract(a.Values, b.Values, false)); }
        public static Fraction operator -(Fraction a, object b) { return new Fraction(Metrics.Subtract(a.Values, b, false)); }
        public static Fraction operator -(object a, Fraction b) { return new Fraction(Metrics.Subtract(b.Values, a, false)); }

        public static Fraction operator -(Fraction vector) {
            var vectorNew = (Fraction)vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }
        
        //===== IEquatable
        
        public override bool Equals(object value) {
            switch (value) {
                case Fraction vector: 
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }
        
        public bool Equals(Fraction value) { return Metrics.Equals(Values, value.Values); }
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
            if (!(value is Fraction)) throw new ArgumentException("Object is not a Fraction");
            if (MagnitudeSquared > ((Fraction)value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((Fraction)value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(Fraction value) {
            if (MagnitudeSquared > value.MagnitudeSquared) return 1;
            if (MagnitudeSquared < value.MagnitudeSquared) return -1;
            return 0;
        }
        
        //===== ISerializable
        
        [System.Security.SecurityCritical /*auto-generated_required*/]
        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context) {
            if (info == null) throw new ArgumentNullException("info");
            info.AddValue("Values", Values);
        }
        
        //===== Methods

        public float ToFloat()
        {
            return Numerator / Denominator;
        }
    }
}