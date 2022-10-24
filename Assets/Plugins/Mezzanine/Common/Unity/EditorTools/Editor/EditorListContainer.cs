using Mz.Unity.EditorTools;
using UnityEditor;
using UnityEngine;

public class EditorListContainer<TListContainer, TListItem> : Editor
    where TListContainer : UnityEngine.Object
{
    public TListContainer Target { get; private set; }
    public SerializedObject TargetSerialized { get; private set; }

    protected EditorList<TListContainer, TListItem> _editorList;
    protected bool _isInitialized;

    private void Awake()
    {
        if (_isInitialized || target == null) return;

        Target = (TListContainer) target;
        TargetSerialized = new SerializedObject(Target);
        var propertyList = TargetSerialized.FindProperty("List");
        
        _Initialize();
        _isInitialized = true;
        
        _SetListEditor(Target, TargetSerialized, propertyList);
    }

    protected virtual void _Initialize() { }

    void OnDisable()
    {
        _editorList?.OnDisable(false);
        _Disable();
        Resources.UnloadUnusedAssets();
    }

    protected virtual void _Disable() { }

    protected virtual void _SetListEditor(
        TListContainer containerGameObject, 
        SerializedObject listContainerSerialized,
        SerializedProperty propertyList
    )
    {
        _editorList = new EditorList<TListContainer, TListItem>(
                containerGameObject, 
                listContainerSerialized,
                propertyList
            );
    }

    public override void OnInspectorGUI()
    {
        TargetSerialized.Update();
        _editorList.Draw();
        TargetSerialized.ApplyModifiedProperties();
    }
}