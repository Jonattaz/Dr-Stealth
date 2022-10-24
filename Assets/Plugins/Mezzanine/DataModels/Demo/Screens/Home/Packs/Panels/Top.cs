using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Panels
{
    public class Top : MzBehaviour {

        public App.UI.ControlBar.Index Controls{ get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            var height = Core.Specs.ControlHeight + Core.Specs.ControlBarMarginVertical * 2;
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1);
            Rect.sizeDelta = new Vector2(0, height);

            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minHeight = height;
            layoutElement.preferredHeight = height;

            Controls = Add<App.UI.ControlBar.Index>("Controls");
            Controls.Panels.LayoutGroup.padding.top = Core.Specs.ControlBarMarginVertical;
            Controls.Panels.LayoutGroup.padding.bottom = Core.Specs.ControlBarMarginVertical;

            var controlBack = Controls.Panels.Left.Main.Add<App.UI.ControlBar.Controls.Left>("ControlBack");
            var buttonBack = controlBack.Button;
            buttonBack.Label.Text.font = Core.Fonts.Icon;
            buttonBack.Label.Text.fontSize = Core.Fonts.Size.Smaller1;
            buttonBack.Label.Text.color = Core.Colors.Light;
            buttonBack.Label.Text.text = "MEZZANINE DATA MODELS";
        }
    }
}
