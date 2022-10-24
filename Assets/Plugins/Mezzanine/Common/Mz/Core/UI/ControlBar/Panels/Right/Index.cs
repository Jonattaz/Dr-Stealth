using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.ControlBar.Panels.Right
{
    public class Index : MzBehaviourBase
    {
        public Gutter Gutter { get; private set; }
        public Main Main { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(1, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(1, 1);
            Rect.sizeDelta = new Vector2(Core.Specs.ControlWidth, Core.Specs.ControlHeight);
            
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = Core.Specs.ControlHeight;
            layoutElement.preferredHeight = Core.Specs.ControlHeight;
            layoutElement.minWidth = Core.Specs.ControlWidth;
            layoutElement.preferredWidth = Core.Specs.ControlWidth;
            
            var layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 0;
            layoutGroup.childAlignment = TextAnchor.MiddleRight;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = true;
            
            Main = Add<Main>("Main");
            Gutter = Add<Gutter>("Gutter");
        }
    }
}