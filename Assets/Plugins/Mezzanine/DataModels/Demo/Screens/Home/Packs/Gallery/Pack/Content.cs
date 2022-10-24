using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack
{
    public class Content : MzBehaviour
    {
        public Levels.Index Levels { get; private set; }
        public Info.Index Info { get; private set; }
        public Status Status { get; private set; }
        
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1);
            Rect.sizeDelta = new Vector2(0, Core.ScreenHeight);

            var layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = Core.Specs.Margin * 1.5f;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
            
            Info = Add<Info.Index>("Info");
            Levels = Add<Levels.Index>("Levels");
            Status = Add<Status>("Status");
        }
    }
}