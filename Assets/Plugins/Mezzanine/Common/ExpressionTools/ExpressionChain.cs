using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Mz.ExpressionTools
{
    public interface IExpressionChain
    {
        Expression Expression { get; }
        List<ExpressionChainNode> Links { get; }
        string Path { get; set; }
        object ValuePrevious { get; }
        object ValueNew { get; }
        bool IsCompiled { get; }

        IExpressionChain Compile(object previousInstance);
        IExpressionChain Set(object value);
        IExpressionChain Set(object previousInstance, object value);
    }

    public class ExpressionChain<TData, TValue> : IExpressionChain
    {
        public ExpressionChain(Expression<Func<TData, TValue>> expression)
        {
            Expression = expression;
            Links = new List<ExpressionChainNode>();
        }

        public Expression Expression { get; protected set; }
        public List<ExpressionChainNode> Links { get; protected set; }
        public string Path { get; set; }
        public object ValuePrevious { get; protected set; }
        public object ValueNew { get; protected set; }
        public bool IsCompiled { get; protected set; }

        private object _baseInstance;
        private ExpressionChainNode _lastLink;
        private object _lastPreviousInstance;

        public IExpressionChain Compile(object previousInstance)
        {
            if (IsCompiled && _baseInstance == previousInstance) return this;
            _baseInstance = previousInstance;
            IsCompiled = true;

            for (var i = Links.Count - 1; i >= 0; i--)
            {
                var link = Links[i];

                if (
                    link.Type != ExpressionNodeType.Array &&
                    link.Type != ExpressionNodeType.CollectionItemAccessor &&
                    link.Type != ExpressionNodeType.Member
                ) continue;

                if (!link.IsCompiled) link.Compile(previousInstance.GetType());
                var instance = link.GetValue(previousInstance);

                // We'll use the last link to set values.
                if (i == 0)
                {
                    _lastLink = link;
                    _lastPreviousInstance = previousInstance;
                }

                previousInstance = instance;
            }

            return this;
        }

        /// <summary>
        /// Chain must be compiled before using this method.
        /// </summary>
        public TValue Get()
        {
            return (TValue) _lastLink.GetValue(_lastPreviousInstance);
        }

        public IExpressionChain Set(object value)
        {
            if (!IsCompiled || _lastLink == null || _lastPreviousInstance == null) return this;
            ValuePrevious = (TValue) _lastLink.GetValue(_lastPreviousInstance);
            _lastLink.SetValue(_lastPreviousInstance, value);
            ValueNew = (TValue) value;
            return this;
        }

        public IExpressionChain Set(object previousInstance, object value)
        {
            Compile(previousInstance);
            return Set(value);
        }
    }
}