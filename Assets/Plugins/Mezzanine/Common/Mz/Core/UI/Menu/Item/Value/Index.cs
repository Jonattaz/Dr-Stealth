using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Menu.Item.Value
{
    public class Index : MzBehaviourBase
    {
        public Content Content { get; private set; }
        public Icon Icon { get; private set; }
        public Gutter Gutter { get; private set; }
        
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);

            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = Core.ScreenWidth / 2 - (float) Core.Specs.Margin / 2;
            layoutElement.preferredWidth = layoutElement.minWidth;
            
            var layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = 0;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlHeight = true;
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childForceExpandWidth = true;

            Content = Add<Content>("Content");
            Icon = Add<Icon>("Icon");
            Gutter = Add<Gutter>("Gutter");
        }
    }
}