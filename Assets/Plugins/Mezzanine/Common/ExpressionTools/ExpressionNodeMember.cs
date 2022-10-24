using System;
using System.Linq.Expressions;
using System.Reflection;
using Mz.TypeTools;

namespace Mz.ExpressionTools
{
    public class ExpressionNodeMember: ExpressionChainNode
    {
        public ExpressionNodeMember(Expression expression) : base(expression)
        {
            Type = ExpressionNodeType.Member;
        }

        private PropertyInfo _propertyInfo;
        private string _memberName;
        
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)   
        private Types.PropertyGetDelegate _getter;
        private Types.PropertySetDelegate _setter;
#endif
        
        public override void Precompile()
        {
            var memberExpression = (MemberExpression)Expression;
            _memberName = memberExpression.Member.Name;
            Key = $"{memberExpression.Member.Name}";
            Label = $"{Expressions.Delimiter}{Key}";
            ExpressionPrevious = memberExpression.Expression;
        }

        public override void Compile(Type typePreviousInstance)
        {
            base.Compile(typePreviousInstance);
            _propertyInfo = typePreviousInstance.GetProperty(_memberName);
            
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)   
            _getter = Types.GetPropertyGetter(_propertyInfo);
            _setter = Types.GetPropertySetter(_propertyInfo);
#endif
        }
        
        public override object GetValue(object previousInstance)
        {
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)   
            // This is much faster than `return _propertyInfo?.GetValue(previousInstance);`
            // But, it won't run on iOS.
            return _getter(previousInstance);
#else
            return _propertyInfo?.GetValue(previousInstance);
#endif
        }

        public override void SetValue(object previousInstance, object value)
        {
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)   
            // This is much faster than `_propertyInfo?.SetValue(previousInstance, value);`
            // But, it won't run on iOS.
            _setter?.Invoke(previousInstance, value);
#else
            _propertyInfo?.SetValue(previousInstance, value);
#endif
        }
    }
}