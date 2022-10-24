using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mz.App.UI.Button
{
    public enum ButtonType
    {
        Flat,
        Image,
        Invisible
    }
    public class Index : MzBehaviourBase, IPointerDownHandler, IPointerUpHandler {
        public Color BackgroundColor
        {
            get
            {
                switch (Type)
                {
                    case ButtonType.Flat:
                        var imageRounded = gameObject.GetComponent<ImageRounded>();
                        return imageRounded.color;
                    case ButtonType.Image:
                        var rawImage = gameObject.GetComponent<RawImage>();
                        return rawImage.color;
                    case ButtonType.Invisible:
                        return new Color(0, 0, 0, 0);
                }

                return Core.Colors.Highlight;
            }
            
            set
            {
                switch (Type)
                {
                    case ButtonType.Flat:
                        var imageRounded = gameObject.GetComponent<ImageRounded>();
                        imageRounded.color = value;
                        break;
                    case ButtonType.Image:
                        var rawImage = gameObject.GetComponent<RawImage>();
                        rawImage.color = value;
                        break;
                }
            }
        }

        public UnityEngine.UI.Button Component { get; set; }
        public Label Label { get; private set; }
        public ButtonType Type { get; private set; } = ButtonType.Flat;

        public float Width
        {
            get => Rect.sizeDelta.x;
            set => Rect.sizeDelta = new Vector2(value, Rect.sizeDelta.y);
        }
        
        public float Height
        {
            get => Rect.sizeDelta.y;
            set => Rect.sizeDelta = new Vector2(Rect.sizeDelta.x, value);
        }

        private string _resourceImagePath;
        public void Initialize(CoreBase core, ButtonType type, string resourceImagePath = null)
        {
            Type = type;
            _resourceImagePath = resourceImagePath;
            Initialize(core);
        }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.pivot = new Vector2(0.5f, 0.5f);
            Rect.sizeDelta = new Vector2(480, 120);
            Rect.anchoredPosition = new Vector2(0, 0);

            if (Type == ButtonType.Image && !string.IsNullOrEmpty(_resourceImagePath))
            {
                var resourceFilePath = _resourceImagePath;
                var rawImage = gameObject.AddComponent<RawImage>();
                var texture = Resources.Load(resourceFilePath) as Texture2D;
                if (texture != null) rawImage.texture = texture;
                rawImage.raycastTarget = true;
            }
            else if (Type == ButtonType.Invisible)
            {
                var imageRounded = gameObject.AddComponent<ImageRounded>();
                imageRounded.color = new Color(0, 0, 0, 0);
                imageRounded.raycastTarget = true;
            }
            else if (Type == ButtonType.Flat)
            {
                var imageRounded = gameObject.AddComponent<ImageRounded>();
                imageRounded.color = Core.Colors.Highlight;
                imageRounded.raycastTarget = true;
            } 

            Component = gameObject.AddComponent<UnityEngine.UI.Button>();
            if (Type == ButtonType.Invisible) Component.transition = Selectable.Transition.None;

            Label = Add<Label>("Label");
            Label.Text.font = Core.Fonts.MainSemibold;
            Label.Text.fontSize = Core.Fonts.Size.Larger1;
            Label.Text.color = Core.Colors.Dark;
        }
        
        public delegate void PointerEventHandler(object sender, PointerEventData eventData);
        public event PointerEventHandler PointerDown;
        public event PointerEventHandler PointerUp;

        public void OnPointerDown(PointerEventData eventData){
            PointerDown?.Invoke(this, eventData);
        }
        
        public void OnPointerUp(PointerEventData eventData){
            PointerUp?.Invoke(this, eventData);
        }
      
    }
}
