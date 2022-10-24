using UnityEngine;
using System;
using UnityEditor;

namespace Mz.App
{
    public abstract class MzBehaviourBase<TCore> : MzBehaviourBase
        where TCore : CoreBase
    {
        public static new TCore Core => (TCore) _core;
    }

    public abstract class MzBehaviourBase : MonoBehaviour
    {
        protected static CoreBase _core;
        public static CoreBase Core => _core;

        // START: Initialization

        public bool IsInitialized { get; private set; }
        public event EventHandler Initialized;

        public RectTransform Rect { get; protected set; }

        public bool IsActive
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        public virtual void Initialize(CoreBase core)
        {
            _Initialize(core);
        }

        private void _Initialize(CoreBase core)
        {
            if (IsInitialized) return;

            if (Core == null) _core = core;

            OnInitializeStarted();
            IsInitialized = true;
            OnInitialized();
            Initialized?.Invoke(this, new EventArgs());
        }

        public void AddRect()
        {
            if (Rect == null) Rect = gameObject.AddComponent<RectTransform>();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(0, 0);
        }

        public void ResetInitialization()
        {
            IsInitialized = false;
        }

        protected virtual void OnInitializeStarted() { }
        protected virtual void OnInitialized() { }

        // END: Initialization

        public TComponent Add<TComponent>(string name, bool isInitialize = true)
            where TComponent : MzBehaviourBase
        {
            var childGameObject = new GameObject(name);
            childGameObject.transform.SetParent(gameObject.transform, false);
            var child = childGameObject.AddComponent<TComponent>();
            if (isInitialize) child.Initialize(Core);
            return child;
        }

        public void UpdateLayout()
        {
            // This is a hack to force layouts to update consistently.
            IsActive = !IsActive;
            IsActive = !IsActive;
        }

        public void ExpandInEditor(bool isSelect = true)
        {
#if UNITY_EDITOR
            try
            {
                UnityEditor.EditorGUIUtility.PingObject(gameObject);
                if (isSelect) Selection.activeObject = gameObject;
            }
            catch (Exception x)
            {
                Debug.Log(x.Message);
            }
#endif
        }
    }
}