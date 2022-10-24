using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Mz.ExpressionTools;

namespace Mz.Models
{
    public class ModelParallel<TData> where TData : class, new()
    {
        public ModelParallel(Model<TData> model, TData data)
        {
            _model = model;
            _data = data;
            _changedProperties = new List<SetOperationBase>();
        }

        private readonly Model<TData> _model;
        private readonly TData _data;
        private readonly List<SetOperationBase> _changedProperties;
        
        public ModelParallel<TData> Set<TValue>(
            Expression<Func<TData, TValue>> propertyAccessorFunc, 
            TValue value
        )
        {
            return _SetParallel(propertyAccessorFunc, value);
        }

        public ModelParallel<TData> Set<TValue>(
            IExpressionChain chain, 
            TValue value
        )
        {
            return _SetParallel(chain, value);
        }

        public ModelParallel<TData> _SetParallel<TValue> (
            Expression<Func<TData, TValue>> propertyAccessorFunc, 
            TValue value
        )
        {
            var setOperation = new SetOperation<TData, TValue>(propertyAccessorFunc, value);
            _changedProperties.Add(setOperation);
            return this;
        }
        
        public ModelParallel<TData> _SetParallel<TValue> (
            IExpressionChain chain, 
            TValue value
        )
        {
            var setOperation = new SetOperation<TData, TValue>(chain, value);
            _changedProperties.Add(setOperation);
            return this;
        }
        
        /// <summary>
        /// Rebuild the Model after a series of changes have been made using the Set operation.
        /// </summary>
        /// <returns></returns>
        public TData Build(bool isTriggerChangedEvent = true, bool isTriggerAutoSave = true)
        {
            if (_model == null || !_model.IsInitialized) return default;
            
            var changedArgs = new ModelChangedEventArgs<TData>(_model);
            _changedProperties?.AsParallel().ForAll(setOperation =>
            {
                var value = setOperation.GetValue();

                var chain = setOperation.GetExpressionChain();
                chain.Set(_data, value);

                if (!isTriggerChangedEvent) return;
                var modelChange = new ModelChange(chain.Path, chain.ValueNew, chain.ValuePrevious);
                changedArgs.Add(modelChange);
            });

            _changedProperties?.Clear();
            
            if (!isTriggerChangedEvent || changedArgs.Changes.Count > 0) return _model.Data;
            _model.DoChanged(changedArgs);
            if (_model.IsAutoSave && isTriggerAutoSave) _model.Save();

            return _model.Data;
        }
    }
}