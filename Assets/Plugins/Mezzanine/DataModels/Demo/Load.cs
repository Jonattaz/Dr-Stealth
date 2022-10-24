using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mz.DemoDataModels
{
    public class Load
    {
        // This Runs automatically at runtime, before the default scene is loaded.
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    
        // With this method, we just want to add a default GameObject to the
        // main scene and attach our App script to it, so that it can do its thing.
        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name != "DemoScene_MzDataModels") return;
            var gameObject = new GameObject("App");
            gameObject.transform.SetParent(null, false);
            gameObject.AddComponent<CoreDemoDataModels>();
        }
    }
}