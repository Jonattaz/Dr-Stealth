using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Panels
{
    public class Index : MzBehaviour
    {
        public Top Top { get; private set; }
        public Center Center { get; private set; }
        public Bottom Bottom { get; private set; }
        public App.UI.Divider DividerTop { get; private set; }
        public App.UI.Divider DividerBottom { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(0, 0);
            
            var layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = 0;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childForceExpandHeight = true;

            Top = Add<Top>("Top");
            
            DividerTop = Add<App.UI.Divider>("Divider");
            DividerTop.SetGradient(0.0f, 0.2f);
            DividerTop.IsActive = false;
            
            Center = Add<Center>("Center");
            
            DividerBottom = Add<App.UI.Divider>("Divider");
            DividerBottom.SetGradient(0.0f, 0.2f);
            DividerBottom.IsActive = true;
            
            Bottom = Add<Bottom>("Bottom");
        }
    }
}