using Mz.App.Procedural;
using UnityEngine;
using UnityEngine.UI;
using Mz.Numerics;

namespace Mz.App.UI
{
    public class Divider : MzBehaviourBase
    {
        public RawImage Image { get; set; }
        public Texture2D Texture { get; private set; }
        public Gradient Gradient { get; set; }
        
        public void SetSize(float x, float y)
        {
            Rect.sizeDelta = new Vector2(x, y);
            Image = gameObject.AddComponent<RawImage>();
            Texture = new Texture2D((int)Numbers.Floor(x), (int)Numbers.Floor(y));
            Image.texture = Texture;
        }

        public void SetGradient(float alphaLeft = 0.0f, float alphaRight = 0.2f)
        {
            Gradient.SetKeys(
                new [] { new GradientColorKey(UnityEngine.Color.white, 0.0f), new GradientColorKey(UnityEngine.Color.white, 1.0f) },
                new [] { new GradientAlphaKey(alphaLeft, 0.0f), new GradientAlphaKey(alphaRight, 1.0f) }
            );
            Texture.DrawGradient(Gradient, Direction.Right);
            Texture.Apply();
        }

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0, 1);
            
            var layoutElement = gameObject.AddComponent<LayoutElement>();
            layoutElement.minWidth = Core.ScreenWidth;
            layoutElement.minHeight = 2;
            layoutElement.preferredWidth = Core.ScreenWidth;
            layoutElement.preferredHeight = 2;
            
            SetSize(Core.ScreenWidth, 2);
            Gradient = new Gradient();
            SetGradient();
        }
    }
}