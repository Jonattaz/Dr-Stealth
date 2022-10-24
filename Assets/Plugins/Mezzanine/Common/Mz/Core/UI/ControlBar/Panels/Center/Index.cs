using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.ControlBar.Panels.Center
{
    public class Index : MzBehaviourBase
    {
        public HorizontalLayoutGroup LayoutGroup { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 0.5f);
            Rect.anchorMax = new Vector2(1, 0.5f);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(0, 120);
            
            LayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            LayoutGroup.spacing = 40;
            LayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            LayoutGroup.childControlWidth = true;
            LayoutGroup.childControlHeight = true;
            LayoutGroup.childForceExpandWidth = false;
            LayoutGroup.childForceExpandHeight = true;
        }
    }
}