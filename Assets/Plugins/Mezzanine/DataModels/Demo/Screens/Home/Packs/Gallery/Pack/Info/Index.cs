using UnityEngine;
using UnityEngine.UI;
using Mz.DemoDataModels.Models;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack.Info
{
    public class Index : MzBehaviour
    {
        private Label _label;
        private Description _description;

        private LayoutElement _layoutElement;
        private float _marginVertical = 90;
        
        public float Height { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0, 1);
            
            _layoutElement = gameObject.AddComponent<LayoutElement>();
            _layoutElement.minWidth = Core.ScreenWidth;
            _layoutElement.minHeight = Core.Specs.ControlHeight;
            _layoutElement.preferredWidth = Core.ScreenWidth;
            _layoutElement.preferredHeight = Core.Specs.ControlHeight;
            
            var layoutGroup = gameObject.AddComponent<VerticalLayoutGroup>();
            layoutGroup.spacing = _marginVertical;
            layoutGroup.childAlignment = TextAnchor.UpperCenter;
            layoutGroup.childControlWidth = true;
            layoutGroup.childControlHeight = false;
            layoutGroup.childForceExpandWidth = false;
            layoutGroup.childForceExpandHeight = false;
            
            _label = Add<Label>("Label");
            _description = Add<Description>("Description");
        }

        public void LoadData(PackData packData)
        {
            _SetText(packData.Label, packData.Description);
        }

        private void _SetText(string label, string description)
        {
            var labelHeight = _label.SetText(label);
            var descriptionHeight = _description.SetText(description);

            var height = labelHeight + _marginVertical + descriptionHeight + _marginVertical;

            Rect.sizeDelta = new Vector2(Core.ScreenWidth, height);
            _layoutElement.minHeight = height;
            _layoutElement.preferredHeight = height;
            Height = height;
        }
    }
}