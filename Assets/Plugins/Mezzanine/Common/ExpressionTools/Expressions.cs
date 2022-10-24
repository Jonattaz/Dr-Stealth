using System;
#if (!UNITY_IOS && !UNITY_WEBGL)
using System.Collections.Concurrent;
#else
using System.Collections.Generic;
#endif
using System.Linq.Expressions;
using System.Text;

namespace Mz.ExpressionTools
{
    public class Expressions
    {
        static Expressions()
        {
#if (!UNITY_IOS && !UNITY_WEBGL)
            _expressionChainCache = new ConcurrentDictionary<Expression, IExpressionChain>();
            _expressionChainLinkCache = new ConcurrentDictionary<Expression, ExpressionChainNode>();
#else
            _expressionChainCache = new Dictionary<Expression, IExpressionChain>();
            _expressionChainLinkCache = new Dictionary<Expression, ExpressionChainNode>();
#endif
        }

#if (!UNITY_IOS && !UNITY_WEBGL)
        private static ConcurrentDictionary<Expression, IExpressionChain> _expressionChainCache;
        private static ConcurrentDictionary<Expression, ExpressionChainNode> _expressionChainLinkCache;
#else
        private static Dictionary<Expression, IExpressionChain> _expressionChainCache;
        private static Dictionary<Expression, ExpressionChainNode> _expressionChainLinkCache;
#endif

        public static char Delimiter => '.';

        public int CacheSizeChain => _expressionChainCache.Count;
        public int ChacheSizeLink => _expressionChainLinkCache.Count;

        public void ClearCaches()
        {
            _expressionChainCache?.Clear();
            _expressionChainLinkCache?.Clear();
        }

        public static ExpressionChain<TData, TValue> GetExpressionChain<TData, TValue>(
            Expression<Func<TData, TValue>> expressionIn)
        {
            if (_expressionChainCache.ContainsKey(expressionIn))
            {
                _expressionChainCache.TryGetValue(expressionIn, out var chainValue);
                if (chainValue != null) return (ExpressionChain<TData, TValue>) chainValue;
            }

            var chain = new ExpressionChain<TData, TValue>(expressionIn);
            var expression = expressionIn.Body;
            var path = new StringBuilder();

            while (expression != null)
            {
                ExpressionChainNode link;

                if (_expressionChainLinkCache.ContainsKey(expression))
                {
                    _expressionChainLinkCache.TryGetValue(expression, out link);
                    if (link != null) chain.Links.Add(link);
                }
                else
                {
                    switch (expression.NodeType)
                    {
                        case ExpressionType.ArrayIndex:
                            link = new ExpressionNodeArray(expression);
                            chain.Links.Add(link);

#if (!UNITY_IOS && !UNITY_WEBGL)
                            _expressionChainLinkCache.TryAdd(expression, link);
#else
                            _expressionChainLinkCache.Add(expression, link);
#endif
                            break;
                        case ExpressionType.MemberAccess:
                            link = new ExpressionNodeMember(expression);
                            chain.Links.Add(link);

#if (!UNITY_IOS && !UNITY_WEBGL)
                            _expressionChainLinkCache.TryAdd(expression, link);
#else
                            _expressionChainLinkCache.Add(expression, link);
#endif
                            break;
                        case ExpressionType.Call:
                            var expressionCall = (MethodCallExpression) expression;
                            if (expressionCall.Method.Name == "get_Item")
                            {
                                // We're accessing an element in a list, or dictionary.
                                link = new ExrpressionNodeGetItem(expression);
                            }
                            else
                            {
                                link = new ExpressionNodeCall(expression);
                            }

                            chain.Links.Add(link);

#if (!UNITY_IOS && !UNITY_WEBGL)
                            _expressionChainLinkCache.TryAdd(expression, link);
#else
                            _expressionChainLinkCache.Add(expression, link);
#endif

                            break;
                        case ExpressionType.Parameter:
                            // There is only one Parameter, and this will represent the instance of TData.
                            // So, no need to add this link to the chain.
                            link = new ExpressionNodeParameter(expression);
                            break;
                        default:
                            throw new NotSupportedException("Unsupported expression node type encountered: " +
                                                            expression.NodeType);
                    }
                }

                if (link == null) continue;
                expression = link.ExpressionPrevious;
                path.Insert(0, link.Label);
            }

            chain.Path = path.ToString();

#if (!UNITY_IOS && !UNITY_WEBGL)
            _expressionChainCache.TryAdd(expressionIn, chain);
#else
            _expressionChainCache.Add(expressionIn, chain);
#endif

            return chain;
        }
    }
}