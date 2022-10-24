using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Mz.Numerics;
using Mz.Unity;

namespace Mz.App
{
    public abstract class CoreBase : MzBehaviourBase
    {
        public CoreBase()
        {
            Colors = new Colors();
            Files = new Files();
            Fonts = new Fonts.Index(this);

            ScreenWidth = 1125;
            ScreenHeight = 2436;
        }

        protected Specs _specs;
        public Specs Specs => _specs;
        protected virtual void SetSpecs() { _specs = new Specs(this); }

        public Mz.App.CameraMain CameraMain { get; private set; }
        public Events Events { get; private set; }
        public Mz.App.Canvas.Index Canvas { get; private set; }
        public FrameRateCounter FrameRateCounter { get; private set; }

        public Colors Colors { get; }
        public Files Files { get; }
        public Mz.App.Fonts.Index Fonts { get; }

        public Vector2 ReferenceResolution { get; private set; }
        public float ScreenWidth { get; protected set; }
        public float ScreenHeight { get; protected set; }
        public float ScreenWidthActual { get; private set; }
        public float ScreenHeightActual { get; private set; }
        public float ScaleFactor { get; private set; }

        // IsFirstUse
        
        public bool IsFirstUse
        {
            get => PlayerPrefs.GetInt("IsFirstUse", 1) == 1;
            set
            {
                PlayerPrefs.SetInt("IsFirstUse", value ? 1 : 0);
                
#if UNITY_WEBGL
                // We need to call Save() for WebGL builds.
                PlayerPrefs.Save();
#endif
            }
        }

        public void Start()
        {
            // FPS
            Application.targetFrameRate = 60;
            
            // We need this GameObject to survive scene changes.
            DontDestroyOnLoad(gameObject);
            
            // Camera
            CameraMain = Add<Mz.App.CameraMain>("CameraMain");
            CameraMain.PreInitialize();

            // EventSystem
            Events = Add<Events>("Events");

            // Canvas
            Canvas = Add<Mz.App.Canvas.Index>("Canvas", false);
            Canvas.PreInitialize(CameraMain.Camera, ScreenWidth, ScreenHeight);
            
            // Frame counter
            FrameRateCounter = Canvas.Add<FrameRateCounter>("FrameRateCounter", false);

            // Initialize
            StartCoroutine(StartInitialize());

            Shader.EnableKeyword("UNITY_UI_CLIP_RECT");
        }

        public IEnumerator StartInitialize()
        {
            // Wait until the end of the first frame, then initialize.
            // We need to do this because Start() runs on the children
            // first, then recurses up through the parents. So, by 
            // waiting until the end of the first frame, we can be sure
            // that Update has been run and our global layout values have 
            // been properly initialized before we initialize children.
            yield return new WaitForEndOfFrame();

            _ContinueInitialize();

            yield return null;
        }

        private void _ContinueInitialize()
        {
            _Layout();
            Canvas.Initialize(this);

            // Presumably, fonts and colors will be initialized here.
            
            // Specs
            SetSpecs();
            
            Initialize(this);
            
            // FrameRateCounter can be initialized, now that fonts are available.
            FrameRateCounter.Initialize(this);
        }

        private void _Layout()
        {
            if (
                !(ScreenWidthActual <= 0) &&
                (!(ScreenWidthActual > 0) || ScreenWidthActual == Screen.width) &&
                !(ScreenHeightActual <= 0) &&
                (!(ScreenHeightActual > 0) || ScreenHeightActual == Screen.height)
            ) return;

            var canvasScaler = Canvas.GetComponent<CanvasScaler>();
            ReferenceResolution = canvasScaler.referenceResolution;

            ScreenWidthActual = (float) Screen.width / Screen.height * ReferenceResolution.y;
            ScreenHeightActual = ReferenceResolution.y;

            ScaleFactor = 1.0f;
        }
        
        public TComponent AddChild<TComponent>(GameObject parentGameObject, string name)
            where TComponent : UnityEngine.Component
        {
            var childGameObject = new GameObject("Landing");
            childGameObject.transform.SetParent(parentGameObject.transform, false);
            var child = childGameObject.AddComponent<TComponent>();
            return child;
        }

        public int ScaleValueInt(int value, bool isCeiling = true)
        {
            if (isCeiling) return (int) Numbers.Ceil(value * ScaleFactor);
            return (int) Numbers.Floor(value * ScaleFactor);
        }

        public float ScaleValue(float value)
        {
            return value * ScaleFactor;
        }
    }
}