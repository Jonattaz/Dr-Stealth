using System;
using System.Linq.Expressions;

namespace Mz.ExpressionTools
{
    public abstract class ExpressionChainNode
    {
        public ExpressionChainNode(Expression expression)
        {
            Expression = expression;
            Precompile();
        }

        public Expression Expression { get; private set; }
        public Expression ExpressionPrevious { get; protected set; }
        public string Key { get; set; }
        public string Label { get; set; }
        public ExpressionNodeType Type { get; set; }
        public bool IsCompiled { get; protected set; }

        public virtual void Precompile() { }

        public virtual void Compile(Type typePreviousInstance)
        {
            if (IsCompiled) return;
            IsCompiled = true;
        }

        public virtual object GetValue(object previousInstance)
        {
            return null;
        }

        public virtual void SetValue(object previousInstance, object value) { }
    }
}