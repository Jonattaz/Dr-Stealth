using System;
using System.Linq.Expressions;

namespace Mz.Models
{
    public static class ModelExtensions
    {
        public static Model<TData> Model<TData>(
            this TData instance
        ) where TData : class, new()
        {
            return new Model<TData>(instance);
        }
        
        public static Model<TData> Set<TData, TValue>(
            this TData instance,
            Expression<Func<TData, TValue>> propertyAccessorFunc, 
            TValue value,
            bool isAsync = false
        ) where TData : class, new()
        {
            var model = new Model<TData>(instance);
            return model.Set(propertyAccessorFunc, value, isAsync);
        }
    }
}