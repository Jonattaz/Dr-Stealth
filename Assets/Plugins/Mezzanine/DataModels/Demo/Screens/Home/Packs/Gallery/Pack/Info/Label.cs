using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack.Info
{
    public class Label : MzBehaviour
    {
        private Text _text;
        private LayoutElement _layoutElement;
        private float _width;

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            _width = Core.ScreenWidth - Core.Specs.GutterHorizontal * 2;
            
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0, 1);
            Rect.anchoredPosition = new Vector2(Core.Specs.GutterHorizontal, 0);
            Rect.sizeDelta = new Vector2(_width, Core.Specs.ControlHeight);
            
            _layoutElement = gameObject.AddComponent<LayoutElement>();
            _layoutElement.minWidth = _width;
            _layoutElement.minHeight = Core.Specs.ControlHeight;
            _layoutElement.preferredWidth = _width;
            _layoutElement.preferredHeight = Core.Specs.ControlHeight;

            _text = gameObject.AddComponent<Text>();
            _text.alignment = TextAnchor.UpperLeft;
            _text.alignByGeometry = true;
            _text.horizontalOverflow = HorizontalWrapMode.Overflow;
            _text.verticalOverflow = VerticalWrapMode.Overflow;
            _text.color = Core.Colors.VeryLight;
            _text.font = Core.Fonts.MainSemibold;
            _text.fontSize = Core.Fonts.Size.Larger2;
        }
        
        public float SetText(string text)
        {
            var textGen = new TextGenerator();
            var generationSettings = _text.GetGenerationSettings(_text.rectTransform.rect.size);
            var height = textGen.GetPreferredHeight(text.ToUpper(), generationSettings);

            Rect.sizeDelta = new Vector2(_width, height);
            _layoutElement.minHeight = height;
            _layoutElement.preferredHeight = height;

            _text.text = text.ToUpper();

            return height;
        }
    }
}