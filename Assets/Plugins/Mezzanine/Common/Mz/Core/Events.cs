using UnityEngine.EventSystems;

namespace Mz.App
{
    public class Events : MzBehaviourBase
    {
        protected override void OnInitializeStarted()
        {
            base.OnInitializeStarted();
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }
}