/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using Mz.TypeTools;

namespace Mz.Parameters {
    public partial class Parameter {
        private static ParameterFactory _factory;
        public static ParameterFactory Create => _factory ?? (_factory = new ParameterFactory());

        public static ParameterTypeGeneral GetGeneralType(ParameterType type) {
            switch (type) {
                case ParameterType.None:
                    return ParameterTypeGeneral.None;
                case ParameterType.Boolean:
                    return ParameterTypeGeneral.Boolean;
                case ParameterType.Array:
                case ParameterType.Collection:
                    return ParameterTypeGeneral.Collection;
                case ParameterType.Byte:
                case ParameterType.Decimal:
                case ParameterType.Double:
                case ParameterType.Float:
                case ParameterType.Int:
                case ParameterType.Long:
                case ParameterType.Number:
                    return ParameterTypeGeneral.Number;
                case ParameterType.Base64Binary:
                case ParameterType.Char:
                case ParameterType.DateTime:
                case ParameterType.Html:
                case ParameterType.Json:
                case ParameterType.Regex:
                case ParameterType.String:
                case ParameterType.Svg:
                case ParameterType.Xml:
                    return ParameterTypeGeneral.String;
                case ParameterType.Color:
                case ParameterType.Metric:
                case ParameterType.Quaternion:
                case ParameterType.Rectangle:
                case ParameterType.Size:
                case ParameterType.Vector:
                    return ParameterTypeGeneral.Metric;
                default:
                    return ParameterTypeGeneral.Object;
            }
        }

        public static int Comparison(object a, object b)
        {
            const double epsilon = 0.0001;
            var aType = a.GetType();
            var bType = b.GetType();

            // If both objects are empty Parameters, we can consider them to be equal.
            if (
                aType == typeof(IParameter) &&
                bType == typeof(IParameter) &&
                ((IParameter)a).IsEmpty &&
                ((IParameter)b).IsEmpty
            ) {
                return 0;
            }

            var aValue = aType == typeof(IParameter) ? ((IParameter)a).Value : a;
            var bValue = bType == typeof(IParameter) ? ((IParameter)a).Value : b;

            if (!aValue.IsNumber() || !bValue.IsNumber()) return 0;
            
            var aValueDouble = Convert.ToDouble(aValue);
            var bValueDouble = Convert.ToDouble(bValue);

            if (double.IsNaN(aValueDouble) || double.IsNaN(bValueDouble)) return 0;

            if (aValueDouble < bValueDouble) return -1;
            if (Math.Abs((float)(aValueDouble - bValueDouble)) < epsilon) return 0;
            return aValueDouble > bValueDouble ? 1 : 0;
        }

        /// <example>
        /// <code>
        /// bool isSubclass = Types.IsSubclassOfRawGeneric(typeof(ParameterCollection<>), parameters.GetType());
        /// </code>
        /// </example>
        public static bool IsCollectionType(Type toCheck) {
            var generic = typeof(ParameterCollection);
            while (toCheck != null && toCheck != typeof(object)) {
                var current = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (current == generic) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
