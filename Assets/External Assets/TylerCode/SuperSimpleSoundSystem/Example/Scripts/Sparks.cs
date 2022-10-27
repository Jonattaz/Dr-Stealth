using UnityEngine;
using TylerCode.SoundSystem;

namespace TylerCode.Examples
{
    public class Sparks : MonoBehaviour
    {
        [SerializeField]
        private float _repeatDuration;
        private S4SoundSource _soundSource;

        void Start()
        {
            _soundSource = GetComponent<S4SoundSource>();
            Invoke("PlaySound", 0);
        }

        private void PlaySound()
        {
            Invoke("PlaySound", _repeatDuration);

            _soundSource.PlaySound("Sparks");
        }
    }
}