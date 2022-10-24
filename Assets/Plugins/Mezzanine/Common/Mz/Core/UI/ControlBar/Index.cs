using UnityEngine;

namespace Mz.App.UI.ControlBar
{
    public class Index : MzBehaviourBase
    {
        public Panels.Index Panels { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(0, 0);

            Panels = Add<Panels.Index>("Panels");
        }
    }
}
