using UnityEditor;

namespace Mz.Unity.EditorTools
{
    public class MzMenuItemArgs<TItem>
    {
        public MzMenuItemArgs(SerializedProperty propertyItem, int itemIndex, TItem itemData, string type, object[] options = null)
        {
            PropertyItem = propertyItem;
            ItemIndex = itemIndex;
            ItemData = itemData;
            Type = type;
            Options = options ?? new object[0];
        }

        public SerializedProperty PropertyItem { get; }
        public int ItemIndex { get; } = -1;
        public TItem ItemData { get; }
        public string Type { get; }
        public object[] Options;
    }
}