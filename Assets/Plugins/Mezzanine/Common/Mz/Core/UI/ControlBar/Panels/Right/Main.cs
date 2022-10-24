using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.ControlBar.Panels.Right
{
    public class Main : MzBehaviourBase
    {
        public HorizontalLayoutGroup LayoutGroup { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(0, 0);
            
            LayoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            LayoutGroup.spacing = 0;
            LayoutGroup.childAlignment = TextAnchor.MiddleRight;
            LayoutGroup.childControlWidth = true;
            LayoutGroup.childControlHeight = true;
            LayoutGroup.childForceExpandWidth = false;
            LayoutGroup.childForceExpandHeight = true;
        }
    }
}