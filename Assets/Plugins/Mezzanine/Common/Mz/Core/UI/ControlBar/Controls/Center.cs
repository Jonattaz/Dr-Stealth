using UnityEngine;
using UnityEngine.UI;
using Mz.App.UI.Button;

namespace Mz.App.UI.ControlBar.Controls
{
    public class Center : MzBehaviourBase
    {
        public Center()
        {
            Alignment = AlignmentType.Center;
        }

        public enum AlignmentType
        {
            Left,
            Center,
            Right
        }
        
        public Button.Index Button { get; private set; }
        public LayoutElement LayoutElement { get; private set; }

        public AlignmentType Alignment { get; set; }

        public float Width
        {
            get => LayoutElement.preferredWidth;
            set => LayoutElement.preferredWidth = value;
        }

        private ButtonType _buttonType = ButtonType.Flat;
        private string _resourceImagePath;
        public void Initialize(CoreBase core, ButtonType buttonType, string resourceImagePath = null)
        {
            _buttonType = buttonType;
            _resourceImagePath = resourceImagePath;
            Initialize(core);
        }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            
            LayoutElement = gameObject.AddComponent<LayoutElement>();
            LayoutElement.minWidth = 120;
            LayoutElement.preferredWidth = 120;
            
            Button = Add<Button.Index>("Button", false);
            Button.Initialize(Core, _buttonType, _resourceImagePath);
            
            Button.AddRect();
            Button.Rect.anchorMin = new Vector2(0, 0);
            Button.Rect.anchorMax = new Vector2(1, 1);
            Button.Rect.sizeDelta = new Vector2(0, 0);
            Button.BackgroundColor = new UnityEngine.Color(1, 1, 1, 0);
            Button.Label.Text.color = Core.Colors.Light;
            Button.Label.Text.font = Core.Fonts.Icon;
            Button.Label.Text.fontSize = Core.Fonts.Size.Icon2;
            Button.Label.Text.text = "";
            
            switch (Alignment)
            {
                case AlignmentType.Left:
                    Rect.anchorMin = new Vector2(0, 1);
                    Rect.anchorMax = new Vector2(0, 1);
                    Rect.pivot = new Vector2(0, 1);
                    
                    Button.Label.Text.alignment = TextAnchor.MiddleLeft;
                    break;
                case AlignmentType.Center:
                    Rect.anchorMin = new Vector2(0.5f, 1);
                    Rect.anchorMax = new Vector2(0.5f, 1);
                    Rect.pivot = new Vector2(0.5f, 1);
                    
                    Button.Label.Text.alignment = TextAnchor.MiddleCenter;
                    break;
                case AlignmentType.Right:
                    Rect.anchorMin = new Vector2(1, 1);
                    Rect.anchorMax = new Vector2(1, 1);
                    Rect.pivot = new Vector2(1, 1);
                    
                    Button.Label.Text.alignment = TextAnchor.MiddleRight;
                    break;
            }
        }
    }
}