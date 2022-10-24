using UnityEngine;
using UnityEngine.UI;

namespace Mz.App
{
    public class FrameRateCounter : MzBehaviourBase
    {
        public bool IsEnabled { get; set; }
        public Text Text { get; private set; }
        const float FpsMeasurePeriod = 0.5f;
        const string DisplayString = "{0} FPS";
        
        private int _fpsAccumulator = 0;
        private float _fpsNextPeriod = 0;
        private int _currentFps;

        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            
            AddRect();
            Rect.anchorMin = new Vector2(0, 1);
            Rect.anchorMax = new Vector2(0, 1);
            Rect.pivot = new Vector2(0, 1);
            Rect.sizeDelta = new Vector2(480, 120);
            Rect.anchoredPosition = new Vector2(80, -80);

            _fpsNextPeriod = Time.realtimeSinceStartup + FpsMeasurePeriod;
            Text = gameObject.AddComponent<Text>();
            Text.horizontalOverflow = HorizontalWrapMode.Overflow;
            Text.verticalOverflow = VerticalWrapMode.Overflow;
            Text.font = Core.Fonts.MainSemibold;
            Text.fontSize = Core.Fonts.Size.Larger1;
        }

        private void Update()
        {
            if (!IsEnabled)
            {
                if (Text != null && Text.text != "") Text.text = "";
                return;
            }
            
            // measure average frames per second
            _fpsAccumulator++;
            
            if (!(Time.realtimeSinceStartup > _fpsNextPeriod)) return;
            
            _currentFps = (int) (_fpsAccumulator/FpsMeasurePeriod);
            _fpsAccumulator = 0;
            _fpsNextPeriod += FpsMeasurePeriod;

            if (Text != null && Text.text != null)
            {
                Text.text = string.Format(DisplayString, _currentFps);
            }
        }
    }
}