using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mz.ExpressionTools;

namespace Mz.Models
{
    public class ModelCollectionParallel<TData> where TData : class, new()
    {
        public ModelCollectionParallel(ModelCollection<TData> collection)
        {
            _collection = collection;
            _models = new List<ModelParallel<TData>>();
            
            for (var i = 0; i < _collection.Count; i++)
            {
                var modelParallel = _collection.ElementAt(i).Parallel;
                _models.Add(modelParallel);
            }
        }

        ~ModelCollectionParallel()
        {
            _models.Clear();
        }
        
        private ModelCollection<TData> _collection;
        private List<ModelParallel<TData>> _models;
        
        public ModelCollectionParallel<TData> Set<TValue>(
            Expression<Func<TData, TValue>> propertyAccessorFunc,
            TValue value,
            bool isTriggerChangedEvent = true,
            bool isTriggerAutoSave = true
        )
        {
            var chain = Expressions.GetExpressionChain(propertyAccessorFunc);
            
            for (var i = 0; i < _models.Count; i++)
            {
                _models[i].Set(chain, value);
            }
            
            return this;
        }

        public void Build(
            IEnumerable<ModelParallel<TData>> models,
            bool isTriggerChangedEvent = true,
            bool isTriggerAutoSave = true
        )
        {
            for (var i = 0; i < _models.Count; i++)
            {
                _models[i].Build(isTriggerChangedEvent, isTriggerAutoSave);
            }
        }
    }
}