using UnityEngine;

namespace Mz.DemoDataModels.Screens
{
    public class Index : MzBehaviour
    {
        public Home.Index Home { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0, 1);
            Rect.anchoredPosition = new Vector2(0, 0);
            Rect.sizeDelta = new Vector2(0, Core.ScreenHeight * 2);
            
            Home = Add<Home.Index>("Home");
        }
    }
}
