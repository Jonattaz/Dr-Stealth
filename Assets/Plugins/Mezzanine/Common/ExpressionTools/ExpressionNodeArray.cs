using System;
using System.Linq.Expressions;
using System.Reflection;
using Mz.TypeTools;

namespace Mz.ExpressionTools
{
    public class ExpressionNodeArray: ExpressionChainNode
    {
        public ExpressionNodeArray(Expression expression) : base(expression)
        {
            Type = ExpressionNodeType.Array;
        }
        
        private int _index;
        private MemberExpression _expressionArray;

#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)   
        private Types.PropertyGetDelegate _getter;
#else
        private PropertyInfo _propertyInfo;
#endif

        public override void Precompile()
        {
            var expressionRight = ((BinaryExpression)Expression).Right;

            switch (expressionRight.NodeType)
            {
                case ExpressionType.Constant:
                    _index = (int)((ConstantExpression)expressionRight).Value;
                    break;
                case ExpressionType.MemberAccess:
                    _index = (int)_MemberToValue((MemberExpression)expressionRight);
                    break;
                default:
                    var lambda = Expression.Lambda(expressionRight);
                    var compiled = lambda.Compile();
                    _index = (int)compiled.DynamicInvoke();
                    break;
            }
            
            _expressionArray = (MemberExpression)((BinaryExpression)Expression).Left;
            Key = $"{_expressionArray.Member.Name}[{_index}]";
            Label = $"{Expressions.Delimiter}{Key}";
            ExpressionPrevious = _expressionArray.Expression;
        }
        
        private object _MemberToValue(MemberExpression expression) {
            var memberInfo = expression.Member;
            object value = null;

            switch (memberInfo.MemberType) {
                // We've got a member property, and we need to get the value it points to.
                case MemberTypes.Property: 
                    // We've got a class property, and we need to unpack the value it holds.
                    var lambda = Expression.Lambda(expression.Expression);
                    var compiled = lambda.Compile();
                    value = compiled.DynamicInvoke();
                    return value;
                case MemberTypes.Field: 
                    // We've got a common variable, and we need to unpack the value it holds.
                    var fieldInfo = (FieldInfo)memberInfo;

                    if (expression.Expression == null) return null;

                    try
                    {
                        if (expression.Expression.GetType() == typeof(ConstantExpression)) {
                            var container = ((ConstantExpression)expression.Expression).Value;
                            value = fieldInfo.GetValue(container);
                        } else {
                            // We got a MemberExpression, and we need to keep digging deeper to get its value.
                            var container = expression.Expression;
                            value = fieldInfo.GetValue(_MemberToValue((MemberExpression)container));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in ExpressionNodeArray._MemberToValue: {ex.Message}");
                        Console.WriteLine($"expression type: {expression.Expression.GetType()}, expression: {expression.Expression}");
                
                        return null;
                    }

                    return value;
                default:
                    throw new NotSupportedException(
                        $"Unsupported reference type object instance encountered: {expression.Expression.Type}."
                    );
            }
        }
        
        public override void Compile(Type typePreviousInstance)
        {
            base.Compile(typePreviousInstance);
            
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
            _getter = Types.GetPropertyGetter(typePreviousInstance, _expressionArray.Member.Name);
#else
            _propertyInfo = typePreviousInstance.GetProperty(_expressionArray.Member.Name);
#endif
        }
        
        public override object GetValue(object previousInstance)
        {
            return GetValue(previousInstance, -1);
        }
        public object GetValue(object previousInstance, int index)
        {
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
            var array = (Array)_getter?.Invoke(previousInstance);
#else  
            var array = (Array)_propertyInfo.GetValue(previousInstance);
#endif
            object value = null;
            if (array != null) value = array.GetValue(index > -1 ? index : _index);
            return value;
        }

        public override void SetValue(object previousInstance, object value)
        {
            SetValue(previousInstance, -1, value);
        }
        public void SetValue(object previousInstance, int index, object value)
        {
#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
            var array = (Array)_getter?.Invoke(previousInstance);
#else  
            var array = (Array)_propertyInfo.GetValue(previousInstance);
#endif
            array?.SetValue(value, index > -1 ? index : _index);
        }
    }
}