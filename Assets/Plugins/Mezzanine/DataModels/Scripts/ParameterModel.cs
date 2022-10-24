using System;
using System.Linq.Expressions;
using Mz.ExpressionTools;
using Mz.Parameters;

namespace Mz.Models
{
    public class ParameterModel<TData, TValue> : Parameter where TData : class, new()
    {
        public ParameterModel(
            IParameterFactory factory,
            Model<TData> model,
            Expression<Func<TData, TValue>> propertyAccessorFunc,
            object key = null,
            object initialValue = null,
            string label = null
        )
            : base(factory, key, null, label)
        {
            _model = model;
            _expressionChain = Expressions.GetExpressionChain(propertyAccessorFunc);
            if (initialValue != null) _model.Set(_expressionChain, initialValue);
        }

        private Model<TData> _model;
        private ExpressionChain<TData, TValue> _expressionChain;

        protected override object _GetValue()
        {
            if (!_expressionChain.IsCompiled) _expressionChain.Compile(_model.Data);
            return _expressionChain.Get();
        }

        protected override void _SetValue(object value)
        {
            _model.Set(_expressionChain, value);
        }
    }
}