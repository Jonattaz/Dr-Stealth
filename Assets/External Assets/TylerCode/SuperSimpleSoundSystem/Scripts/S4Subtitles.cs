using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace TylerCode.SoundSystem
{
    /// <summary>
    /// Simple subtitle system that I bundle with S4
    /// It still supports you using your own if you have advanced features
    /// you need. This is done by using the "InitializeSubtitles" function
    /// on the SoundManager object.
    /// </summary>
    public class S4Subtitles : MonoBehaviour
    {
        public bool subtitlesEnabled = true;

        [SerializeField]
        private Text _messageText;
        [SerializeField]
        private bool _fadeSubs = true;
        [SerializeField]
        private float _fadeSpeed = 0.05f;
        [SerializeField]
        private float _subtitleTime = 2;

        private void Start()
        {
            if (_messageText == null)
            {
                _messageText = GetComponent<Text>();
            }
        }

        public void DisplaySub(string caption)
        {
            _messageText.text = caption;
            Color c = Color.white;
            _messageText.color = c;

            if (_fadeSubs)
            {
                StartCoroutine(FadeOutText());
            }
            else
            {
                Invoke("StopShowingText", _subtitleTime);
            }
        }

        IEnumerator FadeOutText()
        {
            Color c = _messageText.color;
            for (float alpha = 1f; alpha >= 0; alpha -= (_fadeSpeed * Time.deltaTime))
            {
                c.a = alpha;
                _messageText.color = c;
                yield return null;
            }
        }

        IEnumerator FadeInText()
        {
            Color c = _messageText.color;
            for (float alpha = 0f; alpha <= 1; alpha += (_fadeSpeed * Time.deltaTime))
            {
                c.a = alpha;
                _messageText.color = c;
                yield return null;
            }
        }

        private void StopShowingText()
        {
            Color c = _messageText.color;
            c.a = 0;
            _messageText.color = c;
        }

        private void StartShowingText()
        {
            Color c = Color.white;
            _messageText.color = c;
        }
    }
}