/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;

namespace Mz.Parameters {
    public class ParameterRootOptions
    {
        public object Key { get; set; }
        public IParameterCollection Collection { get; set; }
        public string Label { get; set; }
    }
    
    public class ParameterInstanceOptions
    {
        public object Key { get; set; }
        public object Value { get; set; }
        public string Label { get; set; }
        public bool IsRoot { get; set; }
    }

    public class ParameterFactory : ParameterFactoryBase<Parameter, ParameterCollection>
    {
        public override Parameter Root(object key, object value)
        {
            return new Parameter(this, key, value, "root", true);
        }
        
        public Parameter Root(ParameterRootOptions options = null)
        {
            return new Parameter(this, options?.Key, options?.Collection, options?.Label, true);
        }
        
        public override Parameter Instance(object key = null, object value = null)
        {
            return new Parameter(this, key, value);
        }
        
        public override Parameter Computed(
            Func<object, object> getValue,
            object key = null,
            object initialValue = null
        )
        {
            return new ParameterComputed(this, getValue, key, initialValue);
        }
        
        public override ParameterCollection Collection()
        {
            return new ParameterCollection(this);
        }
    }
}
