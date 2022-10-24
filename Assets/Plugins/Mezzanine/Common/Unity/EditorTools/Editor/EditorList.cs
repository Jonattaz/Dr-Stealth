using System;
using UnityEngine;
using System.Collections.Generic;
using Mz.Images;
using UnityEditor;

namespace Mz.Unity.EditorTools
{
    public class EditorList<TListContainer, TItem>
    {
        public EditorList(
            TListContainer containerGameObject, 
            SerializedObject listContainerSerialized, 
            SerializedProperty propertyList,
            string headerLabel = null,
            bool isItemFoldout = false
        )
        {
            Target = containerGameObject;
            _listContainerSerialized = listContainerSerialized;
            _propertyList = propertyList;
            List = Mz.Unity.EditorTools.SerializedTypes.GetList<TItem>(propertyList, listContainerSerialized);
            _headerLabel = headerLabel;
            _isItemFoldout = isItemFoldout;
        }

        public TListContainer Target { get; private set; }
        protected SerializedObject _listContainerSerialized;
        protected SerializedProperty _propertyList;
        public List<TItem> List { get; private set; }
        protected float _inspectorWidth;

        protected EditorListItem _itemEditor;

        protected string _headerLabel;
        protected bool _isItemFoldout;
        protected bool _isExpanded;

        public virtual void OnDisable(bool isUnloadUnusedAssets = true)
        {
            _itemEditor?.OnDisable(isUnloadUnusedAssets);
        }

        public void Draw(bool isAuto = true)
        {
            if (_headerLabel != null)
            {
                _DrawListHeader(_headerLabel);
                if (!_isExpanded)
                {
                    _SetInspectorWidth();
                    return;
                }
            
                if (_isItemFoldout) EditorGUI.indentLevel++;
            }

            for (var i = 0; i < _propertyList.arraySize; i++)
            {
                var propertyItem = _propertyList.GetArrayElementAtIndex(i);
                var itemData = Mz.Unity.EditorTools.SerializedTypes.GetListItem<TItem>(propertyItem, _propertyList, _listContainerSerialized);

                _DrawItemHeader(propertyItem, i, itemData);
                
                EditorGUILayout.Space();
                
                if (!propertyItem.isExpanded) continue;
                _SetItemEditor(propertyItem, itemData);
                _itemEditor?.Draw(propertyItem, itemData, i, _inspectorWidth);
                if (i < _propertyList.arraySize - 1) _DrawItemDivider();
            }

            if (_isItemFoldout) EditorGUI.indentLevel--;

            _DrawListFooter();

            _SetInspectorWidth();
        }

        private void _SetInspectorWidth()
        {
            // Get the width of the inspector, taking the scrollbar into account.
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            
            var rectScreen = GUILayoutUtility.GetLastRect();
            if (rectScreen.width > 1) _inspectorWidth = rectScreen.width;
        }

        protected virtual void _SetItemEditor(SerializedProperty propertyItem, TItem itemData)
        {
            if (_itemEditor != null) return;
            _itemEditor = new EditorListItem();
        }
        
        protected virtual void _DrawListHeader(string foldoutLabel)
        {
            var styleFoldoutHeaderGroup = new GUIStyle(EditorStyles.foldoutHeader);
            styleFoldoutHeaderGroup.fixedHeight = 30;
            styleFoldoutHeaderGroup.fixedWidth = 0;
            var offsetLeft = (EditorGUI.indentLevel + 1) * 15;
            if (EditorGUI.indentLevel == 0) offsetLeft += 4;
            styleFoldoutHeaderGroup.padding = new RectOffset(offsetLeft, 5, 6, 6);
            var styleMenuIcon = new GUIStyle(EditorStyles.foldoutHeaderIcon);

            var contentHeaderGroup = new GUIContent(foldoutLabel);

            Action<Rect> showHeaderContextMenu = rect => { };
            
            _isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(
                _isExpanded,
                contentHeaderGroup, 
                styleFoldoutHeaderGroup, 
                showHeaderContextMenu,
                styleMenuIcon
            );

            EditorGUILayout.EndFoldoutHeaderGroup();
        }

        protected Texture2D _textureItemHeaderBackground;
        protected virtual void _DrawItemHeader(
            SerializedProperty propertyItem, 
            int itemIndex, 
            TItem itemData
        )
        {
            var styleHeaderGroup = _isItemFoldout ? new GUIStyle(EditorStyles.foldoutHeader) : new GUIStyle(EditorStyles.miniBoldLabel);

            if (_isItemFoldout)
            {
                styleHeaderGroup.fontSize = 11;
                styleHeaderGroup.fontStyle = FontStyle.Bold;
            }
            else
            {
                if (_textureItemHeaderBackground == null)
                {
                    var imageBackground = new MzImage(600, 1, new byte[] { 0, 0, 0, 26 });
                    _textureItemHeaderBackground = UnityImageTools.Texture2DFromMzImage(imageBackground);
                }
                
                styleHeaderGroup.normal.background = _textureItemHeaderBackground;
            }
            
            var offsetLeft = (EditorGUI.indentLevel + 1) * 15;
            if (_isItemFoldout && EditorGUI.indentLevel == 0) offsetLeft += 4;
            styleHeaderGroup.padding = new RectOffset(offsetLeft, 5, 6, 6);
            var styleMenuIcon = new GUIStyle(EditorStyles.foldoutHeaderIcon);

            var contentHeaderGroup = new GUIContent($"{_GetItemLabel(propertyItem, itemIndex, itemData)}");

            Action<Rect> showHeaderContextMenu = rect => { _ShowHeaderContextMenu(rect, propertyItem, itemIndex, itemData); };
            
            
            var isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup(
                propertyItem.isExpanded,
                contentHeaderGroup, 
                styleHeaderGroup, 
                showHeaderContextMenu,
                styleMenuIcon
            );
            EditorGUILayout.EndFoldoutHeaderGroup();

            propertyItem.isExpanded = !_isItemFoldout || isExpanded;
        }

        protected virtual string _GetItemLabel(
            SerializedProperty propertyItem,
            int itemIndex,
            TItem itemData
        )
        {
            return propertyItem.displayName;
        }

        protected virtual void _DrawItemDivider()
        {
            var height = 1;
            var color = Color.black;
            GUILayout.Space(10);
            EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, height), color);
        }

        protected virtual void _ShowHeaderContextMenu(Rect position, SerializedProperty propertyItem, int itemIndex, TItem itemData)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Delete"), false, _OnHeaderContextMenuItemClicked, new MzMenuItemArgs<TItem>(propertyItem, itemIndex, itemData, "delete"));
            menu.DropDown(position);
        }

        protected virtual void _OnHeaderContextMenuItemClicked(object userData)
        {
            var args = (MzMenuItemArgs<TItem>) userData;
            switch (args.Type)
            {
                case "delete":
                    var index = args.ItemIndex > -1 ? args.ItemIndex : _propertyList.arraySize - 1;
                    RemoveItem(index);
                    break;
            }
        }

        protected virtual void _DrawListFooter()
        {
            if (GUILayout.Button("Add New"))
            {
                AddItem();
            }
        }

        public SerializedProperty AddItem(object options = null)
        {
            _propertyList.arraySize++;
            _propertyList.serializedObject.ApplyModifiedProperties();
            var propertyItem = _propertyList.GetArrayElementAtIndex(_propertyList.arraySize - 1);
            var itemData = Mz.Unity.EditorTools.SerializedTypes.GetListItem<TItem>(propertyItem, _propertyList, _listContainerSerialized);
            _OnItemAdded(propertyItem, itemData, options);
            return propertyItem;
        }
        
        public void RemoveItem(int index)
        {
            var indexFinal = index > -1 ? index : _propertyList.arraySize - 1;
            
            var propertyItem = _propertyList.GetArrayElementAtIndex(indexFinal);
            var itemData = Mz.Unity.EditorTools.SerializedTypes.GetListItem<TItem>(propertyItem, _propertyList, _listContainerSerialized);
            _OnItemRemoved(propertyItem, itemData);
            
            _propertyList.DeleteArrayElementAtIndex(indexFinal);
            _propertyList.serializedObject.ApplyModifiedProperties();
        }

        protected virtual void _OnItemAdded(SerializedProperty propertyItem, TItem itemData, object options = null) {}
        protected virtual void _OnItemRemoved(SerializedProperty propertyItem, TItem itemData) {}
    }
}