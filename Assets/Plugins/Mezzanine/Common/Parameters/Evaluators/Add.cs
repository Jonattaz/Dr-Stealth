using System;
using System.Collections.Generic;
using System.Linq;
using Mz.Numerics;

namespace Mz.Parameters.Evaluators
{
    public class Add : Parameter
    {
        public Add(
            IParameterFactory factory,
            IParameter left = null,
            IParameter right = null
        ) : base(factory)
        {
            _left = left;
            _right = right;
        }

        private readonly IParameter _left;
        private readonly IParameter _right;

        public override IParameter Evaluate(Stack<IParameter> expressionStack)
        {
            var right = _right != null ? _right.Evaluate(expressionStack) : expressionStack?.Pop();
            var left = _left != null ? _left.Evaluate(expressionStack) : expressionStack?.Pop();
            if (left == null) left = _create.ParameterEmpty();
            if (right == null) right = _create.ParameterEmpty();

            var parameterOut = _create.ParameterEmpty();

            switch (left.TypeGeneral)
            {
                case ParameterTypeGeneral.Boolean:
                    var valueLeftBoolean = left.GetValue<bool>();
                    var valueRightBoolean = right.GetValue<bool>();
                    var valueBoolean = valueLeftBoolean && valueRightBoolean;
                    parameterOut = _create.ParameterInstance(null, valueBoolean);
                    parameterOut.Type = left.Type;
                    break;
                case ParameterTypeGeneral.Collection:
                    var valueLeftCollection = left.GetValue<object[]>();
                    var valueRightCollection = right.GetValue<object[]>();
                    var valueCollection = valueLeftCollection.Concat(valueRightCollection);
                    parameterOut = _create.ParameterInstance(null, valueCollection);
                    parameterOut.Type = left.Type;
                    break;
                case ParameterTypeGeneral.DoubleArray:
                    var valueLeftDoubleArray = left.GetValue<float[]>();
                    var valueRightDoubleArray = right.GetValue<float[]>();
                    var valueDoubleArray = Metrics.Add(valueLeftDoubleArray, valueRightDoubleArray);
                    parameterOut = _create.ParameterInstance(null, valueDoubleArray);
                    parameterOut.Type = left.Type;
                    break;
                case ParameterTypeGeneral.Metric:
                    var valueLeftMetric = left.GetValue<IMetric>();
                    var valueRightMetric = right.GetValue<IMetric>();

                    switch (left.Type)
                    {
                        case ParameterType.Color:
                            var valueFinalColor = (MzColor) valueLeftMetric + (MzColor) valueRightMetric;
                            parameterOut = _create.ParameterInstance(null, valueFinalColor);
                            break;
                        case ParameterType.Size:
                            var valueFinalSize = (MzSize) valueLeftMetric + (MzSize) valueRightMetric;
                            parameterOut = _create.ParameterInstance(null, valueFinalSize);
                            break;
                        case ParameterType.Quaternion:
                            var valueFinalQuaternion = (MzQuaternion) valueLeftMetric + (MzQuaternion) valueRightMetric;
                            parameterOut = _create.ParameterInstance(null, valueFinalQuaternion);
                            break;
                        case ParameterType.Rectangle:
                            var valueFinalRectangle = (MzRectangle) valueLeftMetric + (MzRectangle) valueRightMetric;
                            parameterOut = _create.ParameterInstance(null, valueFinalRectangle);
                            break;
                        case ParameterType.Vector:
                            var valueFinalVector = (MzVector) valueLeftMetric + (MzVector) valueRightMetric;
                            parameterOut = _create.ParameterInstance(null, valueFinalVector);
                            break;
                    }

                    parameterOut.Type = left.Type;
                    break;
                case ParameterTypeGeneral.Number:
                    var valueLeftNumber = Convert.ToDouble(left.Value);
                    var valueRightNumber = Convert.ToDouble(right.Value);
                    var valueNumber = valueLeftNumber + valueRightNumber;

                    switch (left.Type)
                    {
                        case ParameterType.Byte:
                            var valueFinalByte = (byte) valueNumber;
                            parameterOut = _create.ParameterInstance(null, valueFinalByte);
                            break;
                        case ParameterType.Int:
                            var valueFinalInt = (int) valueNumber;
                            parameterOut = _create.ParameterInstance(null, valueFinalInt);
                            break;
                        case ParameterType.Float:
                            var valueFinalFloat = (float) valueNumber;
                            parameterOut = _create.ParameterInstance(null, valueFinalFloat);
                            break;
                        default:
                            var valueFinalNumber = valueNumber;
                            parameterOut = _create.ParameterInstance(null, valueFinalNumber);
                            break;
                    }

                    parameterOut.Type = left.Type;
                    break;
                case ParameterTypeGeneral.String:
                    var valueLeftString = left.GetValue<string>();
                    var valueRightString = right.GetValue<string>();
                    var valueString = valueLeftString + valueRightString;
                    parameterOut = _create.ParameterInstance(null, valueString);
                    parameterOut.Type = left.Type;
                    break;
            }

            return parameterOut;
        }
    }
}