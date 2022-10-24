// Uncomment the compiler definition below for better performance,
// if you're using a current version of Unity that includes an assembly reference
// for System.Reflection.Emit, and you're not building for WebGL.

// #define REFLECTION_EMIT

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
using System.Reflection.Emit;
#endif

namespace Mz.TypeTools
{
    public class SimpleTypeComparer : IEqualityComparer<Type>
    {
        public bool Equals(Type x, Type y)
        {
            return x.Assembly == y.Assembly &&
                   x.Namespace == y.Namespace &&
                   x.Name == y.Name;
        }

        public int GetHashCode(Type obj)
        {
            throw new NotImplementedException();
        }
    }

    public class PropertyDescription
    {
        public PropertyDescription(object ownerInstance, PropertyInfo propertyInfo, object value)
        {
            OwnerInstance = ownerInstance;
            Info = propertyInfo;
            Value = value;
        }

        public object OwnerInstance { get; set; }
        public PropertyInfo Info { get; private set; }
        public object Value { get; set; }
    }

    public static class Types
    {
        public static bool IsEnumerable(Type type)
        {
            return type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type);
        }

        public static bool IsGenericList(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>));
        }

        public static bool IsGenericDictionary(Type type)
        {
            return type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Dictionary<,>));
        }

        /// <example>
        /// <code>
        /// bool isSubclass = Types.IsSubclassOfRawGeneric(typeof(ParameterSet<>), parameters.GetType());
        /// </code>
        /// </example>
        public static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        public static MethodInfo GetGenericMethod(this Type source, string name, Type[] parameterTypes)
        {
            var methods = source.GetMethods();
            foreach (var method in methods.Where(m => m.Name == name && m.IsGenericMethod))
            {
                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                if (methodParameterTypes.SequenceEqual(parameterTypes, new SimpleTypeComparer()))
                {
                    return method;
                }
            }

            return null;
        }

        public static bool IsNumber(this object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong
                   || value is float
                   || value is double
                   || value is decimal;
        }

        public static bool IsStruct(this Type source)
        {
            return source.IsValueType && !source.IsPrimitive && !source.IsEnum;
        }

        public static IEnumerable<PropertyInfo> GetReadWriteProperties(Type type)
        {
            return type.GetProperties().Where(x => x.CanRead && x.CanWrite);
        }

        public static PropertyDescription GetPropertyDescription(object instance, string propertyPath,
            char delimiter = '.')
        {
            while (true)
            {
                if (instance == null) throw new ArgumentException("Value cannot be null.", "instance");
                if (propertyPath == null) throw new ArgumentException("Value cannot be null.", "memberPath");

                if (propertyPath.Contains(delimiter))
                {
                    var pathSegments = propertyPath.Split(new char[] {delimiter}, 2);
                    instance = GetPropertyDescription(instance, pathSegments[0], delimiter).Value;
                    propertyPath = pathSegments[1];
                }
                else
                {
                    var propertyInfo = instance.GetType().GetProperty(propertyPath);
                    var propertyValue = propertyInfo.GetValue(instance, null);

                    return new PropertyDescription(instance, propertyInfo, propertyValue);
                }
            }
        }

        public static object GetPropertyValue(object instance, string propertyPath, char delimiter = '.')
        {
            foreach (var part in propertyPath.Split(delimiter))
            {
                if (instance == null) return null;

                var type = instance.GetType();
                var info = type.GetProperty(part);
                if (info == null) return null;

                instance = info.GetValue(instance, null);
            }

            return instance;
        }

        /// <summary>
        /// Sets the property value.
        /// </summary>
        /// <returns>The previous property value.</returns>
        /// <example> 
        /// <code>
        /// class Pet {
        ///     public Pet(string name) {
        ///         Name = name;
        ///     }
        /// 
        ///     public string Name { get; private set; }
        /// }
        /// 
        /// class Person {
        ///     public Person(Pet pet) {
        ///         Pet = pet;
        ///     }
        /// 
        ///     public Pet Pet { get; private set;
        /// }
        /// 
        /// class TestClass {
        ///     static string Main() {
        ///         var pet = new Pet("Fido");
        ///         var persion = new Person(pet);
        /// 
        ///         ObjectTools.SetPropertyValue(person, "Pet.Name", "Rover");
        ///         return person.Pet.Name;
        ///     }
        /// }
        /// </code>
        /// </example>
        public static object SetPropertyValue(object instance, string propertyPath, object value, char delimiter = '.')
        {
            var propertyDescription = GetPropertyDescription(instance, propertyPath, delimiter);
            propertyDescription.Info.SetValue(propertyDescription.OwnerInstance, value);
            return propertyDescription.Value;
        }
        
        public static FieldInfo GetField(Type type, string path)
        {
            return type.GetField(path, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }
        
        public static FieldInfo GetFieldViaPath(this Type type, string path)
        {
            var containerType = type;
            var fieldInfo = GetField(containerType, path);
            var pathSegments = path.Split('.');
 
            for (var i = 0; i < pathSegments.Length; i++)
            {
                fieldInfo = GetField(containerType, pathSegments[i]);
 
                // There are only two container field types that can be serialized: Array and List<T>
                if (fieldInfo != null && fieldInfo.FieldType.IsArray)
                {
                    containerType = fieldInfo.FieldType.GetElementType();
                    i += 2;
                    continue;
                }
 
                if (fieldInfo != null && fieldInfo.FieldType.IsGenericType)
                {
                    containerType = fieldInfo.FieldType.GetGenericArguments()[0];
                    i += 2;
                    continue;
                }

                if (fieldInfo == null) return null;
                containerType = fieldInfo.FieldType;   
            }
 
            return fieldInfo;
        }

#if (REFLECTION_EMIT && !UNITY_IOS && !UNITY_WEBGL)
        public delegate T ConstructorDelegate<T>();
        public static ConstructorDelegate<T> GetConstructor<T>()
        {
            var type = typeof(T);
            var constructor = type.GetConstructor(new Type[0]);
            var methodName = $"{type.Name}Ctor";
            
            var dynamicMethod = new DynamicMethod(
                methodName, 
                type, 
                new Type[0], 
                typeof(Activator)
            );

            var ilGenerator = dynamicMethod.GetILGenerator();
            
            // Emit CIL
            ilGenerator.Emit(OpCodes.Newobj, constructor);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(ConstructorDelegate<T>)) as ConstructorDelegate<T>;
        }
  
        public delegate object PropertyGetDelegate(object instance);
        public static PropertyGetDelegate GetPropertyGetter(Type type, string propertyName, bool isThrowErrors = false)
        {
            var propertyInfo = type.GetProperty(propertyName);
            return GetPropertyGetter(propertyInfo, isThrowErrors);
        }
        public static PropertyGetDelegate GetPropertyGetter(PropertyInfo propertyInfo, bool isThrowErrors = false)
        {
            var getter = propertyInfo?.GetGetMethod(true);

            if (getter == null)
            {
                if (isThrowErrors) throw new Exception("Unable to retrieve MethodInfo in Types.GetPropertyGetter()");
                return null;
            }

            var dynamicMethod = new DynamicMethod(
                "GetValue", typeof(object), 
                new [] { typeof(object) }, 
                typeof(object),
                true
            );

            var ilGenerator = dynamicMethod.GetILGenerator();
            
            // Emit CIL
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Call, getter);

            var returnType = getter.ReturnType;
            if (returnType.GetTypeInfo().IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, returnType);
            }

            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(PropertyGetDelegate)) as PropertyGetDelegate;
        }
        
        public delegate void PropertySetDelegate(object instance, object value);
        public static PropertySetDelegate GetPropertySetter(Type type, string propertyName, bool isThrowErrors = false)
        {
            var propertyInfo = type.GetProperty(propertyName);
            return GetPropertySetter(propertyInfo, isThrowErrors);
        }
        public static PropertySetDelegate GetPropertySetter(PropertyInfo propertyInfo, bool isThrowErrors = false)
        {
            var setter = propertyInfo?.GetSetMethod(true);

            if (setter == null)
            {
                if (isThrowErrors) throw new Exception("Unable to retrieve MethodInfo in Types.GetPropertySetter()");
                return null;
            }

            var dynamicMethod = new DynamicMethod(
                "SetValue", typeof(void), 
                new [] { typeof(object), typeof(object) }, 
                typeof(object),
                true
            );

            var ilGenerator = dynamicMethod.GetILGenerator();
            
            // Emit CIL
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);

            var parameterType = setter.GetParameters()[0].ParameterType;

            if (parameterType.GetTypeInfo().IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, parameterType);
            }
            
            ilGenerator.Emit(OpCodes.Call, setter);
            ilGenerator.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(typeof(PropertySetDelegate)) as PropertySetDelegate;
        }
#endif
    }
}