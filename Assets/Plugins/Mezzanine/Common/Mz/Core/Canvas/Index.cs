using UnityEngine;
using UnityEngine.UI;

namespace Mz.App.Canvas
{
    public class Index : MzBehaviourBase
    {
        public Color BackgroundColor
        {
            get => _image.color;
            set => _image.color = value;
        }

        private UnityEngine.UI.Image _image;
        
        public void PreInitialize(Camera camera, float screenWidth, float screenHeight)
        {
            // This stuff needs to be done before initialization happens.
            
            var canvas = gameObject.AddComponent<UnityEngine.Canvas>();
            canvas.worldCamera = camera;
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            var canvasScaler = gameObject.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(screenWidth, screenHeight);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            canvasScaler.matchWidthOrHeight = 1.0f;

            var raycaster = gameObject.AddComponent<GraphicRaycaster>();
            raycaster.enabled = true;

            // This is necessary for ImageRounded
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord2;
            canvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord3;
            
            _image = gameObject.AddComponent<UnityEngine.UI.Image>();
            // NOTE: alpha needs to be 0 here.
            _image.color = new Color(0, 0, 0, 0);

            Rect = gameObject.GetComponent<RectTransform>();
        }
    }
}