using UnityEngine;
using UnityEngine.UI;
using Mz.App.UI.Button;

namespace Mz.App.UI.ControlBar.Panels.Right
{
    public class Gutter : MzBehaviourBase
    {
        public Button.Index Button { get; private set; }
        public LayoutElement LayoutElement { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(1, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(1, 1);
            Rect.sizeDelta = new Vector2(Core.Specs.GutterHorizontal, Core.Specs.ControlHeight);
            
            LayoutElement = gameObject.AddComponent<LayoutElement>();
            LayoutElement.minHeight = Core.Specs.ControlHeight;
            LayoutElement.preferredHeight = Core.Specs.ControlHeight;
            LayoutElement.minWidth = Core.Specs.GutterHorizontal;
            LayoutElement.preferredWidth = Core.Specs.GutterHorizontal;
            
            Button = Add<Button.Index>("Button", false);
            Button.Initialize(Core, ButtonType.Invisible);
            Button.Rect.anchorMin = new Vector2(0, 0);
            Button.Rect.anchorMax = new Vector2(1, 1);
            Button.Rect.sizeDelta = new Vector2(0, 0);
        }
    }
}