using UnityEditor;

namespace Mz.Unity.EditorTools
{
    public class EditorListItem
    {
        protected float _inspectorWidth;
        
        public virtual void Draw(SerializedProperty propertyItem, object itemData, int index, float inspectorWidth)
        {
            _inspectorWidth = inspectorWidth;
            _DrawPanelTop(propertyItem, index, itemData);
            _DrawPanelFields(propertyItem, itemData);
        }

        //===== Virtual
        
        public virtual void OnDisable(bool isUnloadUnusedAssets = true) { }
        
        protected virtual void _DrawPanelTop(
            SerializedProperty propertyItem, 
            int itemIndex, 
            object itemData
        ) { }

        protected virtual void _DrawPanelFields(SerializedProperty propertyItem, object itemData)
        {
            foreach (SerializedProperty propertyItemField in propertyItem)
            {
                if (propertyItemField.isArray && propertyItemField.propertyType == SerializedPropertyType.Generic)
                {
                    _DrawFieldListHeader(propertyItemField, itemData, EditorGUI.indentLevel);

                    if (propertyItemField.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        _DrawPanelFields(propertyItemField, itemData);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    SerializedTypes.DrawField(propertyItemField);
                }
            }
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        protected virtual void _DrawFieldListHeader(SerializedProperty propertyItemField, object itemData, int indentLevel = 0)
        {
            EditorGUILayout.BeginHorizontal();
            propertyItemField.isExpanded = EditorGUILayout.Foldout(propertyItemField.isExpanded, propertyItemField.displayName);
            EditorGUILayout.EndHorizontal();
        }
    }
}