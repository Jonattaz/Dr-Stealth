using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Menu.Item.Value
{
    public class Content : MzBehaviourBase
    {
        protected UnityEngine.UI.Text _text;
        public string Text
        {
            get => _text.text;
            set => _text.text = value;
        }
        
        public Color Color
        {
            get => _text.color;
            set => _text.color = value;
        }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0.5f, 0.5f);

            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = Core.ScreenWidth / 2 - (float) Core.Specs.Margin / 2 - Core.Specs.ControlHeight - Core.Specs.GutterHorizontal;
            layoutElement.preferredWidth = layoutElement.minWidth;

            _text = gameObject.AddComponent<Text>();
            _text.font = Core.Fonts.MainSemibold;
            _text.fontSize = Core.Fonts.Size.Larger1;
            _text.color = Core.Colors.Light;
            _text.supportRichText = true;
            _text.alignment = TextAnchor.MiddleLeft;
            _text.alignByGeometry = true;
            _text.horizontalOverflow = HorizontalWrapMode.Wrap;
            _text.verticalOverflow = VerticalWrapMode.Truncate;
        }
    }
}