using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI
{
    public class SpacerVertical : MzBehaviourBase
    {
        private LayoutElement _layoutElement;
        private float _width;

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            _width = Core.ScreenWidth - Core.Specs.GutterHorizontal * 2;

            var height = Core.Specs.Margin;
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0, 1);
            Rect.anchoredPosition = new Vector2(Core.Specs.GutterHorizontal, 0);
            Rect.sizeDelta = new Vector2(_width, height);
            
            _layoutElement = gameObject.AddComponent<LayoutElement>();
            _layoutElement.minWidth = _width;
            _layoutElement.minHeight = height;
            _layoutElement.preferredWidth = _width;
            _layoutElement.preferredHeight = height;
        }
        
        public float SetHeight(float value)
        {
            Rect.sizeDelta = new Vector2(_width, value);
            _layoutElement.minHeight = value;
            _layoutElement.preferredHeight = value;
            return value;
        }
    }
}