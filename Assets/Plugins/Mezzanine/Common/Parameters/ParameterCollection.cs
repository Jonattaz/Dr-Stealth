/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mz.Parameters {
    public class ParameterCollection : IParameterCollection
    {
        //===== Construction
        #region Construction

        public ParameterCollection(IParameterFactory factory) {
            Create = factory;
            Collection = new SortedSet<IParameter>();
        }

        #endregion

        //===== Properties
        #region Properties

        public IParameterFactory Create { get; protected set; }
        public SortedSet<IParameter> Collection { get; protected set; }
        public IEnumerator GetEnumerator() { return Collection.GetEnumerator(); }
        public IParameter Parent { get; private set; }

        #endregion
        
        //===== Events
        #region Events
        
        public event Parameter.CollectionAttachedEventHandler Attached;
        public void DoAttached() {
            ForEach((parameter, index, count) => {
                parameter.DoAttached();
                return true;
            }, false, false);
            
            Attached?.Invoke(this);
        }
        
        public event Parameter.CollectionDetachedEventHandler Detached;
        internal void DoDetached(IParameter parentPrevious) {
            Detached?.Invoke(this, parentPrevious);
        }
        
        #endregion

        //===== Public Methods
        #region Public Methods

        public void SetParent(IParameter value) {
            // Don't attach to more than one parent.
            if (value != null && Parent != null) return;
            
            var parentPrevious = Parent;
            var rootPrevious = Parent?.Root;
            Parent = value;
            var root = value?.Root;

            ForEach((parameter, index, count) => {
                parameter.SetParent(value);
                return true;
            }, false, false);
            
            if (root != null && root != rootPrevious) DoAttached();
            else if (root == null && rootPrevious != null) DoDetached(parentPrevious);
        }

        public virtual IParameter Add(IParameter parameter) {
            if (parameter == null) return null;
            
            // Make sure we only add one parameter per key.
            // This will return null if the parameter doesn't already exist in the collection.
            var child = Collection.SingleOrDefault(p => p?.Key.ToString() == parameter.Key.ToString());
            if (child != null) return child;
            
            // Don't attach to more than one parent.
            if (parameter.Parent != null) {
                // Don't add if it's already a child.
                if (parameter.Parent == Parent) return parameter;
                parameter.Parent.Children.Remove(parameter);
            }

            parameter.Index = Collection.Count;
            Collection.Add(parameter);
            if (Parent != null) parameter.SetParent(Parent);
            return parameter;
        }

        public virtual IParameter Add(object keyOrPath, object value = null) {
            return Get<IParameter>(keyOrPath, true, value);
        }

        public IParameter Remove(IParameter parameter) {
            Collection.Remove(parameter);
            parameter.Index = -1;
            if (Parent != null) parameter.SetParent(null);
            return parameter;
        }

        public void Clear()
        {
            foreach (var parameter in Collection) Remove(parameter);
        }
        
        public IParameter Remove(object key) {
            var parameter = Get<IParameter>(key);
            return Remove(parameter);
        }

        public TParameter GetByKey<TParameter>(object key, bool isCreate = true, object value = null)
            where TParameter : class, IParameter
        {
            var child = Collection.SingleOrDefault(
                p => p != null && p.Key != null && (string) key != "" && p.Key.ToString() == key.ToString()
            );

            if (child != null) return (TParameter)child;
            if (!isCreate) return (TParameter)Create.ParameterEmpty();
            
            child = Create?.ParameterInstance(key, value ?? Create.ParameterCollection());
            if (child == null) return default;
            Add(child);

            return (TParameter)child;
        }

        public TParameter GetByIndex<TParameter>(int index)
            where TParameter : class, IParameter
        {
            if (index >= Collection.Count) return (TParameter)Create.ParameterEmpty();
            return (TParameter)Collection.ElementAt(index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyOrPath">
        /// An identifying object, such as a GUID or string, or a path string
        /// using the appropriate delimiter (Create.PathDelimiter) to separate segments
        /// </param>
        /// <param name="isCreate"></param>
        /// <param name="value"></param>
        /// <param name="label"></param>
        /// <param name="action">Of the form: (parameter, index, pathSegmentCount) => void</param>
        /// <returns></returns>
        public TParameter Get<TParameter>(
            object keyOrPath, 
            bool isCreate = true, 
            object value = null,
            Action<TParameter, int, int> action = null
        )
            where TParameter : class, IParameter
        {
            if (string.IsNullOrEmpty(Convert.ToString(keyOrPath))) return (TParameter)Create.ParameterEmpty();
   
            var segments = keyOrPath is string path ? path.Split(Create.PathDelimiter) : new [] { keyOrPath };
            if (segments.Length < 1) return (TParameter)Create.ParameterEmpty();
            
            // Do we have a single key value?
            var key = segments[segments.Length - 1];
            if (segments.Length == 1) return GetByKey<TParameter>(key, true, value ?? Create.ParameterCollection());

            // We have a path with multiple segment strings.
            IParameterCollection collection = this;
            TParameter child = default;
            
            for (var i = 0; i < segments.Length; i++) {
                if (collection == null) break;
                var segment = segments[i];

                if (segment == key)
                {
                    child = collection.GetByKey<TParameter>(segment, true, value);
                }
                else
                {
                    child = collection.GetByKey<TParameter>(segment);
                    if (child != null && !child.IsCollection) child.Value = Create.ParameterCollection();
                }
                
                collection = child?.Type == ParameterType.Collection ? child.GetValue<IParameterCollection>() : null;
                
                action?.Invoke(child, i, segments.Length);
            }

            return child ?? (TParameter)Create.ParameterEmpty();
        }

        public Parameter Get(
            object keyOrPath, 
            bool isCreate = true, 
            object value = null,
            Action<Parameter, int, int> action = null
        ) => Get<Parameter>(keyOrPath, isCreate, value, action);

        public object ValueOf(object key) { return Get<IParameter>(key)?.Value; }
        public TValue ValueOf<TValue>(object key) { return (TValue)Get<IParameter>(key)?.Value; }

        public List<IParameter> ForEach(
            Func<IParameter, int, int, bool> action, 
            bool isBreakOnFalse = false, 
            bool isRecursive = true
        ) {
            var list = new List<IParameter>();

            var index = 0;
            var count = Collection.Count;
            foreach (var parameter in Collection) {
                if (isRecursive && !parameter.IsEmpty && parameter.IsCollection) {
                    var collection = parameter.GetValue<IParameterCollection>();
                    collection?.ForEach(action, isBreakOnFalse, isRecursive);
                }

                if (action(parameter, index, count)) list.Add(parameter);
                else if (isBreakOnFalse) break;

                index++;
            }

            return list;
        }

        #endregion

        //===== Operators
        #region Operators

        public IParameter this[int index] => Collection.ToList()[index];
        public IParameter this[object key] => Get<IParameter>(key);

        public override string ToString() {
            var text = @"{ ";

            var index = 0;
            foreach(var parameter in Collection) {
                text += parameter.ToString();
                if (index + 1 < Collection.Count) text += @", ";
                index++;
            }

            text += @" }";
            return text;
        }

        #endregion
        
        //===== Activation
        #region Activation
        
        public IParameter Current { get; protected set; }
        
        public IParameter Activate(
            object keyOrPath,
            object activationValue = null,
            object deactivationValue = null
        )
        {
            if (string.IsNullOrEmpty(Convert.ToString(keyOrPath))) return Current;
            var activated = Get<IParameter>(keyOrPath, true, false, (s, i, length) =>
            {
                if (i < length - 1)
                {
                    s?.Parent?.Children?.ActivateChild(s, activationValue, deactivationValue);
                }
            });
            var collection = activated?.Parent?.Children;
            return collection?.ActivateChild(activated, activationValue, deactivationValue);
        }

        // Always use only this method to set the current tile.
        public IParameter ActivateChild(IParameter tile, object activationValue = null, object deactivationValue = null)
        {
            var tilePrevious = Current;
            Current = tile != null && tile is IParameter tileCast ? tileCast : null;
            tilePrevious?.Deactivate(Current, deactivationValue);
            Current?.Activate(tilePrevious, activationValue);

            return Current;
        }

        public void DeactivateAll(IParameter activated, object deactivationValue)
        {
            foreach (var tile in Collection)
            {
                tile?.Deactivate(activated, deactivationValue);
            }
        }
        
        public void UpdateAll(float deltaTime = 0)
        {
            foreach (var tile in Collection)
            {
                tile?.Update(deltaTime);
            }
        }
        
        #endregion
    }
}
