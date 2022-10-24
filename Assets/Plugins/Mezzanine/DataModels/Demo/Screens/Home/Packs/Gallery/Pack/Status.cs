using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack
{
    public class Status : MzBehaviour
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
            Rect.pivot = new Vector2(0, 0.5f);
            Rect.anchoredPosition = new Vector2(Core.Specs.GutterHorizontal, 0);
            Rect.sizeDelta = new Vector2(_width, Core.Specs.ControlHeight);
            
            _layoutElement = gameObject.AddComponent<LayoutElement>();
            _layoutElement.minWidth = _width;
            _layoutElement.minHeight = Core.Specs.ControlHeight;
            _layoutElement.preferredWidth = _width;
            _layoutElement.preferredHeight = Core.Specs.ControlHeight;

            _text = gameObject.AddComponent<Text>();
            _text.supportRichText = true;
            _text.alignment = TextAnchor.UpperLeft;
            _text.alignByGeometry = true;
            _text.horizontalOverflow = HorizontalWrapMode.Wrap;
            _text.verticalOverflow = VerticalWrapMode.Overflow;
            _text.color = Core.Colors.MediumDark0;
            _text.font = Core.Fonts.Icon;
            _text.fontSize = Core.Fonts.Size.Smaller1;
        }

        private string _textChanged;
        private string _textSaved;

        public void SetTextChanged(string text)
        {
            _textChanged = text;
            _SetText();
        }
        
        public void SetTextSaved(string text)
        {
            _textSaved = text;
            _SetText();
        }
        
        private void _SetText()
        {
            var textFinal = _textChanged + _textSaved;
            textFinal = textFinal.Replace("<br>", System.Environment.NewLine);
            
            var textGen = new TextGenerator();
            var generationSettings = _text.GetGenerationSettings(_text.rectTransform.rect.size);
            var height = textGen.GetPreferredHeight(textFinal, generationSettings);
            
            Rect.sizeDelta = new Vector2(_width, height);
            _layoutElement.minHeight = height;
            _layoutElement.preferredHeight = height;
            
            _text.text = textFinal;
        }
    }
}