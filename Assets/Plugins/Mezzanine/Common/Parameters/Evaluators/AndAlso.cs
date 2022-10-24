using System;
using System.Collections.Generic;
using Mz.Numerics;

namespace Mz.Parameters.Evaluators
{
    public class AndAlso : Parameter
    {
        public AndAlso(
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
                    var valueLeftBool = left.GetValue<bool>();
                    var valueRightBool = right.GetValue<bool>();
                    var valueBool = valueLeftBool && valueRightBool;
                    parameterOut = _create.ParameterInstance(null, valueBool);
                    break;
            }

            parameterOut.Type = ParameterType.Boolean;
            return parameterOut;
        }
    }
}