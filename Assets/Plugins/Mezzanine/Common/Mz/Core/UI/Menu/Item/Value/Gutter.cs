using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Menu.Item.Value
{
    public class Gutter : MzBehaviourBase
    {
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);

            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = Core.Specs.ControlHeight + Core.Specs.GutterHorizontal;
            layoutElement.preferredWidth = Core.Specs.ControlHeight + Core.Specs.GutterHorizontal;
        }
    }
}