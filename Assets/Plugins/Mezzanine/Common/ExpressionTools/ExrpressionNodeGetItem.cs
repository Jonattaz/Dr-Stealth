using System.Linq.Expressions;

namespace Mz.ExpressionTools
{
    public class ExrpressionNodeGetItem: ExpressionChainNode
    {
        public ExrpressionNodeGetItem(Expression expression) : base(expression)
        {
            Type = ExpressionNodeType.CollectionItemAccessor;
        }

        private MethodCallExpression _call;
        private object _accessor;
        
        public override void Precompile()
        {
            _call = (MethodCallExpression)Expression;
            _accessor = ((ConstantExpression)_call.Arguments[0]).Value;
            Key = $"GetItem({_call.Arguments[0]}";
            Label = $"{Expressions.Delimiter}{Key})";
            ExpressionPrevious = ((MethodCallExpression)Expression).Object;
        }

        public override object GetValue(object previousInstance)
        {
            return _call.Method.Invoke(previousInstance, new [] { _accessor });
        }
        
        public object GetValue(object previousInstance, object accessor)
        {
            return _call.Method.Invoke(previousInstance, new [] { accessor });
        }
    }
}