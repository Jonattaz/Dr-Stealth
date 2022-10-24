using System;
using System.Linq.Expressions;
using Mz.ExpressionTools;

namespace Mz.Models
{
    public abstract class SetOperationBase
    {
        public virtual object GetValue() { return default; }
        public virtual IExpressionChain GetExpressionChain() { return null; }
    }
    
    public class SetOperation<TData, TValue> : SetOperationBase
    {
        public SetOperation(
            Expression<Func<TData, TValue>> expression,
            TValue value
        )
        {
            _expression = expression;
            _value = value;
        }

        public SetOperation(
            IExpressionChain chain,
            TValue value
        )
        {
            _chain = chain;
            _value = value;
        }

        private Expression<Func<TData, TValue>> _expression;
        private IExpressionChain _chain;
        private TValue _value;

        public override object GetValue() { return _value; }

        public override IExpressionChain GetExpressionChain()
        {
            if (_chain != null) return _chain;
            if (_expression == null) return null;
            _chain = Expressions.GetExpressionChain(_expression);
            return _chain;
        }
    }
}