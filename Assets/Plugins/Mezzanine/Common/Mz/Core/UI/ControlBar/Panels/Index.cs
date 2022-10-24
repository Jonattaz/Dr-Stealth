using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.ControlBar.Panels
{
    public class Index : MzBehaviourBase
    {
        public Left.Index Left { get; private set; }
        public Center.Index Center { get; private set; }
        public Right.Index Right { get; private set; }
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
            LayoutGroup.childAlignment = TextAnchor.MiddleCenter;
            LayoutGroup.childControlWidth = true;
            LayoutGroup.childControlHeight = true;
            LayoutGroup.childForceExpandWidth = true;
            LayoutGroup.childForceExpandHeight = true;

            Left = Add<Left.Index>("left");
            Center = Add<Center.Index>("Center");
            Right = Add<Right.Index>("Right");
        }
    }
}