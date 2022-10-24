/*
 * Based on DeJson, copyright 2014, by Gregg Tavares.
 */

using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;
using Mz.TypeTools;

namespace Mz.Serializers {
    public class SerializerJson {
        private SerializerJson(bool isIncludeTypeInfoForDerivedTypes, bool isPrettyPrint, bool isIncludePrivateFields) {
            _builder = new StringBuilder();
            _isIncludeTypeInfoForDerivedTypes = isIncludeTypeInfoForDerivedTypes;
            _isPrettyPrint = isPrettyPrint;
            _prefix = "";

            _fieldFlags =
                BindingFlags.Instance |
                BindingFlags.Public |
                (isIncludePrivateFields ? BindingFlags.NonPublic : 0);
        }

        //===== Static

        public static string Serialize(
            object value,
            bool isIncludeTypeInfoForDerivedTypes = false,
            bool isPrettyPrint = false,
            bool isIncludePrivateFields = false
        ) {
            var serializer = new SerializerJson(isIncludeTypeInfoForDerivedTypes, isPrettyPrint, isIncludePrivateFields);
            return serializer.Run(value);
        }

       //===== Public

        public string Run(object value) {
            _builder.Clear();
            _SerializeValue(value);
            return _builder.ToString();
        }

        //===== Private

        private StringBuilder _builder;
        private readonly bool _isIncludeTypeInfoForDerivedTypes;
        private readonly bool _isPrettyPrint;
        private readonly BindingFlags _fieldFlags;
        private string _prefix;

        private void _Indent() { if (_isPrettyPrint) _prefix = _prefix + "  "; }
        private void _Outdent() { if (_isPrettyPrint) _prefix = _prefix.Substring(2); }
        private void _AddIndent() { if (_isPrettyPrint) _builder.Append(_prefix); }
        private void _AddLine() { if (_isPrettyPrint) _builder.Append("\n"); }
        private void _AddSpace() { if (_isPrettyPrint) _builder.Append(" "); }

        private void _SerializeValue(object value) {
            if (value == null) {
                _builder.Append("undefined");
                return;
            }

            var type = value.GetType();

            if (type.IsArray) {
                var rank = type.GetArrayRank();
                if (rank > 1) _SerializeMultidimensionalArray(value);
                else _SerializeArray(value);
            } else if (Types.IsGenericList(type)) {
                var elementType = type.GetGenericArguments()[0];
                var castMethod = typeof(Enumerable).GetMethod("Cast")?.MakeGenericMethod(elementType);
                var toArrayMethod = typeof(Enumerable).GetMethod("ToArray")?.MakeGenericMethod(elementType);
                var castObjectEnum = castMethod?.Invoke(null, new [] { value });
                var castObject = toArrayMethod?.Invoke(null, new [] { castObjectEnum });
                _SerializeArray(castObject);
            } else if (type.IsEnum) {
                _SerializeString(value.ToString());
            } else if (type == typeof(string)) {
                _SerializeString(value as string);
            } else if (type == typeof(char)) {
                _SerializeString(value.ToString());
            } else if (type == typeof(bool)) {
                _builder.Append((bool)value ? "true" : "false");
            } else if (type == typeof(bool)) {
                _builder.Append((bool)value ? "true" : "false");
                _builder.Append(Convert.ChangeType(value, typeof(string)));
            } else if (type == typeof(int)) {
                _builder.Append(value);
            } else if (type == typeof(byte)) {
                _builder.Append(value);
            } else if (type == typeof(sbyte)) {
                _builder.Append(value);
            } else if (type == typeof(short)) {
                _builder.Append(value);
            } else if (type == typeof(ushort)) {
                _builder.Append(value);
            } else if (type == typeof(int)) {
                _builder.Append(value);
            } else if (type == typeof(uint)) {
                _builder.Append(value);
            } else if (type == typeof(long)) {
                _builder.Append(value);
            } else if (type == typeof(ulong)) {
                _builder.Append(value);
            } else if (type == typeof(float)) {
                _builder.Append(((float)value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            } else if (type == typeof(double)) {
                _builder.Append(((double)value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            } else if (type == typeof(float)) {
                _builder.Append(((float)value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            } else if (type == typeof(double)) {
                _builder.Append(((double)value).ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            } else if (type.IsValueType) {
                _SerializeObject(value);
            } else if (type.IsClass) {
                _SerializeObject(value);
            } else {
                throw new InvalidOperationException("Unsupported type: " + type.Name);
            }
        }

        private void _SerializeArray(object value) {
            _builder.Append("[");
            _AddLine();
            _Indent();
            var array = value as Array;
            var isFirst = true;
            foreach (var element in array) {
                if (!isFirst) {
                    _builder.Append(",");
                    _AddLine();
                }
                _AddIndent();
                _SerializeValue(element);
                isFirst = false;
            }
            _AddLine();
            _Outdent();
            _AddIndent();
            _builder.Append("]");
        }
        
        private void _SerializeMultidimensionalArray(object value)
        {
            var jaggedArray = TypeConverter.ToJaggedArray(value);
            _SerializeArray(jaggedArray);
        }

        private void _SerializeDictionary(IDictionary value) {
            var isFirst = true;
            foreach (var key in value.Keys) {
                if (!isFirst) {
                    _builder.Append(',');
                    _AddLine();
                }

                _AddIndent();
                _SerializeString(key.ToString());
                _builder.Append(':');
                _AddSpace();

                _SerializeValue(value[key]);

                isFirst = false;
            }
        }

        private void _SerializeObject(object value) {
            _builder.Append("{");
            _AddLine();
            _Indent();
            var isFirst = true;
            if (_isIncludeTypeInfoForDerivedTypes) {
                // Only include type info for derived types.
                var type = value.GetType();
                var baseType = type.BaseType;
                if (baseType != null && baseType != typeof(object)) {
                    _AddIndent();
                    _SerializeString("classType"); // assuming this won't clash with user's properties.
                    _builder.Append(":");
                    _AddSpace();
                    _SerializeString(type.AssemblyQualifiedName);
                }
            }

            IDictionary asDictionary;
            if ((asDictionary = value as IDictionary) != null) {
                _SerializeDictionary(asDictionary);
            } else {
                // Don't grab indexed properties (list and array items). 
                var properties = value.GetType().GetProperties().Where(p => p.GetIndexParameters().Length == 0);
                foreach (var info in properties) {
                    // According to the docs, this should weed out operator overloads.
                    if (info.IsSpecialName) continue;

                    // Ignore properties with no setters. These can't be deserialized and will raise errors.
                    if (!info.CanWrite) continue;

                    var propertyValue = info.GetValue(value);
                    if (propertyValue == null) continue;
                    if (!isFirst) {
                        _builder.Append(",");
                        _AddLine();
                    }
                    _AddIndent();
                    _SerializeString(info.Name);
                    _builder.Append(":");
                    _AddSpace();
                    _SerializeValue(propertyValue);
                    isFirst = false;
                }

                var fields = value.GetType().GetFields(_fieldFlags);
                foreach (var info in fields) {
                    if (info.IsStatic) continue;
              
                    var fieldValue = info.GetValue(value);
                    if (fieldValue == null) continue;
                    if (!isFirst) {
                        _builder.Append(",");
                        _AddLine();
                    }
                    _AddIndent();
                    _SerializeString(info.Name);
                    _builder.Append(":");
                    _AddSpace();
                    _SerializeValue(fieldValue);
                    isFirst = false;
                }
            }

            _AddLine();
            _Outdent();
            _AddIndent();
            _builder.Append("}");
        }

        private void _SerializeString(string value) {
            _builder.Append('\"');

            var charArray = value.ToCharArray();
            foreach (var character in charArray) {
                switch (character) {
                    case '"':
                        _builder.Append("\\\"");
                        break;
                    case '\\':
                        _builder.Append("\\\\");
                        break;
                    case '\b':
                        _builder.Append("\\b");
                        break;
                    case '\f':
                        _builder.Append("\\f");
                        break;
                    case '\n':
                        _builder.Append("\\n");
                        break;
                    case '\r':
                        _builder.Append("\\r");
                        break;
                    case '\t':
                        _builder.Append("\\t");
                        break;
                    default:
                        var codepoint = Convert.ToInt32(character);
                        if ((codepoint >= 32) && (codepoint <= 126)) {
                            _builder.Append(character);
                        } else {
                            _builder.Append("\\u");
                            _builder.Append(codepoint.ToString("x4"));
                        }
                        break;
                }
            }

            _builder.Append('\"');
        }
    } 
}
