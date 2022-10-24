using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Mz.ExpressionTools;

namespace Mz.Models
{
    public class ModelCollection<TData> : KeyedCollection<object, Model<TData>> where TData : class, new()
    {
        public ModelCollection() : base()
        {
            _parallel = new ModelCollectionParallel<TData>(this);
        }
        
        private ModelCollectionParallel<TData> _parallel;
        public ModelCollectionParallel<TData> Parallel => _parallel;
        
        protected override object GetKeyForItem(Model<TData> item)
        {
            return item.Key;
        }
        
        public Model<TData> Add(string key = null)
        {
            var model = key == null ? new Model<TData>() : new Model<TData>(default, key);
            Add(model);
            return model;
        }
        public Model<TData> Add(TData initialData, string key = null)
        {
            var model = new Model<TData>(initialData, key);
            Add(model);
            return model;
        }

        public Model<TData> Get(string key)
        {
            return this.SingleOrDefault(m => (string)m.Key == key);
        }

        public Model<TData> Get(int index)
        {
            return this.ElementAt(index);
        }

        public ModelCollection<TData> Set<TValue>(
            Expression<Func<TData, TValue>> propertyAccessorFunc,
            TValue value,
            bool isTriggerChangedEvent = true,
            bool isTriggerAutoSave = true
        )
        {
            var chain = Expressions.GetExpressionChain(propertyAccessorFunc);
            
            // Parallel processing doesn't seem to speed anything up here,
            // even when we have upwards of 5000000 models.

            for (var i = 0; i < Count; i++)
            {
                this.ElementAt(i).Set(chain, value, isTriggerChangedEvent, isTriggerAutoSave);
            }

            return this;
        }
    }
}