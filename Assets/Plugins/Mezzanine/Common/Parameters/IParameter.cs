/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using System.Collections.Generic;

namespace Mz.Parameters {
    public interface IParameter : IComparable
    {
        //===== Properties
        
        object Key { get; }
        string Label { get; set; }
        bool IsRoot { get; }
        IParameter Root { get; }
        IParameter Parent { get; }
        string Path { get; }
        object Value { get; set; }
        bool IsEmpty { get; }
        bool IsCollection { get; }
        IParameterCollection Children { get; set; }
        ParameterType Type { get; set; }
        ParameterTypeGeneral TypeGeneral { get; }
        string TypeAsString { get; }
        
        /// <summary>
        /// Returns -1 if the item is not found in the parent collection.
        /// </summary>
        int Index { get; set; }

        //===== Events
        
        event Parameter.ValueChangedEventHandler ValueChanged;
        event Parameter.AttachedEventHandler Attached;
        event Parameter.DetachedEventHandler Detached;
        
        //===== Methods

        void SetParent(IParameter value);
        void DoAttached();
        
        TValue GetValue<TValue>();
        string ToString();

        IParameter Evaluate(Stack<IParameter> expressionStack);

        void DoSubValueChanged(IParameter parameter, object valuePrevious);
        
        //===== Activation
        
        bool IsActive { get; }
        IParameter Previous { get; }

        void Activate(IParameter previous, object activationValue = null);
        void Deactivate(IParameter activated, object deactivationValue = null);

        void Update(float deltaTime = 0);
    } 
}
