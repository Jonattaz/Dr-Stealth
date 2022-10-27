using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

namespace TylerCode.SoundSystem
{
    public class S4SoundManager : MonoBehaviour
    {
        //Overrides for volume, these are GLOBAL. Use the sound source volume to tweak specific instances
        [Tooltip("The GLOBAL music volume, wire up to your settings menu")]
        public float musicVolume = 1.0f;
        [Tooltip("The GLOBAL sound volume, wire up to your settings menu")]
        public float soundVolume = 1.0f;
        [SerializeField]
        [Tooltip("Turns on additional logging throughout the application")]
        private bool _debugMode = false;

        private S4Subtitles _subtitleEngine;
        private Dictionary<int, SoundObject> _sounds = new Dictionary<int, SoundObject>();
        private List<string> _globalSounds = new List<string>();
        private Action<string> _subtitleAction;
        private int _currentSong = 0;

        private void Awake()
        {
            GameObject[] managers = GameObject.FindGameObjectsWithTag("SoundManager");
            _subtitleEngine = FindObjectOfType<S4Subtitles>();

            if (managers.Length > 1)
            {
                Destroy(this.gameObject);
            }

            DontDestroyOnLoad(this.gameObject);
        }

        public void InitializeSubtitles(Action<string> subtitleAction)
        {
            _subtitleAction = subtitleAction;
        }

        private void OnLevelWasLoaded(int level)
        {
            _subtitleEngine = FindObjectOfType<S4Subtitles>();
        }

        public int PlaySound(SoundPlayerSettings playerSettings)
        {
            int playId = UnityEngine.Random.Range(10000, 99999); //Generating a psudorandom ID, will likely change to ULID

            //Once these are ULIDs we won't have to do this
            if (_sounds.ContainsKey(playId))
            {
                playId = UnityEngine.Random.Range(10000, 99999);
            }

            //Preparing the game object
            GameObject go = new GameObject();
            go.name = $"SoundPlayer({playerSettings.soundName})";
            S4SoundPlayer player = go.AddComponent<S4SoundPlayer>();

            //Overrides for global sounds to be attached to the camera instead of anything else
            if (playerSettings.globalSound)
            {
                if (_globalSounds.Contains(playerSettings.soundName) && playerSettings.looping && playerSettings.persistSceneChange == false)
                {
                    if (Application.isEditor && _debugMode)
                    {
                        Debug.LogWarning($"The sound {playerSettings.soundName} is a global and looping sound that is already playing. We are starting a new instance.");
                    }
                }
                if (_globalSounds.Contains(playerSettings.soundName) && playerSettings.looping && playerSettings.persistSceneChange)
                {
                    if (Application.isEditor && _debugMode)
                    {
                        Debug.LogWarning($"The sound {playerSettings.soundName} is a global, persistive, and looping sound that is already playing. We prevented a new one from starting.");
                    }

                    Destroy(go);
                    return 0;
                }

                playerSettings.parentObject = GameObject.FindObjectOfType<AudioListener>().transform;
                _globalSounds.Add(playerSettings.soundName);
            }

            //Subtitle/Closed Caption call. Comment exists for searchability
            //So you can attach your own closed caption system to S4, or use the built-in one. I'd 
            //recommend built-in unless you are doing something weird with your subs. 
            if (_subtitleAction != null)
            {
                _subtitleAction(playerSettings.closedCaption); // Calls out to a subtitle system to display captions
            }
            else
            {
                if (_subtitleEngine != null)
                    _subtitleEngine.DisplaySub(playerSettings.closedCaption);
            }

            if (playerSettings.isMusic)
            {
                StartNewSong(playerSettings, playId);
                player.StartPlayer(playerSettings, playId, this, true);
            }
            else
            {
                player.StartPlayer(playerSettings, playId, this);
            }

            //Finally, playing the sound and adding it to our table of sounds
            _sounds.Add(playId, new SoundObject(playerSettings, go));

            return playId;
        }

        //These volume modification functions work for both 
        public void ChangeMusicVolume(float multiplier)
        {
            musicVolume = multiplier;

            if (_sounds.ContainsKey(_currentSong))
            {
                _sounds[_currentSong].soundPlayerObject.GetComponent<AudioSource>().volume = _sounds[_currentSong].playerSettings.volume * musicVolume;
            }
        }

        public void ChangeSoundVolume(float multiplier)
        {
            soundVolume = multiplier;

            foreach (KeyValuePair<int, SoundObject> valuePair in _sounds)
            {
                if (valuePair.Value.playerSettings.isMusic == false)
                {
                    valuePair.Value.soundPlayerObject.GetComponent<AudioSource>().volume = _sounds[_currentSong].playerSettings.volume * soundVolume;
                }
            }
        }

        //Stops a sound by its ID
        public void StopSound(int id)
        {
            if (id == 0)
            {
                return;
            }

            _sounds[id].soundPlayerObject.GetComponent<S4SoundPlayer>().StopPlayer();
        }

        //Fades out a sound by its ID
        public void StopSound(int id, bool fadeOut)
        {
            if (id == 0)
            {
                return;
            }

            _sounds[id].soundPlayerObject.GetComponent<S4SoundPlayer>().StopPlayer(fadeOut);
        }

        //Removes a sound from the sound table
        public void RemoveSound(int id)
        {
            if (_sounds.ContainsKey(id))
            {
                _sounds.Remove(id);
            }
        }

        private void StartNewSong(SoundPlayerSettings soundPlayerSettings, int id)
        {
            if (_sounds.ContainsKey(_currentSong))
            {
                _globalSounds.Remove(_sounds[_currentSong].playerSettings.soundName);
            }

            StopSound(_currentSong, true);
            _currentSong = id;
        }
    }

    /// <summary>
    /// This keeps track of our game objects out in the world. Lets us see how long they have been playing etc.
    /// 
    /// It was done this way so we can just keep the gameobjects referenced for easy destruction on level loading should we need it.
    /// </summary>
    public class SoundObject
    {
        public SoundPlayerSettings playerSettings;
        public GameObject soundPlayerObject;
        public DateTime startTime;

        public SoundObject(SoundPlayerSettings settings, GameObject obj)
        {
            playerSettings = settings;
            soundPlayerObject = obj;
            startTime = DateTime.Now;
        }
    }

    public static class FadeAudioSource
    {
        public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
        {
            float currentTime = 0;
            float start = audioSource.volume;

            while (currentTime < duration)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
                yield return null;
            }
            yield break;
        }
    }

}