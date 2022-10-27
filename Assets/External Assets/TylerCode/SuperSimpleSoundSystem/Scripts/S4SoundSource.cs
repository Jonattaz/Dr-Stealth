using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace TylerCode.SoundSystem
{
    /// <summary>
    /// This is the actual "Play a sound" class, add this to a game object and off you go.
    /// </summary>
    public class S4SoundSource : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Sound to play when object is created, can be empty for none.")]
        private string _soundToPlayOnAwake;
        [SerializeField]
        [Tooltip("List of sounds attached to this object, used to communicate with the sound manager")]
        private List<SoundPlayerSettings> _sounds;

        private S4SoundManager _soundManager;

        private void Start()
        {
            _soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<S4SoundManager>();

            if (string.IsNullOrEmpty(_soundToPlayOnAwake) == false)
            {
                PlaySound(_soundToPlayOnAwake);
            }
        }

        /// <summary>
        /// Plays a sound by its name and takes in an optional position override. This plays a sound player config by its name.
        /// </summary>
        /// <param name="name">Name of the sound to play</param>
        /// <param name="overridePosition">Positional override (optional)</param>
        public void PlaySound(string name, Vector3? overridePosition = null)
        {
            SoundPlayerSettings _soundPlayerSettings = _sounds.FirstOrDefault(sp => sp.soundName == name);

            if (overridePosition != null)
            {
                _soundPlayerSettings.positionToPlay = overridePosition.Value;
            }

            if (_soundPlayerSettings == null)
            {
                Debug.LogError("The sound " + name + " does not exist on this SoundSource!");
                return;
            }

            _soundManager.PlaySound(_soundPlayerSettings);
        }

        public void StopSound()
        {
            //TODO: Stopping sounds
        }
    }
}