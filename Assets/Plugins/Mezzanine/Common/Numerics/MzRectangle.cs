using System;
using System.Runtime.Serialization;
using Mz.TypeTools;

namespace Mz.Numerics 
{
    using NumericBaseType = System.Single;
    
    public struct MzRectangle : IMetric, ICloneable, IComparable, ISerializable, IComparable<MzRectangle>, IEquatable<MzRectangle> {
        public MzRectangle(params NumericBaseType[] values) {
            _values = Metrics.InitializeValues(values) ?? Metrics.EmptyValues;
        }
        
        IMetric IMetric.Empty => Empty;
        public static MzRectangle Empty => new MzRectangle(new NumericBaseType[0]);
        
        //===== Common

        public MzRectangle Set(params NumericBaseType[] values) {
            Metrics.Set(Values, values);
            return this;
        }
        
        private readonly NumericBaseType[] _values;
        public NumericBaseType[] Values => _values ?? Metrics.EmptyValues;
        
        public NumericBaseType Magnitude => Metrics.Magnitude(Values);
        public NumericBaseType MagnitudeSquared => Metrics.MagnitudeSquared(Values);
        public bool IsEmpty { get { return Metrics.IsEmpty(Values); } }
        public override string ToString() { return Metrics.ToString(Values); }
        public object Clone() { return new MzRectangle(Values); }
        public TObject To<TObject>() { return TypeConverter.To<TObject>(this); }

        public MzRectangle Normalize(bool isModifyOriginal = true) {
            var values = Metrics.Normalize(Values, isModifyOriginal);
            return isModifyOriginal ? this : new MzRectangle(values);
        }

        public MzRectangle Negate(bool isModifyOriginal = true) {
            var values = Metrics.MultiplyNumber(Values, -1, isModifyOriginal);
            return isModifyOriginal ? this : new MzRectangle(values);
        }

        //===== Value Accessors

        public NumericBaseType X {
            get => Values.Length > 0 ? Values[0] : Metrics.EmptyValue;
            set { if (Values.Length > 0) Values[0] = value; }
        }

        public NumericBaseType Left => X;
        public int XInt => (int) X;

        public NumericBaseType Y {
            get => Values.Length > 1 ? Values[1] : Metrics.EmptyValue;
            set { if (Values.Length > 1) Values[1] = value; }
        }

        public NumericBaseType Top => Y;
        public int YInt => (int) Y;

        public NumericBaseType Width {
            get => Values.Length > 2 ? Values[2] : Metrics.EmptyValue;
            set { if (Values.Length > 2) Values[2] = value; }
        }

        public int WidthInt => (int) Width;

        public NumericBaseType Height {
            get => Values.Length > 3 ? Values[3] : Metrics.EmptyValue;
            set { if (Values.Length > 3) Values[3]= value; }
        }

        public int HeightInt => (int) Height;

        //===== From
        
        public static MzRectangle From(object value) {
            switch (value) {
                case double[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(NumericBaseTypeArray);
                }
                
                case float[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(NumericBaseTypeArray);
                }
                
                case decimal[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(NumericBaseTypeArray);
                }
                
                case int[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(NumericBaseTypeArray);
                }
                
                case long[] array: {
                    var NumericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(NumericBaseTypeArray);
                }
                
                case byte[] array: {
                    var numericBaseTypeArray = Array.ConvertAll(array, x => (NumericBaseType)x);
                    return new MzRectangle(numericBaseTypeArray);
                }

                default:
                    return new MzRectangle(0, 0, 0, 0);
            }
        }

        //===== Implicit Conversions
        
        //===== Operator overloading

        public static bool operator ==(MzRectangle a, MzRectangle b) { return Metrics.Equals(a.Values, b.Values); }
        public static bool operator ==(MzRectangle a, object b) { return Metrics.Equals(a.Values, b); }
        public static bool operator ==(object a, MzRectangle b) { return Metrics.Equals(b.Values, a); }

        public static bool operator !=(MzRectangle a, MzRectangle b) { return !Metrics.Equals(a.Values, b.Values); }
        public static bool operator !=(MzRectangle a, object b) { return !Metrics.Equals(a.Values, b); }
        public static bool operator !=(object a, MzRectangle b) { return !Metrics.Equals(b.Values, a); }
        
        public static MzRectangle operator *(MzRectangle a, MzRectangle b) { return new MzRectangle(Metrics.Multiply(a.Values, b.Values, false)); }
        public static MzRectangle operator *(MzRectangle a, object b) { return new MzRectangle(Metrics.Multiply(a.Values, b, false)); }
        public static MzRectangle operator *(object a, MzRectangle b) { return new MzRectangle(Metrics.Multiply(b.Values, a, false)); }
        
        public static MzRectangle operator /(MzRectangle a, MzRectangle b) { return new MzRectangle(Metrics.Divide(a.Values, b.Values, false)); }
        public static MzRectangle operator /(MzRectangle a, object b) { return new MzRectangle(Metrics.Divide(a.Values, b, false)); }
        public static MzRectangle operator /(object a, MzRectangle b) { return new MzRectangle(Metrics.Divide(b.Values, a, false)); }

        public static MzRectangle operator +(MzRectangle a, MzRectangle b) { return new MzRectangle(Metrics.Add(a.Values, b.Values, false)); }
        public static MzRectangle operator +(MzRectangle a, object b) { return new MzRectangle(Metrics.Add(a.Values, b, false)); }
        public static MzRectangle operator +(object a, MzRectangle b) { return new MzRectangle(Metrics.Add(b.Values, a, false)); }
        
        public static MzRectangle operator -(MzRectangle a, MzRectangle b) { return new MzRectangle(Metrics.Subtract(a.Values, b.Values, false)); }
        public static MzRectangle operator -(MzRectangle a, object b) { return new MzRectangle(Metrics.Subtract(a.Values, b, false)); }
        public static MzRectangle operator -(object a, MzRectangle b) { return new MzRectangle(Metrics.Subtract(b.Values, a, false)); }

        public static MzRectangle operator -(MzRectangle vector) {
            var vectorNew = (MzRectangle)vector.Clone();
            Metrics.MultiplyNumber(vectorNew.Values, -1, true);
            return vectorNew;
        }
        
        //===== IEquatable
        
        public override bool Equals(object value) {
            switch (value) {
                case MzRectangle vector: 
                    return Metrics.Equals(Values, vector.Values);
                case double[] _:
                    return Metrics.Equals(Values, value);
                case float[] _:
                    return Metrics.Equals(Values, value);
                default:
                    return false;
            }
        }
        
        public bool Equals(MzRectangle value) { return Metrics.Equals(Values, value.Values); }
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
            if (!(value is MzRectangle)) throw new ArgumentException("Object is not a MzRectangle");
            if (MagnitudeSquared > ((MzRectangle)value).MagnitudeSquared) return 1;
            if (MagnitudeSquared < ((MzRectangle)value).MagnitudeSquared) return -1;
            return 0;
        }

        public int CompareTo(MzRectangle value) {
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