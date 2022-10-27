using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TylerCode.SoundSystem
{
    /// <summary>
    /// This class is responsible for actually playing the sound. You shouldn't ever really worry about this. 
    /// </summary>
    public class S4SoundPlayer : MonoBehaviour
    {
        [SerializeField]
        private SoundPlayerSettings _soundPlayerSettings;
        [SerializeField]
        private int _playerID = 0;

        private bool _naturalDeath = false; //Lets the player know if it's being destroyed as part of a scene cleanup

        //This function should be cleaned up instead of a ton of if statements. 
        /// <summary>
        /// Starts playing a sound by adding an audio source to this object with the proper configuration and starting the sound. 
        /// </summary>
        /// <param name="player">The sound player settings for this sound.</param>
        /// <param name="id">The ID assigned to this sound from the S4 manager.</param>
        /// <param name="manager">The S4 Sound Manager controlling the operation.</param>
        /// <param name="fadeIn">Should this sound have a 3 second fade-in? (Optional)</param>
        public void StartPlayer(SoundPlayerSettings player, int id, S4SoundManager manager, bool fadeIn = false)
        {
            _soundPlayerSettings = player;
            float desiredVolume = 1;

            if (_soundPlayerSettings.parentObject != null)
            {
                this.transform.position = (_soundPlayerSettings.parentObject.position + _soundPlayerSettings.positionToPlay);
                this.transform.parent = _soundPlayerSettings.parentObject;
            }
            else
            {
                this.transform.position = _soundPlayerSettings.positionToPlay;
            }

            if (player.isMusic)
            {
                desiredVolume = _soundPlayerSettings.volume * manager.musicVolume;
            }
            else
            {
                desiredVolume = _soundPlayerSettings.volume * manager.soundVolume;
            }

            AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();

            audioSource.clip = _soundPlayerSettings.audioClip;
            audioSource.loop = _soundPlayerSettings.looping;

            if (player.randomPitch)
            {
                audioSource.pitch = Random.Range(player.minPitch, player.maxPitch);
            }
            else
            {
                audioSource.pitch = _soundPlayerSettings.minPitch;
            }

            if (fadeIn)
            {
                audioSource.volume = 0;
                StartCoroutine(FadeAudioSource.StartFade(audioSource, 3, desiredVolume));
            }
            else
            {
                audioSource.volume = desiredVolume;
            }

            if (_soundPlayerSettings.globalSound)
            {
                this.transform.parent = null;
                audioSource.rolloffMode = AudioRolloffMode.Linear;
                audioSource.minDistance = 10000;
                audioSource.maxDistance = 10000;
                audioSource.dopplerLevel = 0;
            }

            if (_soundPlayerSettings.looping == false)
            {
                Invoke("EndOfSound", _soundPlayerSettings.audioClip.length);
            }

            if (_soundPlayerSettings.persistSceneChange)
            {
                DontDestroyOnLoad(this.gameObject);
            }

            audioSource.Play();
        }

        /// <summary>
        /// Exists because you can't invoke a method with optional arguments. 
        /// </summary>
        private void EndOfSound()
        {
            StopPlayer();
        }

        /// <summary>
        /// Stops the sound/song from playing
        /// </summary>
        /// <param name="fade">Should this sound fade out over 3 seconds? (Optional)</param>
        public void StopPlayer(bool fade = false)
        {
            S4SoundManager manager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<S4SoundManager>();

            AudioSource audioSource = GetComponent<AudioSource>();

            if (fade)
            {
                StartCoroutine(FadeAudioSource.StartFade(audioSource, 3, 0));
                Invoke("StopSound", 3);
            }
            else
            {
                StopSound();
            }

            manager.RemoveSound(_playerID);
            _naturalDeath = true;
        }

        public void OnDestroy()
        {
            if (_naturalDeath == false && _playerID != 0)
            {
                StopPlayer();
            }
        }

        private void StopSound()
        {
            AudioSource audioSource = GetComponent<AudioSource>();

            audioSource.Stop();
            Destroy(this.gameObject);
        }
    }

    [System.Serializable]
    public class SoundPlayerSettings
    {
        public string soundName;
        public string closedCaption;
        public AudioClip audioClip;
        public Vector3 positionToPlay;
        public Transform parentObject;
        public bool looping;
        public SoundPlayerSettings nextSound;
        public float volume;
        public bool randomPitch = false;
        public float minPitch = 1;
        public float maxPitch = 1;
        public bool isMusic;
        public bool persistSceneChange; //Calls DontDestroyOnLoad on the object and persists between scenes
        public bool globalSound; //Used when the sound is played at the same position as the listener

        public SoundPlayerSettings(string name, string caption, AudioClip clip, Vector3 position, float vol, bool loop = false, Transform parent = null, SoundPlayerSettings next = null, bool persist = false, bool music = false, float pitchMin = 1, float pitchMax = 1, bool randomizePitch = false)
        {
            soundName = name;
            closedCaption = caption;
            audioClip = clip;
            positionToPlay = position;
            parentObject = parent;
            looping = loop;
            nextSound = next;
            minPitch = pitchMin;
            maxPitch = pitchMax;
            randomPitch = randomizePitch;
            volume = vol;
            isMusic = music;
            persistSceneChange = persist;
        }
    }
}