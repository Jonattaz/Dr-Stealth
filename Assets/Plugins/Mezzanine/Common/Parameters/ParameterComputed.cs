/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;

namespace Mz.Parameters {
    public class ParameterComputed : Parameter {
        public ParameterComputed(
            IParameterFactory factory,
            Func<object, object> getValue, 
            object key = null, 
            object initialValue = null, 
            string label = null
        ) 
            : base(factory, key, null, label) {
            _value = initialValue;
            _getValue = getValue;
        }

        private Func<object, object> _getValue;
        private object _value;

        protected override object _GetValue()
        {
            var valuePrevious = _value;
            var value = _getValue(valuePrevious);
            _SetValue(value);
            return value;
        }

        protected override void _SetValue(object value)
        {
            _value = value;
        }
    }
}
