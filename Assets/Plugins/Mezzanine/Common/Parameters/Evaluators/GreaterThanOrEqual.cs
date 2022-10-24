using System;
using System.Collections.Generic;

namespace Mz.Parameters.Evaluators
{
    public class GreaterThanOrEqual : Parameter
    {
        public GreaterThanOrEqual(
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
                case ParameterTypeGeneral.Number:
                    var valueLeftNumber = Convert.ToDouble(left.Value);
                    var valueRightNumber = Convert.ToDouble(right.Value);
                    var valueNumber = valueLeftNumber >= valueRightNumber;
                    parameterOut = _create.ParameterInstance(null, valueNumber);
                    break;
                default:
                    var valueDefault = Parameter.Comparison(left, right) >= 0;
                    parameterOut = _create.ParameterInstance(null, valueDefault);
                    break;
            }

            parameterOut.Type = ParameterType.Boolean;
            return parameterOut;
        }
    }
}