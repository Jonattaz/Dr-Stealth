using System;
using System.Collections.Generic;

namespace Mz.Parameters.Evaluators
{
    public class LessThan : Parameter
    {
        public LessThan(
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
            if (left == null) left = Create.Instance();
            if (right == null) right = Create.Instance();

            var parameterOut = Create.Instance();

            switch (left.TypeGeneral)
            {
                case ParameterTypeGeneral.Number:
                    var valueLeftNumber = Convert.ToDouble(left.Value);
                    var valueRightNumber = Convert.ToDouble(right.Value);
                    var valueNumber = valueLeftNumber < valueRightNumber;
                    parameterOut = Create.Instance(null, valueNumber);
                    break;
                default:
                    var valueDefault = Parameter.Comparison(left, right) < 0;
                    parameterOut = Create.Instance(null, valueDefault);
                    break;
            }

            parameterOut.Type = ParameterType.Boolean;
            return parameterOut;
        }
    }
}