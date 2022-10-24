using UnityEngine;
using UnityEngine.UI;

namespace Mz.DemoDataModels.Screens.Home.Packs.Gallery.Pack
{
    public class Index : MzBehaviour
    {
        public ScrollRect ScrollRect { get; private set; }
        public Content Content { get; private set; }
        public int PackKey { get; private set; }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            AddRect();
            Rect.anchorMin = new Vector2(0, 0);
            Rect.anchorMax = new Vector2(1, 1);
            Rect.pivot = new Vector2(0.5f, 1.0f);
            
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = Core.ScreenWidth;
            layoutElement.preferredWidth = Core.ScreenWidth;
            layoutElement.minHeight = Core.ScreenHeight;
            layoutElement.preferredHeight = Core.ScreenHeight;

            // Needed so user can scroll by dragging on background.
            var image = gameObject.AddComponent<Image>();
            image.color = new Color(0, 0, 0, 0);
            
            Content = Add<Content>("Content");
            Content.Levels.LevelClicked += (sender, index) =>
            {
                OnLevelClicked(index);
            };
            
            ScrollRect = gameObject.AddComponent<ScrollRect>();
            ScrollRect.horizontal = false;
            ScrollRect.viewport = Rect;
            ScrollRect.content = Content.Rect;

            gameObject.AddComponent<RectMask2D>();
        }

        private bool _isLoaded;
        public void LoadData(Models.PackData packData)
        {
            PackKey = packData.Key;
            
            if (!_isLoaded) Content.Info.LoadData(packData);
            Content.Levels.LoadData(packData);

            _isLoaded = true;
        }

        private void OnLevelClicked(int index)
        {
            Core.LoadLevel(PackKey, index + 1);
        }
    }
}