using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Menu.Item.Value
{
    public class Icon : MzBehaviourBase
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
            layoutElement.minWidth = Core.Specs.ControlHeight;
            layoutElement.preferredWidth = Core.Specs.ControlHeight;

            _text = gameObject.AddComponent<Text>();
            _text.font = Core.Fonts.Icon;
            _text.fontSize = Core.Fonts.Size.Icon3;
            _text.color = Core.Colors.Light;
            _text.supportRichText = true;
            _text.alignment = TextAnchor.MiddleRight;
            _text.alignByGeometry = true;
            _text.horizontalOverflow = HorizontalWrapMode.Overflow;
            _text.verticalOverflow = VerticalWrapMode.Overflow;
            _text.text = "›";
        }
    }
}