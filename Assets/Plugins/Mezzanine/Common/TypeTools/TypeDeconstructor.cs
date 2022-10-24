/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Mz.TypeTools {
    public static class TypeDeconstructor {
        private static BindingFlags _fieldFlags;

        public static Dictionary<string, object> Deconstruct(object sourceObject, bool isIncludePrivateFields = false, bool isThrowErrors = false) {
            _fieldFlags = BindingFlags.Instance | BindingFlags.Public | (isIncludePrivateFields ? BindingFlags.NonPublic : 0);
            return _DeconstructObject(sourceObject);
        }

        private static Dictionary<string, object> _DeconstructObject(object sourceObject) {
            var targetDictionary = new Dictionary<string, object>();
            var sourceType = sourceObject.GetType();
            if (!sourceType.IsClass && !sourceType.IsStruct()) return targetDictionary;
            var fields = sourceType.GetFields(_fieldFlags);
            _DeconstructFields(sourceObject, targetDictionary, fields);
            
            // Don't grab indexed properties (list and array items). We'll pick those up later.
            var properties = sourceType.GetProperties().Where(p => p.GetIndexParameters().Length == 0);
            _DeconstructProperties(sourceObject, targetDictionary, properties.ToArray());
       
            return targetDictionary;
        }

        private static void _DeconstructFields(
            object sourceObject,
            Dictionary<string, object> targetDictionary,
            FieldInfo[] fields
        ) {
            foreach (var info in fields) {
                if (info.IsStatic) continue;
                var fieldValue = info.GetValue(sourceObject);
                if (fieldValue != null) {
                    _DeconstructField(targetDictionary, info.Name, fieldValue);
                }
            }
        }

        private static void _DeconstructField(
            Dictionary<string, object> targetDictionary, 
            string name, 
            object value
        ) {
            value = _DeconstructValue(value);
            targetDictionary.Add(name, value);
        }

        private static void _DeconstructProperties(
            object sourceObject,
            Dictionary<string, object> targetDictionary, 
            PropertyInfo[] properties
        ) {
            if (sourceObject == null) return;
            foreach (var info in properties) {
                if (!info.CanWrite) continue;
                var propertyValue = info.GetValue(sourceObject);
                if (propertyValue != null) {
                    _DeconstructProperty(targetDictionary, info.Name, propertyValue);
                }
            }
        }

        private static void _DeconstructProperty(
            Dictionary<string, object> targetDictionary, 
            string name, 
            object value
        ) {
            value = _DeconstructValue(value);
            targetDictionary.Add(name, value);
        }

        private static object _DeconstructValue(object value) {
            var valueType = value.GetType();

            if (valueType == typeof(string)) {
                // Get this out of the way, since strings are also IEnumerable and Class types
                return value;
            } 
            
            if (Types.IsEnumerable(valueType)) {
                return (from object item in (IEnumerable) value select _DeconstructValue(item)).ToArray();
            }

            // Otherwise, it's a primitive value type, so just return it.
            if (!valueType.IsClass && !valueType.IsStruct()) return value;
            
            // If the object is a user-defined class or struct, break it down into a dictionary.
            var dictionary = _DeconstructObject(value);
            return dictionary;
        }
    } // End class
}
