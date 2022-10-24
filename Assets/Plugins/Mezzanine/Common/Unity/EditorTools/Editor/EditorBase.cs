using System.Collections.Generic;
using Mz.Unity.EditorTools;
using UnityEditor;
using UnityEngine;

public abstract class EditorBase<TDataObject> : Editor
    where TDataObject : UnityEngine.Object
{
    public TDataObject Target { get; private set; }
    public SerializedObject TargetSerialized { get; private set; }
    
    protected bool _isInitialized;

    private void Awake()
    {
        if (_isInitialized || target == null) return;
        
        Target = (TDataObject) target;
        TargetSerialized = new SerializedObject(Target);

        _Initialize();
        _isInitialized = true;
    }
    
    protected virtual void _Initialize() {}
    
    void OnDisable()
    {
        _Disable();
        Resources.UnloadUnusedAssets();
    }

    protected virtual void _Disable() { }
    
    public override void OnInspectorGUI()
    {
        if (TargetSerialized == null) return;
        TargetSerialized.Update();
        var fields = SerializedTypes.GetFields(TargetSerialized);
        _DrawFields(fields);
        TargetSerialized.ApplyModifiedProperties();
    }

    protected virtual void _DrawFields(Dictionary<string, SerializedProperty> fields)
    {
        SerializedTypes.DrawFields(fields.Values);
    }
}