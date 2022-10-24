using UnityEngine;
using Mz.App.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack.Levels
{
    public class LevelGridCell : MzBehaviour
    {
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Image = gameObject.AddComponent<ImageRounded>();
            
            Label = Add<App.UI.Button.Label>("Label");
            Label.Text.font = Core.Fonts.MainBold;
            Label.Text.fontSize = Core.Fonts.Size.Smaller1;
            Label.Text.color = Core.Colors.Light;
        }
        
        public ImageRounded Image { get; private set; }
        public App.UI.Button.Label Label { get; private set; }

        public void SetState(int value)
        {
            Label.Text.font = Core.Fonts.MainBold;
            Label.Text.fontSize = Core.Fonts.Size.Smaller1;
            
            switch (value)
            {
                case -2: // Ad Watched
                    Image.color = Core.Colors.MediumDark3;
                    Label.Text.color = Core.Colors.Dark;
                    break;
                case -1: // Locked
                    Image.color = new Color(1f, 1f, 1f, 0f);
                    Label.Text.color = Core.Colors.MediumDark2;
                    break;
                case 0: // In progress
                    Image.color = Core.Colors.Highlight;
                    Label.Text.color = Core.Colors.Light;
                    break;
                case 1: // Fail. Completed. Mistakes.
                    Label.Text.font = Core.Fonts.Icon;
                    Label.Text.fontSize = Core.Fonts.Size.Larger2;
                    Image.color = Core.Colors.MediumDark3;
                    Label.Text.color = Core.Colors.Highlight;
                    break;
                case 2: // Good. Completed. No Mistakes. Hints.
                    Image.color = Core.Colors.MediumDark3;
                    Label.Text.color = Core.Colors.Light;
                    break;
                case 3: // Perfect. Completed. No Mistakes. No hints.
                    Label.Text.fontSize = Core.Fonts.Size.Larger2;
                    Image.color = Core.Colors.MediumDark3;
                    Label.Text.color = Core.Colors.Light;
                    break;
            }
        }
    }
}