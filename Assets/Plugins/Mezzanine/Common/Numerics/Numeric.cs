using System;
using System.Collections.Generic;
using System.Globalization;

namespace Mz.Numerics
{
    using NumericBaseType = System.Single;
    
    public struct Numeric : IComparable, IEquatable<Numeric>, IFormattable
    {
        private Numeric(int value)
        {
            Value = (NumericBaseType)Convert.ToDouble(value);
        }
        
        private Numeric(float value)
        {
            Value = (NumericBaseType)Convert.ToDouble(value);
        }
        
        private Numeric(double value)
        {
            Value = (NumericBaseType)Convert.ToDouble(value);
        }
        
        private Numeric(long value)
        {
            Value = (NumericBaseType)Convert.ToDouble(value);
        }
        
        private NumericBaseType Value { get; }

        public const NumericBaseType MinValue = NumericBaseType.MinValue;
        public const NumericBaseType MaxValue = NumericBaseType.MaxValue;
        public const NumericBaseType NegativeInfinity = NumericBaseType.NegativeInfinity;
        public const NumericBaseType PositiveInfinity = NumericBaseType.PositiveInfinity;
        public const NumericBaseType NaN = NumericBaseType.NaN;

        public static bool IsNegativeInfinity(Numeric value)
        {
            return value == (Numeric)NegativeInfinity;
        }
        
        public static bool IsPositiveInfinity(Numeric value)
        {
            return value == PositiveInfinity;
        }

        public string ToString(string value, IFormatProvider formatProvider)
        {
            return Value.ToString(CultureInfo.CurrentCulture);
        }
        
        public override string ToString()
        {
            return ToString(null, System.Globalization.CultureInfo.CurrentCulture);
        }

        public int CompareTo(object other)
        {
            switch (other)
            {
                case Numeric otherConverted:
                    if (this == otherConverted) return 0;
                    if (this > otherConverted) return 1;
                    return -1;
                case double otherConverted:
                    if (Numbers.Abs(this - otherConverted) < Numbers.Epsilon) return 0;
                    if (this > otherConverted) return 1;
                    return -1;
                case float otherConverted:
                    if (Numbers.Abs(this - otherConverted) < Numbers.Epsilon) return 0;
                    if (this > otherConverted) return 1;
                    return -1;
                case int otherConverted:
                    if (Numbers.Abs(this - otherConverted) < Numbers.Epsilon) return 0;
                    if (this > otherConverted) return 1;
                    return -1;
                case long otherConverted:
                    if (Numbers.Abs(this - (NumericBaseType)otherConverted) < Numbers.Epsilon) return 0;
                    if (this > (NumericBaseType)otherConverted) return 1;
                    return -1;
            }

            return -1;
        }

        bool IEquatable<Numeric>.Equals(Numeric other) => Equals(other);
        public override bool Equals(object other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (other is Numeric otherNumeric)
            {
                return Numbers.Abs(Value - otherNumeric.Value) < Numbers.Epsilon;
            }
            
            return Numbers.Abs(Value - (NumericBaseType)Convert.ToDouble(other)) < Numbers.Epsilon;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<NumericBaseType>.Default.GetHashCode(Value);
        }
        
        //===== Implicit conversions
        
        //----- int
        
        public static implicit operator Numeric(int value)
        {
            return new Numeric(value);
        }

        public static implicit operator int(Numeric value)
        {
            return Convert.ToInt32(value.Value);
        }
        
        //----- float
        
        public static implicit operator Numeric(float value)
        {
            return new Numeric(value);
        }

        public static implicit operator float(Numeric value)
        {
            return (float)Convert.ToDouble(value.Value);
        }

        //----- double
        
        public static implicit operator Numeric(double value)
        {
            return new Numeric(value);
        }

        public static implicit operator double(Numeric value)
        {
            return Convert.ToDouble(value.Value);
        }
        
        //----- long
        
        public static implicit operator Numeric(long value)
        {
            return new Numeric(value);
        }

        public static implicit operator long(Numeric value)
        {
            return Convert.ToInt64(value.Value);
        }
        
        //----- bool
        
        public static implicit operator Numeric(bool value)
        {
            return value ? new Numeric(1) : new Numeric(0);
        }

        public static implicit operator bool(Numeric value)
        {
            return value > 0;
        }
        
        //===== Operator overloads
        
        //----- Numeric / Numeric

        private static bool _LessThan(Numeric a, Numeric b)
        {
            return !_Equals(a, b) && Comparer<NumericBaseType>.Default.Compare(a.Value, b.Value) < 0;
        }
        
        private static bool _Equals(Numeric a, Numeric b)
        {
            return Numbers.Abs(a.Value - b.Value) < Numbers.Epsilon;
        }

        private static bool _GreaterThan(Numeric a, Numeric b)
        {
            return !_LessThan(a, b) && !_Equals(a, b);
        }

        public static bool operator <(Numeric a, Numeric b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(Numeric a, Numeric b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(Numeric a, Numeric b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(Numeric a, Numeric b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Numeric a, Numeric b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(Numeric a, Numeric b)
        {
            return !(a == b);
        }

        public static Numeric operator +(Numeric a, Numeric b)
        {
            return a.Value + b.Value;
        }

        public static Numeric operator -(Numeric a, Numeric b)
        {
            return a.Value - b.Value;
        }
        
        public static Numeric operator *(Numeric a, Numeric b)
        {
            return a.Value * b.Value;
        }

        public static Numeric operator /(Numeric a, Numeric b)
        {
            return a.Value / b.Value;
        }
        
        //----- double / Numeric

        public static bool operator <(double a, Numeric b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(double a, Numeric b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(double a, Numeric b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(double a, Numeric b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(double a, Numeric b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(double a, Numeric b)
        {
            return !(a == b);
        }

        public static Numeric operator +(double a, Numeric b)
        {
            return a + b.Value;
        }

        public static Numeric operator -(double a, Numeric b)
        {
            return a - b.Value;
        }
        
        public static Numeric operator *(double a, Numeric b)
        {
            return a * b.Value;
        }

        public static Numeric operator /(double a, Numeric b)
        {
            return a / b.Value;
        }
        
        //----- Numeric / double

        public static bool operator <(Numeric a, double b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(Numeric a, double b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(Numeric a, double b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(Numeric a, double b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Numeric a, double b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(Numeric a, double b)
        {
            return !(a == b);
        }

        public static Numeric operator +(Numeric a, double b)
        {
            return a.Value + b;
        }

        public static Numeric operator -(Numeric a, double b)
        {
            return a.Value - b;
        }
        
        public static Numeric operator *(Numeric a, double b)
        {
            return a.Value * b;
        }

        public static Numeric operator /(Numeric a, double b)
        {
            return a.Value / b;
        }
     
        //----- float / Numeric

        public static bool operator <(float a, Numeric b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(float a, Numeric b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(float a, Numeric b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(float a, Numeric b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(float a, Numeric b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(float a, Numeric b)
        {
            return !(a == b);
        }

        public static Numeric operator +(float a, Numeric b)
        {
            return a + b.Value;
        }

        public static Numeric operator -(float a, Numeric b)
        {
            return a - b.Value;
        }
        
        public static Numeric operator *(float a, Numeric b)
        {
            return a * b.Value;
        }

        public static Numeric operator /(float a, Numeric b)
        {
            return a / b.Value;
        }
        
        //----- Numeric / float

        public static bool operator <(Numeric a, float b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(Numeric a, float b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(Numeric a, float b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(Numeric a, float b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Numeric a, float b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(Numeric a, float b)
        {
            return !(a == b);
        }

        public static Numeric operator +(Numeric a, float b)
        {
            return a.Value + b;
        }

        public static Numeric operator -(Numeric a, float b)
        {
            return a.Value - b;
        }
        
        public static Numeric operator *(Numeric a, float b)
        {
            return a.Value * b;
        }

        public static Numeric operator /(Numeric a, float b)
        {
            return a.Value / b;
        }
        
        //----- int / Numeric

        public static bool operator <(int a, Numeric b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(int a, Numeric b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(int a, Numeric b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(int a, Numeric b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(int a, Numeric b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(int a, Numeric b)
        {
            return !(a == b);
        }

        public static Numeric operator +(int a, Numeric b)
        {
            return a + b.Value;
        }

        public static Numeric operator -(int a, Numeric b)
        {
            return a - b.Value;
        }
        
        public static Numeric operator *(int a, Numeric b)
        {
            return a * b.Value;
        }

        public static Numeric operator /(int a, Numeric b)
        {
            return a / b.Value;
        }
        
        //----- Numeric / int

        public static bool operator <(Numeric a, int b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(Numeric a, int b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(Numeric a, int b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(Numeric a, int b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Numeric a, int b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(Numeric a, int b)
        {
            return !(a == b);
        }

        public static Numeric operator +(Numeric a, int b)
        {
            return a.Value + b;
        }

        public static Numeric operator -(Numeric a, int b)
        {
            return a.Value - b;
        }
        
        public static Numeric operator *(Numeric a, int b)
        {
            return a.Value * b;
        }
        
        public static Numeric operator /(Numeric a, int b)
        {
            return a.Value / b;
        }
      
        //----- long / Numeric

        public static bool operator <(long a, Numeric b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(long a, Numeric b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(long a, Numeric b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(long a, Numeric b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(long a, Numeric b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(long a, Numeric b)
        {
            return !(a == b);
        }

        public static Numeric operator +(long a, Numeric b)
        {
            return a + b.Value;
        }

        public static Numeric operator -(long a, Numeric b)
        {
            return a - b.Value;
        }
        
        public static Numeric operator *(long a, Numeric b)
        {
            return a * b.Value;
        }

        public static Numeric operator /(long a, Numeric b)
        {
            return a / b.Value;
        }
        
        //----- Numeric / long

        public static bool operator <(Numeric a, long b)
        {
            return _LessThan(a, b);
        }
        
        public static bool operator ==(Numeric a, long b)
        {
            return _Equals(a, b);
        }

        public static bool operator >(Numeric a, long b)
        {
            return _GreaterThan(a, b);
        }

        public static bool operator <=(Numeric a, long b)
        {
            return a < b || a == b;
        }

        public static bool operator >=(Numeric a, long b)
        {
            return a > b || a == b;
        }

        public static bool operator !=(Numeric a, long b)
        {
            return !(a == b);
        }

        public static Numeric operator +(Numeric a, long b)
        {
            return a.Value + b;
        }

        public static Numeric operator -(Numeric a, long b)
        {
            return a.Value - b;
        }
        
        public static Numeric operator *(Numeric a, long b)
        {
            return a.Value * b;
        }
        
        public static Numeric operator /(Numeric a, long b)
        {
            return a.Value / b;
        }
    }
}