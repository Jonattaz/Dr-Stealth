/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;
using System.IO;

namespace Mz.Parameters {
    public abstract class ParameterFactoryBase<TParameter, TParameterCollection> : IParameterFactory
        where TParameter : Parameter
        where TParameterCollection : ParameterCollection
    {
        public char PathDelimiter { get; set; } = '.';

        public abstract TParameter Root(object key, object value);

        public IParameter ParameterEmpty() => Instance();
        public IParameter ParameterInstance(object key, object value) => Instance(key, value);
        
        public abstract TParameter Instance(object key = null, object value = null);
        
        public TParameter FromKeyOrPath(
            object keyOrPath,
            object value = null, 
            string label = null,
            Action<TParameter> action = null
        ) {
            if (string.IsNullOrEmpty(Convert.ToString(keyOrPath))) return Instance();
            
            var segments = keyOrPath is string path ? path.Split(PathDelimiter) : new [] { keyOrPath };
            if (segments.Length < 1) return Instance();
            
            var collection = Collection();
            var key = segments[segments.Length - 1];
            var parent = Instance(segments[0], null);
        
            if (segments.Length == 1)
            {
                parent.Value = value;
                parent.Label = label;
                return parent;
            }
        
            parent.Value = collection;
            var child = Instance(key, value);
            
            for (var i = 1; i < segments.Length; i++) {
                if (collection == null) break;
                var segment = segments[i];
        
                if (segment == key)
                {
                    child = collection.GetByKey<TParameter>(segment, true, value);
                }
                else
                {
                    var childCollection = Collection();
                    child = collection.GetByKey<TParameter>(segment, true, childCollection);
                }
                
                collection = child?.Type == ParameterType.Collection ? child?.GetValue<TParameterCollection>() : null;
        
                action?.Invoke(child);
            }
        
            return parent;
        }

        public IParameter ParameterComputed(
            Func<object, object> getValue,
            object key = null,
            object initialValue = null
        ) => Computed(getValue, key, initialValue);

        public abstract TParameter Computed(
            Func<object, object> getValue,
            object key = null,
            object initialValue = null
        );

        public IParameterCollection ParameterCollection() => Collection();
        public abstract TParameterCollection Collection();

        public IParameterCollection ParameterLoadCollection(TextReader reader) => LoadCollection(reader);
        public TParameterCollection LoadCollection(TextReader reader) {
            var parser = new Parser(this);
            return (TParameterCollection)parser.Parse(reader);
        }
        
        //===== Evaluators

        public virtual IParameter Add(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Add(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter Subtract(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Subtract(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter Multiply(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Multiply(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter Divide(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Divide(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter Equal(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Equal(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter NotEqual(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.NotEqual(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter GreaterThan(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.GreaterThan(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter GreaterThanOrEqual(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.GreaterThanOrEqual(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter LessThan(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.LessThan(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter LessThanOrEqual(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.LessThanOrEqual(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter AndAlso(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.AndAlso(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter OrElse(TParameter left, TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.OrElse(this, left, right);
            return evaluator.Evaluate(expressionStack);
        }
        
        public virtual IParameter Negate(TParameter right, Stack<IParameter> expressionStack = null)
        {
            var evaluator = new Evaluators.Negate(this, right);
            return evaluator.Evaluate(expressionStack);
        }
    }
}
