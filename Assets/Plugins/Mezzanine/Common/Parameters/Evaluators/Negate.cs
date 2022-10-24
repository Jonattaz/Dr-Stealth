using System.Collections.Generic;

namespace Mz.Parameters.Evaluators
{
    public class Negate : Parameter
    {
        public Negate(
            IParameterFactory factory,
            IParameter right = null
        ) : base(factory)
        {
            _right = right;
        }
        
        private readonly IParameter _right;

        public override IParameter Evaluate(Stack<IParameter> expressionStack)
        {
            var right = _right != null ? _right.Evaluate(expressionStack) : expressionStack?.Pop();
            if (right == null) right = _create.ParameterEmpty();

            var parameterOut = _create.ParameterEmpty();

            switch (right.TypeGeneral)
            {
                case ParameterTypeGeneral.Boolean:
                    var valueRightBool = right.GetValue<bool>();
                    var valueBool = !valueRightBool;
                    parameterOut = _create.ParameterInstance(null, valueBool);
                    break;
            }

            parameterOut.Type = ParameterType.Boolean;
            return parameterOut;
        }
    }
}