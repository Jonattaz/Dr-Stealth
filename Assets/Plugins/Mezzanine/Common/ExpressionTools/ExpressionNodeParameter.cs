using System.Linq.Expressions;

namespace Mz.ExpressionTools
{
    public class ExpressionNodeParameter: ExpressionChainNode
    {
        public ExpressionNodeParameter(Expression expression) : base(expression)
        {
            Type = ExpressionNodeType.Parameter;
        }

        public override void Precompile()
        {
            var expressionParameter = (ParameterExpression)Expression;
            Key = expressionParameter.Type.ToString();
            Label = Key;
            ExpressionPrevious = null;
        }
    }
}