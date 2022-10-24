/*
 * Copyright (c) 2019 Victor Russ, victor@mezz.tech
 * 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at https://mozilla.org/MPL/2.0/.
 * */

using System;
using Mz.Numerics;

namespace Mz.Parameters {
    public enum ParameterType {
        None,
        Array,
        Base64Binary,
        Boolean,
        Byte,
        Char,
        Collection,
        Color,
        DateTime,
        Decimal,
        Double,
        DoubleArray,
        Float,
        Html,
        Int,
        Json,
        Long,
        Metric,
        Number,
        Object,
        Quaternion,
        Rectangle,
        Regex,
        Size,
        String,
        Svg,
        Vector,
        Xml
    }
    
    public enum ParameterTypeGeneral {
        None,
        Boolean,
        Collection,
        DoubleArray,
        Metric,
        Number,
        Object,
        String
    }

    public class ParameterTypes {
        public static string ParameterTypeToString(ParameterType type) {
            switch (type) {
                case ParameterType.Array:
                    return "array";
                case ParameterType.Base64Binary:
                    return "base64binary";
                case ParameterType.Boolean:
                    return "bool";
                case ParameterType.Byte:
                    return "byte";
                case ParameterType.Char:
                    return "char";
                case ParameterType.Collection:
                    return "collection";
                case ParameterType.Color:
                    return "color";
                case ParameterType.DateTime:
                    return "datetime";
                case ParameterType.Decimal:
                    return "decimal";
                case ParameterType.Double:
                    return "double";
                case ParameterType.Float:
                    return "float";
                case ParameterType.Html:
                    return "html";
                case ParameterType.Int:
                    return "int";
                case ParameterType.Json:
                    return "json";
                case ParameterType.Long:
                    return "long";
                case ParameterType.Number:
                    return "number";
                case ParameterType.Object:
                    return "object";
                case ParameterType.Quaternion:
                    return "quaternion";
                case ParameterType.Rectangle:
                    return "rectangle";
                case ParameterType.Regex:
                    return "regex";
                case ParameterType.Size:
                    return "size";
                case ParameterType.String:
                    return "string";
                case ParameterType.Svg:
                    return "svg";
                case ParameterType.Vector:
                    return "mzVector";
                case ParameterType.Xml:
                    return "xml";
                default:
                    return "none";
            }
        }

        public static Type StringToType(string type) {
            switch (type.ToLower()) {
                case "none":
                case "null":
                    return null;
                case "array":
                    return typeof(Array);
                case "base64binary":
                    return typeof(byte[]);
                case "bool":
                case "boolean":
                    return typeof(bool);
                case "byte":
                    return typeof(byte);
                case "char":
                    return typeof(char);
                case "collection":
                    return typeof(IParameterCollection);
                case "color":
                    return typeof(MzColor);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(decimal);
                case "double":
                case "number":
                    return typeof(double);
                case "float":
                case "single":
                    return typeof(float);
                case "int":
                case "integer":
                    return typeof(int);
                case "long":
                    return typeof(long);
                case "object":
                    return typeof(object);
                case "quaternion":
                    return typeof(MzQuaternion);
                case "rectangle":
                    return typeof(MzRectangle);
                case "size":
                    return typeof(MzSize);
                case "mzVector":
                    return typeof(MzVector);
                case "string":
                case "html":
                case "json":
                case "xml":
                case "svg":
                case "regex":
                    return typeof(string);
                default:
                    return null;
            }
        }

        public static ParameterType StringToParameterType(string type) {
            switch (type.ToLower()) {
                case "none":
                case "null":
                    return ParameterType.None;
                case "array":
                    return ParameterType.Array;
                case "base64binary":
                    return ParameterType.Base64Binary;
                case "bool":
                case "boolean":
                    return ParameterType.Boolean;
                case "byte":
                    return ParameterType.Byte;
                case "char":
                    return ParameterType.Char;
                case "collection":
                    return ParameterType.Collection;
                case "color":
                    return ParameterType.Color;
                case "datetime":
                    return ParameterType.DateTime;
                case "decimal":
                    return ParameterType.Decimal;
                case "double":
                    return ParameterType.Double;
                case "float":
                case "single":
                    return ParameterType.Float;
                case "html":
                    return ParameterType.Html;
                case "int":
                case "integer":
                    return ParameterType.Int;
                case "json":
                    return ParameterType.Json;
                case "long":
                    return ParameterType.Long;
                case "number":
                    return ParameterType.Number;
                case "object":
                    return ParameterType.Object;
                case "quaternion":
                    return ParameterType.Quaternion;
                case "rectangle":
                    return ParameterType.Rectangle;
                case "regex":
                    return ParameterType.Regex;
                case "size":
                    return ParameterType.Size;
                case "string":
                    return ParameterType.String;
                case "svg":
                    return ParameterType.Svg;
                case "mzVector":
                    return ParameterType.Vector;
                case "xml":
                    return ParameterType.Xml;
                default:
                    return ParameterType.None;
            }
        }
    } 
}
