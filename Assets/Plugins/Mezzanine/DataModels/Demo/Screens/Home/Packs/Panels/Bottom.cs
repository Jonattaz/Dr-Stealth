using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Panels
{
    public class Bottom : MzBehaviour
    {
        public App.UI.ControlBar.Index Controls{ get; private set; }
        public App.UI.Button.Index ButtonPrevious { get; set; }
        public App.UI.Button.Index ButtonNext { get; set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            var height = Core.Specs.ControlHeight + Core.Specs.ControlBarMarginVertical * 2;
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 0);
            Rect.pivot = new Vector2(0.5f, 0);
            Rect.sizeDelta = new Vector2(0, height);
            
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;

            Controls = Add<App.UI.ControlBar.Index>("Controls");
            Controls.Panels.LayoutGroup.padding.top = Core.Specs.ControlBarMarginVertical;
            Controls.Panels.LayoutGroup.padding.bottom = Core.Specs.ControlBarMarginVertical;
            
            var controlPrevious = Controls.Panels.Left.Main.Add<App.UI.ControlBar.Controls.Left>("ControlPrevious");
            ButtonPrevious = controlPrevious.Button;
            ButtonPrevious.IsActive = false;
            ButtonPrevious.Label.Text.font = Core.Fonts.Icon;
            ButtonPrevious.Label.Text.fontSize = Core.Fonts.Size.Icon3;
            ButtonPrevious.Label.Text.color = Core.Colors.Light;
            ButtonPrevious.Label.Text.text = "←";
            ButtonPrevious.Component.onClick.AddListener(OnButtonPreviousClick);
            Controls.Panels.Left.Gutter.Button.Component.onClick.AddListener(OnButtonPreviousClick);

            var controlNext = Controls.Panels.Right.Main.Add<App.UI.ControlBar.Controls.Right>("ControlNext");
            ButtonNext = controlNext.Button;
            ButtonNext.Label.Text.font = Core.Fonts.Icon;
            ButtonNext.Label.Text.fontSize = Core.Fonts.Size.Icon3;
            ButtonNext.Label.Text.color = Core.Colors.Light;
            ButtonNext.Label.Text.text = "→";
            ButtonNext.Component.onClick.AddListener(OnButtonNextClick);
            Controls.Panels.Right.Gutter.Button.Component.onClick.AddListener(OnButtonNextClick);
        }

        private int _rankCurrent = 0;
        public void OnButtonPreviousClick()
        {
            _rankCurrent--;
            if (_rankCurrent < 0) _rankCurrent = 0;
            if (_rankCurrent == 0) ButtonPrevious.IsActive = false;
            if (_rankCurrent < Core.Model.Data.Packs.Length - 1) ButtonNext.IsActive = true;
          
            var rect = Core.Screens.Home.Packs.Panels.Center.Gallery.Rect;
            rect.anchoredPosition = new Vector2(-Core.ScreenWidth * _rankCurrent, rect.anchoredPosition.y);
        }
        
        public void OnButtonNextClick()
        {
            _rankCurrent++;
            if (_rankCurrent == Core.Model.Data.Packs.Length - 1) ButtonNext.IsActive = false;
            if (_rankCurrent > 0) ButtonPrevious.IsActive = true;
            if (_rankCurrent > Core.Model.Data.Packs.Length - 1) _rankCurrent = Core.Model.Data.Packs.Length - 1;
         
            var rect = Core.Screens.Home.Packs.Panels.Center.Gallery.Rect;
            rect.anchoredPosition = new Vector2(-Core.ScreenWidth * _rankCurrent, rect.anchoredPosition.y);
        }
    }
}

