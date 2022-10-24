/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mz.Identifiers;
using Mz.Numerics;

namespace Mz.Parameters {
    public partial class Parameter : IParameter
    {
        //===== Construction
        #region Construction

        public Parameter() {}
        
        public Parameter(
            IParameterFactory factory,
            object key = null, 
            object value = null, 
            string label = null,
            bool isRoot = false
        ) {
            _create = factory;
            Key = key ?? Identifier.Get();
            Value = value;
            _label = !string.IsNullOrEmpty(label) ? label : key?.ToString();
            IsRoot = isRoot;
        }

        #endregion
        
        //===== Properties
        #region Properties

        protected readonly IParameterFactory _create;

        public object Key { get; }

        private string _label;
        public string Label
        {
            get => !string.IsNullOrEmpty(_label) ? _label : Key?.ToString();
            set => _label = value;
        }
        public bool IsRoot { get; private set; }
        
        /// <summary>
        /// Returns -1 if the item is not found in the parent collection.
        /// </summary>
        public int Index { get; set; }

        public IParameter Root {
            get {
                if (IsRoot) return this;
                
                var parent = Parent;

                while (parent != null && parent.Parent != null) {
                    parent = parent.Parent;
                }

                return parent != null && parent.IsRoot ? parent : null;
            }
        }

        public IParameter Parent { get; private set; }

        public void SetParent(IParameter value) {
            // Don't attach to more than one parent
            if (value != null && Parent != null) return;
            
            var parentPrevious = Parent;
            Parent = value;
            var root = Parent?.Root;

            if (Parent != null) {
                if (root != null) DoAttached();
                IsRoot = false;
            }
            else {
                DoDetached(parentPrevious);
            }
        }

        public string Path {
            get {
                var path = "";
                var pathSegmentDelimiter = _create.PathDelimiter;
                var parameter = Parent;

                while (parameter != null) {
                    path = pathSegmentDelimiter + parameter.Key.ToString() + path;
                    parameter = parameter.Parent;
                }

                return path;
            }
        }

        private object _value;
        public object Value {
            get => _GetValue();
            set => _SetValue(value);
        }
        
        public TValue GetValue<TValue>() 
        {
            return (TValue) _GetValue();
        }

        public virtual IParameter Evaluate(Stack<IParameter> expressionStack)
        {
            return _create.ParameterInstance(Key, Value);
        }

        protected virtual object _GetValue() => _value;

        protected virtual void _SetValue(object value)
        {
            var valuePrevious = _value;
            if (valuePrevious != null && valuePrevious is IParameterCollection parameterPrevious) parameterPrevious.SetParent(null);
            _value = value;
            if (_value != null && _value is IParameterCollection parameter) parameter.SetParent((IParameter)this);
            DoValueChanged(this, valuePrevious);
        }

        public bool IsEmpty =>
            Value == null ||
            Type == ParameterType.None ||
            TypeGeneral == ParameterTypeGeneral.Number && double.IsNaN(Convert.ToDouble(Value)) ||
            Type == ParameterType.Int && GetValue<int>() == int.MinValue ||
            Type == ParameterType.Float && Numbers.Abs(GetValue<float>()) - Numbers.Abs(float.MinValue) < Numbers.Epsilon ||
            Type == ParameterType.Double && Numbers.Abs(GetValue<float>()) - Numbers.Abs(float.MinValue) < Numbers.Epsilon ||
            TypeGeneral == ParameterTypeGeneral.String && GetValue<string>() == "";

        public bool IsCollection => Value != null && IsCollectionType(Value.GetType());

        public IParameterCollection Children {
            get {
                IParameterCollection collection;

                if (IsCollection && !IsEmpty) {
                    collection = GetValue<IParameterCollection>();
                }
                else
                {
                    collection = _create.ParameterCollection();
                    Value = collection;
                }

                return collection;
            }

            set => Value = value;
        }

        public ParameterCollection Parameters => (ParameterCollection) Children;

        private ParameterType _type = ParameterType.None;
        public ParameterType Type { 
            get {
                if (_type != ParameterType.None) return _type;
                
                if (IsCollection) return ParameterType.Collection;
                if (Value == null) return ParameterType.None;

                var type = Value.GetType();
                if (type.IsArray && type != typeof(double[])) return ParameterType.Array;
                
                switch (Value) {
                    case bool _:
                        return ParameterType.Boolean;
                    case byte _:
                        return ParameterType.Byte;
                    case char _:
                        return ParameterType.Char;
                    case MzColor _:
                        return ParameterType.Color;
                    case DateTime _:
                        return ParameterType.DateTime;
                    case double _:
                        return ParameterType.Double;
                    case float _:
                        return ParameterType.Float;
                    case int _:
                        return ParameterType.Int;
                    case double[] _:
                        return ParameterType.DoubleArray;
                    case MzRectangle _:
                        return ParameterType.Rectangle;
                    case Regex _:
                        return ParameterType.Regex;
                    case MzQuaternion _:
                        return ParameterType.Quaternion;
                    case MzSize _:
                        return ParameterType.Size;
                    case string _:
                        return ParameterType.String;
                    case MzVector _:
                        return ParameterType.Vector;
                    default:
                        return ParameterType.Object;
                }
            }

            set => _type = value;
        }

        public ParameterTypeGeneral TypeGeneral => GetGeneralType(Type);
        public string TypeAsString => Type.ToString();

        #endregion
        
        //===== Events
        #region Events
        
        public event Parameter.ValueChangedEventHandler ValueChanged;
        public void DoValueChanged(IParameter parameter, object valuePrevious) {
            ValueChanged?.Invoke(parameter, valuePrevious);
            Parent?.DoSubValueChanged(parameter, valuePrevious);
        }
        
        public event Parameter.ValueChangedEventHandler SubValueChanged;
        public void DoSubValueChanged(IParameter parameter, object valuePrevious) {
            SubValueChanged?.Invoke(parameter, valuePrevious);
            Parent?.DoSubValueChanged(parameter, valuePrevious);
        }
        
        public event Parameter.AttachedEventHandler Attached;
        public void DoAttached() {
            Attached?.Invoke(this);
        }
        
        public event Parameter.DetachedEventHandler Detached;
        internal void DoDetached(IParameter parentPrevious) {
            Detached?.Invoke(this, parentPrevious);
        }

        #endregion

        //===== Public Methods
        #region Public Methods

        public override string ToString() {
            var text = $"\"{Key}\": ";

            if (IsCollection) {
                var collection = GetValue<IParameterCollection>();
                text += collection.ToString();
            } else if (Type == ParameterType.Array) {
                text += @"[";
                var value = GetValue<object[]>();
                for (var i = 0; i < value.Length; i++) {
                    text += value[i].ToString();
                    if (i + 1 < value.Length) text += @", ";
                }
                text += @"]";
            } else if (TypeGeneral == ParameterTypeGeneral.String) {
                text += $"\"{Value}\"";
            } else text += $"{Value}";

            return text;
        }
        
        // Implement IComparable, used for sorting
        public int CompareTo(object parameter)
        {
            switch (parameter) {
                case IParameter other when other.Index > Index:
                    return -1;
                case IParameter other when other.Index == Index:
                    return 0;
                case IParameter other when other.Index < Index:
                    return 1;
                default:
                    return -1;
            }
        }

        #endregion
        
        //===== Activation / Deactivation
        #region Activation
        
        public bool IsActive { get; protected set; }
        public IParameter Previous { get; protected set; }
        
        public virtual void Activate(IParameter previous, object activationValue = null)
        {
            if (IsActive) return;
        
            if (previous != null) Previous = previous;
            IsActive = true;
            
            OnActivated(previous, activationValue);
        }

        protected virtual void OnActivated(IParameter previous, object activationValue) {}

        public virtual void Deactivate(IParameter activated, object deactivationValue = null)
        {
            if (!IsActive) return;

            if (activated?.Key != Key)
            {
                Children?.DeactivateAll(activated, deactivationValue);
                OnDeactivated(activated, deactivationValue);
            }

            IsActive = false;
        }

        protected virtual void OnDeactivated(IParameter activated, object deactivationValue) {}
        
        public void Update(float deltaTime = 0)
        {
            if (!IsActive) return;
            Children?.UpdateAll(deltaTime);
            OnUpdated(deltaTime);
        }
        
        protected virtual void OnUpdated(float deltaTime = 0) {}
        
        // Needed to overload == operator
        public override bool Equals(object instance) {
            if (!(instance is Parameter)) return false;
            return Comparison(this, instance) == 0;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        
        #endregion
    }
}
