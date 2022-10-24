using UnityEngine;

namespace Mz.DemoDataModels.Screens.Home
{
    public class Index : MzBehaviour
    {
        
        public Packs.Index Packs { get; private set; }
        
        public Color BackgroundColor
        {
            get => _image.color;
            set => _image.color = value;
        }

        private UnityEngine.UI.Image _image;
  
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0.5f, 0.5f);
            Rect.anchorMax = new Vector2(0.5f, 0.5f);
            Rect.anchoredPosition = new Vector2(0, 0);
            Rect.sizeDelta = new Vector2(Core.ScreenWidth, Core.ScreenHeight);
            Rect.pivot = new Vector2(0.5f, 0);
            
            Packs = Add<Packs.Index>("Packs");
            
            _image = gameObject.AddComponent<UnityEngine.UI.Image>();
            _image.color = Core.Colors.Dark;
        }
    }
}

