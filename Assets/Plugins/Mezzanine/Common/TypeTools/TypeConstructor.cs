/*
 * Copyright (c) 2019 Victor Russ
 *
 * Originally based on parts of DeJson by Gregg Tavares.
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mz.TypeTools {
    public static class TypeConstructor {
        private static BindingFlags _fieldFlags;
        private static bool _isThrowErrors;

        public static object Construct(
            Type targetType,
            Dictionary<string, object> sourceDictionary,
            bool isIncludePrivateFields = false, 
            bool isThrowErrors = false
        ) {
            _fieldFlags = BindingFlags.Instance | BindingFlags.Public | (isIncludePrivateFields ? BindingFlags.NonPublic : 0);
            _isThrowErrors = isThrowErrors;
            return ConstructObject(targetType, sourceDictionary);
        }

        public static object ConstructObject(Type targetType, Dictionary<string, object> sourceDictionary)
        {
            var targetObject = Activator.CreateInstance(targetType);
            
            var fields = targetType.GetFields(_fieldFlags);
            var fieldDescriptions = new Dictionary<string, object>();
            foreach (var info in fields) {
                if (info.IsStatic) continue;
                object fieldValue;
                if (sourceDictionary.TryGetValue(info.Name, out fieldValue)) {
                    fieldDescriptions.Add(info.Name, fieldValue);
                }
            }

            _SetFields(targetType, targetObject, fieldDescriptions);

            var properties = targetType.GetProperties();
            var propertyDescriptions = new Dictionary<string, object>();
            foreach (var info in properties) {
                object propertyValue;
                if (sourceDictionary.TryGetValue(info.Name, out propertyValue)) {
                    propertyDescriptions.Add(info.Name, propertyValue);
                }
            }

            _SetProperties(targetType, targetObject, propertyDescriptions);

            return targetObject;
        }

        private static void _SetFields(Type targetType, object targetObject, Dictionary<string, object> fieldsDescriptions) {
            foreach (var fieldsDescription in fieldsDescriptions) {
                _SetField(targetType, targetObject, fieldsDescription.Key, fieldsDescription.Value);
            }
        }

        private static void _SetField(Type targetType, object targetObject, string name, object value) {
            var info = targetType.GetField(name, _fieldFlags);
            if (info == null) return;

            var fieldType = info.FieldType;
            value = TypeConverter.To(value, fieldType, _isThrowErrors);
            if (fieldType.IsInstanceOfType(value)) {
                info.SetValue(targetObject, value);
            }
        }

        private static void _SetProperties(Type targetType, object targetObject, Dictionary<string, object> propertyDescriptions) {
            foreach (KeyValuePair<string, object> propertyDescription in propertyDescriptions) {
                _SetProperty(targetType, targetObject, propertyDescription.Key, propertyDescription.Value);
            }
        }

        private static void _SetProperty(Type targetType, object targetObject, string name, object value) {
            var info = targetType.GetProperty(name);
            if (info == null) return;

            var propertyType = info.PropertyType;
            value = TypeConverter.To(value, propertyType, _isThrowErrors);
            if (propertyType.IsInstanceOfType(value)) {
                info.SetValue(targetObject, value);
            }
        }
    } 
}
