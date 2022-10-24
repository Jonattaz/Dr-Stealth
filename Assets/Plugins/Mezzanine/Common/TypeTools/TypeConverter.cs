/*
 * Copyright (c) 2019 Victor Russ
 *
 * Originally based on parts of DeJson by Gregg Tavares.
 * Also includes multidimensional array conversion routines from Newtonsoft.Json.
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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mz.TypeTools {
    public static class TypeConverter {
        
        public static TObject To<TObject>(object value, bool isThrowErrors = false) {
            return (TObject)_ToType(value, typeof(TObject), isThrowErrors);
        }
  
        public static object To(object value, Type targetType, bool isThrowErrors = false) {
            return _ToType(value, targetType, isThrowErrors);
        }

        //===== Private

        private static object _ToType(
            object value, 
            Type type, 
            bool isThrowErrors = false
        ) {
            try
            {
                if (type.IsArray)
                {
                    var rank = type.GetArrayRank();

                    return rank > 1 ? 
                        _ToMultiDimensionalArray(value, type, rank, isThrowErrors) : 
                        _ToArray(value, type, isThrowErrors);
                }
                
                if (Types.IsGenericList(type)) return _ToList(value, type, isThrowErrors);
                if (Types.IsGenericDictionary(type)) return _ToDictionary(value, type, isThrowErrors);
                if (type.IsEnum) return Enum.Parse(type,Convert.ToString(value));
                if (type == typeof(string)) return Convert.ToString(value);
                if (type == typeof(byte)) return Convert.ToByte(value);
                if (type == typeof(sbyte)) return Convert.ToSByte(value);
                if (type == typeof(short)) return Convert.ToInt16(value);
                if (type == typeof(ushort)) return Convert.ToUInt16(value); 
                if (type == typeof(int)) return Convert.ToInt32(value);
                if (type == typeof(uint)) return Convert.ToUInt32(value);
                if (type == typeof(long)) return Convert.ToInt64(value);
                if (type == typeof(ulong)) return Convert.ToUInt64(value);
                if (type == typeof(char)) return Convert.ToChar(value);
                if (type == typeof(double)) return Convert.ToDouble(value);
                if (type == typeof(float)) return Convert.ToSingle(value);
                if (type == typeof(int)) return Convert.ToInt32(value);
                if (type == typeof(float)) return Convert.ToSingle(value);
                if (type == typeof(double)) return Convert.ToDouble(value);
                if (type == typeof(bool)) return Convert.ToBoolean(value);
                if (type == typeof(bool)) return Convert.ToBoolean(value);
                if (type == typeof(object)) return value;
                
                if (type.IsClass || type.IsStruct()) {
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    
                    if (value.GetType() != typeof(Dictionary<string, object>)) {
                        dictionary = TypeDeconstructor.Deconstruct(value);
                    } else {
                        dictionary = (Dictionary<string, object>) value;
                    }
                    
                    return TypeConstructor.Construct(type, dictionary,false, isThrowErrors);
                } 
            }
            catch (Exception ex)
            {
                if (isThrowErrors) throw new Exception($"Unable to convert to given type. value: {value}, type: {type}. {ex.Message}");
                return value;
            }

            return value;
        }

        private static object _ToDictionary(object value, Type type, bool isThrowErrors = false) {
            var typeDef = type.GetGenericTypeDefinition();
            var typeArgs = type.GetGenericArguments();
            var constructed = typeDef.MakeGenericType(typeArgs);
            var dictionary = Activator.CreateInstance(constructed);
            var sourceDictionary = (Dictionary<string, object>)value;
            foreach (KeyValuePair<string, object> entry in sourceDictionary) {
                var elementKey = _ToType(entry.Key, typeArgs[0], isThrowErrors);
                var elementValue = _ToType(entry.Value, typeArgs[1], isThrowErrors);
                dictionary.GetType().GetMethod("Add")?.Invoke(dictionary, new[] { elementKey, elementValue });
            }
            return dictionary;
        }

        private static object _ToList(object value, Type type, bool isThrowErrors = false) {
            try {
                var typeDef = type.GetGenericTypeDefinition();
                var typeArgs = type.GetGenericArguments();
                var constructed = typeDef.MakeGenericType(typeArgs);
                var list = Activator.CreateInstance(constructed);
                var elements = new List<object>((IEnumerable<object>)value);
                foreach (var elementValue in elements) {
                    var o = _ToType(elementValue, typeArgs[0], isThrowErrors);
                    list.GetType().GetMethod("Add")?.Invoke(list, new[] { o });
                }
                return list;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error trying to convert object of type {value.GetType()} to a List<>.");
                Console.Write(ex.Message);
                return new List<object>();
            }
        }

        private static object _ToArray(object value, Type type, bool isThrowErrors = false) {
            var elements = (List<object>)value;
            var numElements = elements.Count;
            var elementType = type.GetElementType() ?? typeof(object);
            var array = Array.CreateInstance(elementType, numElements);
            var index = 0;
            foreach (var elementValue in elements) {
                var o = _ToType(elementValue, elementType, isThrowErrors);
                array.SetValue(o, index);
                ++index;
            }
            return array;
        }
        
        // START: Multidimensional arrays
        
        private static object _ToMultiDimensionalArray(object value, Type type, int rank, bool isThrowErrors = false) {
            try {
                var elements = (List<object>)value;
                var elementType = type.GetElementType() ?? typeof(object);
                var dimensions = _GetDimensions(elements, rank);

                while (dimensions.Count < rank) dimensions.Add(0);
                var multidimensionalArray = Array.CreateInstance(elementType, dimensions.ToArray());
                _CopyFromJaggedToMultidimensionalArray(elements, multidimensionalArray, _ArrayEmpty<int>());

                return multidimensionalArray;
            }
            catch (Exception ex) {
                Console.WriteLine($"Error trying to convert object of type {value.GetType()} to a multidimensional array.");
                Console.Write(ex.Message);
                return null;
            }
        }
        
        private static IList<int> _GetDimensions(IList elements, int dimensionsCount)
        {
            IList<int> dimensions = new List<int>();

            var currentArray = elements;
            while (true)
            {
                dimensions.Add(currentArray.Count);

                // don't keep calculating dimensions for arrays inside the value array
                if (dimensions.Count == dimensionsCount)  break;
                if (currentArray.Count == 0) break;

                var v = currentArray[0];
                if (v is IList list) currentArray = list;
                else break;
            }

            return dimensions;
        }
        
        private static void _CopyFromJaggedToMultidimensionalArray(IList elements, Array multidimensionalArray, int[] indices)
        {
            var dimension = indices.Length;
            if (dimension == multidimensionalArray.Rank)
            {
                multidimensionalArray.SetValue(_JaggedArrayGetValue(elements, indices), indices);
                return;
            }

            var dimensionLength = multidimensionalArray.GetLength(dimension);
            var list = (IList)_JaggedArrayGetValue(elements, indices);
            var currentValuesLength = list.Count;
            if (currentValuesLength != dimensionLength)
            {
                throw new Exception("Cannot deserialize non-cubical array as multidimensional array.");
            }

            var newIndices = new int[dimension + 1];
            for (var i = 0; i < dimension; i++)
            {
                newIndices[i] = indices[i];
            }

            for (var i = 0; i < multidimensionalArray.GetLength(dimension); i++)
            {
                newIndices[dimension] = i;
                _CopyFromJaggedToMultidimensionalArray(elements, multidimensionalArray, newIndices);
            }
        }
        
        private static T[] _ArrayEmpty<T>()
        {
            // Enumerable.Empty<T> no longer returns an empty array in .NET Core 3.0
            return EmptyArrayContainer<T>.Empty;
        }
        
        private static class EmptyArrayContainer<T>
        {
#pragma warning disable CA1825 // Avoid zero-length array allocations.
            public static readonly T[] Empty = new T[0];
#pragma warning restore CA1825 // Avoid zero-length array allocations.
        }
        
        private static object _JaggedArrayGetValue(IList elements, IList<int> indices)
        {
            var currentList = elements;
            for (var i = 0; i < indices.Count; i++)
            {
                var index = indices[i];
                if (i == indices.Count - 1) return currentList[index];
                
                currentList = (IList)currentList[index];
            }
            return currentList;
        }
        
        /// <summary>
        /// Takes a two-dimensional array [,] and converts it to a jagged array [][]
        /// </summary>
        public static object ToJaggedArray(object value)
        {
            var multiDimensionalArray = value as Array;
            if (multiDimensionalArray == null) return new object[0][];

            var rowsFirstIndex = multiDimensionalArray.GetLowerBound(0);
            var rowsLastIndex = multiDimensionalArray.GetUpperBound(0);
            var numberOfRows = rowsLastIndex - rowsFirstIndex + 1;

            var columnsFirstIndex = multiDimensionalArray.GetLowerBound(1);
            var columnsLastIndex = multiDimensionalArray.GetUpperBound(1);
            var numberOfColumns = columnsLastIndex - columnsFirstIndex + 1;

            object[][] jaggedArray = new object[numberOfRows][];
            for (var i = 0; i < numberOfRows; i++)
            {
                jaggedArray[i] = new object[numberOfColumns];

                for (var j = 0; j < numberOfColumns; j++)
                {
                    jaggedArray[i][j] = multiDimensionalArray.GetValue(i + rowsFirstIndex, j + columnsFirstIndex);
                }
            }
            return jaggedArray;
        }

        // END: Multidimensional arrays
    } 
}
