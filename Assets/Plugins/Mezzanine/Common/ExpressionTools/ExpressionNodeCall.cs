using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mz.ExpressionTools
{
    public class ExpressionNodeCall: ExpressionChainNode
    {
        public ExpressionNodeCall(Expression expression) : base(expression)
        {
            Type = ExpressionNodeType.Method;
            _arguments = new List<object>();
        }
        
        private MethodCallExpression _call;
        private List<object> _arguments;

        public override void Precompile()
        {
            _call = (MethodCallExpression)Expression;

            _arguments.Clear();
            for (var i = 0; i < _call.Arguments.Count; i++)
            {
                _arguments.Add(((ConstantExpression)_call.Arguments[0]).Value);
            }
            
            Key = $"{_call.Method.Name}()";
            Label = $"{Expressions.Delimiter}{Key})";
            ExpressionPrevious = ((MethodCallExpression)Expression).Object;
        }

        public override object GetValue(object previousInstance)
        {
            return _call.Method.Invoke(previousInstance, _arguments.ToArray());
        }

        public object GetValue(object previousInstance, object[] arguments)
        {
            return _call.Method.Invoke(previousInstance, arguments);
        }
    }
}