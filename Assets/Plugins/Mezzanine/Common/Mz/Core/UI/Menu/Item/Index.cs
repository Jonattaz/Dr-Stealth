using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.UI.Menu.Item
{
    public class Index : MzBehaviourBase
    {
        public Label Label { get; private set; }
        public Value.Index Value { get; private set; }
        public float Height { get; set; }
        public UnityEngine.UI.Button Button { get; private set; }

        private ImageRounded _image;

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            Height = Core.Specs.ControlHeight;
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1);
            Rect.sizeDelta = new Vector2(0, Height);

            var layoutGroup = gameObject.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.spacing = Core.Specs.Margin;
            layoutGroup.childAlignment = TextAnchor.MiddleCenter;
            layoutGroup.childControlHeight = true;
            layoutGroup.childControlWidth = true;
            layoutGroup.childForceExpandHeight = true;
            layoutGroup.childForceExpandWidth = true;

            Label = Add<Label>("Label");
            Value = Add<Value.Index>("Value");

            // The image is needed so that the whole area of the menu item is clickable.
            _image = gameObject.AddComponent<ImageRounded>();
            _image.color = new Color(1, 1, 1, 0);
            Button = gameObject.AddComponent<UnityEngine.UI.Button>();
            Button.image = _image;
        }
    }
}