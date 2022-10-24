using UnityEngine;

namespace Mz.DemoDataModels.Screens.Home.Packs
{
    public class Index : MzBehaviour
    {
        public Panels.Index Panels { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0, 1);
            Rect.sizeDelta = new Vector2(0, 0);

            Panels = Add<Panels.Index>("Panels");
        }
    }
}

