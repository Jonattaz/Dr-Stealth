using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Panels
{
    public class Center : MzBehaviour 
    {
        
        public Gallery.Index Gallery { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            var controlBarHeight = Core.Specs.ControlHeight + Core.Specs.ControlBarMarginVertical * 2;
            var dividerHeight = 2;
            var height = Core.ScreenHeight - controlBarHeight * 2 - dividerHeight * 2;
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1);
            
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;

            Gallery = Add<Gallery.Index>("Gallery");
            
            gameObject.AddComponent<RectMask2D>();
        }
    }
}