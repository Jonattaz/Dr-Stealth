using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Button
{
    public class Label : MzBehaviourBase
    {
        public Text Text { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);

            Text = gameObject.AddComponent<Text>();
            Text.alignment = TextAnchor.MiddleCenter;
            Text.alignByGeometry = true;
            Text.horizontalOverflow = HorizontalWrapMode.Overflow;
            Text.verticalOverflow = VerticalWrapMode.Overflow;
            Text.color = Core.Colors.VeryLight;
            Text.font = Core.Fonts.MainSemibold;
            Text.fontSize = Core.Fonts.Size.Larger1;
        }
    }
}