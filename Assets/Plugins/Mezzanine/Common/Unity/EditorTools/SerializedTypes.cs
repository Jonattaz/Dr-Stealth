using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Mz.Unity.EditorTools
{
    public static class SerializedTypes
    {
        /// <summary>
        /// Draw all the fields of the given property to the editor.
        /// </summary>
        public static void DrawFields(SerializedObject objectSerialized)
        {
            var properties = GetFields(objectSerialized);
            DrawFields(properties.Values);
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        /// <summary>
        /// Draw each of the properties in the provided collection to the editor.
        /// </summary>
        public static void DrawFields(IEnumerable<SerializedProperty> properties, bool isIgnoreScriptField = true)
        {
            foreach (var property in properties)
            {
                // Ignore the script field by default. Who needs it?
                if (isIgnoreScriptField && property.name == "m_Script") continue;
                
                if (property.isArray && property.propertyType == SerializedPropertyType.Generic)
                {
                    DrawArrayField(property);
                }
                else
                {
                    DrawField(property);
                }
            }
        }

        /// <summary>
        /// Collect all the properties of the given serialized object into a dictionary, keyed by name, and return the dictionary.
        /// </summary>
        public static  Dictionary<string, SerializedProperty> GetFields(SerializedObject objectSerialized)
        {
            Dictionary<string, SerializedProperty> properties = new Dictionary<string, SerializedProperty>();

            if (objectSerialized == null) return properties;
            
            // Get the first property in the object.
            var property = objectSerialized.GetIterator();
             
            while (property.NextVisible(true))
            {
                if (properties.ContainsKey(property.name)) continue;
                properties.Add(property.name, property.Copy());
            }

            return properties;
        }
        
        /// <summary>
        /// Return the field specified by the provided path. The path will be delimited with '.'.
        /// </summary>
        public static SerializedProperty GetField(SerializedProperty property, string propertyPath)
        {
            return property.FindPropertyRelative(propertyPath);
        }
        
        /// <summary>
        /// Draw the field specified by the provided path to the editor. The path will be delimited with '.'.
        /// </summary>
        public static void DrawField(SerializedProperty property, string propertyPath)
        {
            var propertyItemField = GetField(property, propertyPath);
            if (propertyItemField != null) DrawField(propertyItemField);
        }
        
        /// <summary>
        /// Draw the given field to the editor.
        /// </summary>
        public static void DrawField(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true);
        }

        /// <summary>
        /// Draw the given array field to the editor.
        /// </summary>
        public static void DrawArrayField(SerializedProperty property)
        {
            DrawArrayFieldHeader(property);
        
            if (!property.isExpanded) return;
            EditorGUI.indentLevel++;
            DrawFields(property.serializedObject);
            EditorGUI.indentLevel--;
        }
        
        /// <summary>
        /// Draw the fold-out header of the provided array field.
        /// </summary>
        /// <returns>A bool indicated whether the header is currently expanded.</returns>
        public static bool DrawArrayFieldHeader(SerializedProperty property)
        {
            EditorGUILayout.BeginHorizontal();
            property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, property.displayName);
            EditorGUILayout.EndHorizontal();
            return property.isExpanded;
        }

        /// <summary>
        /// Return the value of the given property.
        /// </summary>
        public static T GetValue<T>(this SerializedProperty property, SerializedObject container)
        {
            var fieldInfo = GetFieldInfo(property);
            var value = fieldInfo.GetValue(container.targetObject);
            return (T) value;
        }
        
        /// <summary>
        /// Set the value of the given property.
        /// </summary>
        /// <param name="isApply">If true, ApplyModifiedProperties() is called on the container object.</param>
        public static void SetValue(this SerializedProperty property, SerializedObject container, object value, bool isApply = true)
        {
            var fieldInfo = GetFieldInfo(property);
            fieldInfo.SetValue(container.targetObject, value);
            if (isApply) container.ApplyModifiedProperties();
        }

        /// <summary>
        /// Get the FieldInfo object for the specified property.
        /// </summary>
        public static FieldInfo GetFieldInfo(this SerializedProperty property)
        {
            if (
                property == null || 
                property.serializedObject == null ||
                property.serializedObject.targetObject == null
            ) return null;
            
            var containerType = property.serializedObject.targetObject.GetType();
            return Mz.TypeTools.Types.GetFieldViaPath(containerType, property.propertyPath);
        }
        
        /// <summary>
        /// Get the List represented by the given property.
        /// </summary>
        public static List<TItem> GetList<TItem>(SerializedProperty propertyList, SerializedObject container)
        {
            // See: http://sketchyventures.com/2015/08/07/unity-tip-getting-the-actual-object-from-a-custom-property-drawer/
            var fieldInfo = GetFieldInfo(propertyList);
            if (fieldInfo == null) return new List<TItem>();
            var list = fieldInfo.GetValue(container.targetObject);
            return (List<TItem>)list;
        }
        
        /// <summary>
        /// Get a particular item from the List represented by a property.
        /// </summary>
        public static TItem GetListItem<TItem>(SerializedProperty propertyItem, SerializedProperty propertyList, SerializedObject container)
        {
            // See: http://sketchyventures.com/2015/08/07/unity-tip-getting-the-actual-object-from-a-custom-property-drawer/
            var fieldInfo = GetFieldInfo(propertyList);
            var list = GetList<TItem>(propertyList, container);
            var index = GetIndexFromListItemProperty(propertyItem);
            return list[index];
        }
        
        /// <summary>
        /// Get the value of a particular item from the List represented by a property.
        /// </summary>
        public static TValue GetListItemValue<TItem, TValue>(
            SerializedProperty propertyField,
            SerializedProperty propertyListItem, 
            SerializedProperty propertyList, 
            SerializedObject container
        )
        {
            var item = GetListItem<UnityEngine.Object>(propertyListItem, propertyList, container);
            var itemSerialized = new SerializedObject(item);
            return GetValue<TValue>(propertyField, itemSerialized);
        }

        /// <summary>
        /// Get the index of a particular item from the List represented by a property.
        /// </summary>
        public static int GetIndexFromListItemProperty(SerializedProperty property)
        {
            var path = property.propertyPath;
            var index = Convert.ToInt32(new string(path.Where(char.IsDigit).ToArray()));
            return index;
        }
    }
}