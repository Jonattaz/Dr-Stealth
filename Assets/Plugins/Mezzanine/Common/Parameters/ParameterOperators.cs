using Mz.Numerics;
using Mz.TypeTools;

namespace Mz.Parameters
{
    public partial class Parameter
    {
        public static IParameter operator +(Parameter left, Parameter right)
        {
            return Create.Add(left, right);
        }
        
        public static IParameter operator -(Parameter left, Parameter right)
        {
            return Create.Subtract(left, right);
        }
        
        public static IParameter operator *(Parameter left, Parameter right)
        {
            return Create.Multiply(left, right);
        }
        
        public static IParameter operator /(Parameter left, Parameter right)
        {
            return Create.Divide(left, right);
        }
        
        public static IParameter operator &(Parameter left, Parameter right)
        {
            return Create.AndAlso(left, right);
        }

        public static bool operator false(Parameter left)
        {
            return false;
        }

        public static bool operator true(Parameter left)
        {
            return true;
        }

        public static IParameter operator |(Parameter left, Parameter right)
        {
            return Create.OrElse(left, right);
        }

        public static IParameter operator !(Parameter operand)
        {
            return Create.Negate(operand);
        }

        public static IParameter operator <(Parameter left, Parameter right)
        {
            return Create.LessThan(left, right);
        }

        public static IParameter operator >(Parameter left, Parameter right)
        {
            return Create.GreaterThan(left, right);
        }

        public static IParameter operator <=(Parameter left, Parameter right)
        {
            return Create.LessThanOrEqual(left, right);
        }

        public static IParameter operator >=(Parameter left, Parameter right)
        {
            return Create.GreaterThanOrEqual(left, right);
        }

        public static IParameter operator ==(Parameter left, Parameter right)
        {
            return Create.Equal(left, right);
        }

        public static IParameter operator !=(Parameter left, Parameter right)
        {
            return Create.NotEqual(left, right);
        }
        
        public static Parameter operator --(Parameter parameter) {
            if (!parameter.Value.IsNumber()) return parameter;
       
            switch (parameter.Type) {
                case ParameterType.Byte:
                    var valueByte = parameter.GetValue<byte>();
                    parameter.Value = --valueByte;
                    break;
                case ParameterType.Int:
                    var valueInt = parameter.GetValue<int>();
                    parameter.Value = --valueInt;
                    break;
                case ParameterType.Float:
                    var valueFloat = parameter.GetValue<float>();
                    parameter.Value = --valueFloat;
                    break;
                case ParameterType.Double:
                    var valueDouble = parameter.GetValue<double>();
                    parameter.Value = --valueDouble;
                    break;
                case ParameterType.Vector:
                    var valueVector = parameter.GetValue<float[]>();
                    parameter.Value = Metrics.SubtractNumber(valueVector, 1);
                    break;
            }
            
            return parameter;
        }
        
        public static Parameter operator ++(Parameter parameter) {
            if (!parameter.Value.IsNumber()) return parameter;
       
            switch (parameter.Type) {
                case ParameterType.Byte:
                    var valueByte = parameter.GetValue<byte>();
                    parameter.Value = ++valueByte;
                    break;
                case ParameterType.Int:
                    var valueInt = parameter.GetValue<int>();
                    parameter.Value = ++valueInt;
                    break;
                case ParameterType.Float:
                    var valueFloat = parameter.GetValue<float>();
                    parameter.Value = ++valueFloat;
                    break;
                case ParameterType.Double:
                    var valueDouble = parameter.GetValue<double>();
                    parameter.Value = ++valueDouble;
                    break;
            }
            
            return parameter;
        }
    }
}