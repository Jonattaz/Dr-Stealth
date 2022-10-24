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

namespace Mz.Parameters {
    public interface IParameterCollection : IEnumerable
    {
        //===== Properties
        
        SortedSet<IParameter> Collection { get; }
        IParameter Parent { get; }
        
        //===== Events
        
        event Parameter.CollectionAttachedEventHandler Attached;
        event Parameter.CollectionDetachedEventHandler Detached;
        
        //===== Methods

        IParameter Add(IParameter parameter);
        IParameter Add(object keyOrPath = null, object value = null);
        IParameter Remove(IParameter parameter);
        IParameter Remove(object key);
        
        TParameter Get<TParameter>(object keyOrPath, bool isCreate = true, object value = null, Action<TParameter, int, int> action = null) where TParameter : class, IParameter;
        TParameter GetByKey<TParameter>(object key, bool isCreate = true, object value = null) where TParameter : class, IParameter;

        object ValueOf(object key);
        TValue ValueOf<TValue>(object key);

        List<IParameter> ForEach(
            Func<IParameter, int, int, bool> action,
            bool isBreakOnFalse = false,
            bool isRecursive = true
        );

        //----- For internal use, but must be included in interface

        void SetParent(IParameter value);

        //===== Operators
        IParameter this[int index] { get; }
        IParameter this[object key] { get; }
        
        //===== Activation
        
        IParameter Current { get; }

        IParameter Activate(
            object keyOrPath,
            object activationValue = null,
            object deactivationValue = null
        );

        // Always use only this method to set the current tile.
        IParameter ActivateChild(
            IParameter parameter, 
            object activationValue = null,
            object deactivationValue = null
        );

        void DeactivateAll(IParameter activated, object deactivationValue);

        void UpdateAll(float deltaTime = 0);
    }
}
