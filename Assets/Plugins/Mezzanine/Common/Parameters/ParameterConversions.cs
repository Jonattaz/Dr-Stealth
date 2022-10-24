/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;

namespace Mz.Parameters {
    public partial class Parameter {
        
        // Conversion from Parameter to int
        public static implicit operator int(Parameter parameter) {
            return parameter.TypeGeneral == ParameterTypeGeneral.Number ? Convert.ToInt32(parameter.Value) : 0;
        }

        // Conversion from Parameter to double
        public static implicit operator double(Parameter parameter) {
            return parameter.TypeGeneral == ParameterTypeGeneral.Number ? Convert.ToDouble(parameter.Value) : 0.0;
        }

        // Conversion from Parameter to float
        public static implicit operator float(Parameter parameter) {
            return parameter.TypeGeneral == ParameterTypeGeneral.Number ? (float)Convert.ToDouble(parameter.Value) : 0.0f;
        }

        // Conversion from Parameter to string
        public static implicit operator string(Parameter parameter) {
            return parameter.TypeGeneral == ParameterTypeGeneral.String ? Convert.ToString(parameter.Value) : "";
        }
        
        // Conversion from Parameter to bool
        public static implicit operator bool(Parameter parameter) {
            return parameter.TypeGeneral == ParameterTypeGeneral.Boolean && Convert.ToBoolean(parameter.Value);
        }
        
        // Conversion from int to Parameter
        public static implicit operator Parameter(int value) { return Create.Instance(null, value); }
        
        // Conversion from double to Parameter
        public static implicit operator Parameter(double value) { return Create.Instance(null, value); }
        
        // Conversion from double[] to Parameter
        public static implicit operator Parameter(double[] value) { return Create.Instance(null, value); }
        
        // Conversion from float to Parameter
        public static implicit operator Parameter(float value) { return Create.Instance(null, value); }
        
        // Conversion from string to Parameter
        public static implicit operator Parameter(string value) { return Create.Instance(null, value); }
        
        // Conversion from bool to Parameter
        public static implicit operator Parameter(bool value) { return Create.Instance(null, value); }
    }
}
