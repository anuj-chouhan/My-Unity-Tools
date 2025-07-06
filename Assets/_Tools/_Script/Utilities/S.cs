using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Anuj.Utility.Audio
{
    /// <summary>
    /// Configuration data for playing an audio clip using the audio manager.
    /// Supports 2D/3D settings, optional pooling, looping, and parenting.
    /// </summary>
    public class PlaySoundData
    {
        /// <summary>
        /// The AudioClip to be played.
        /// </summary>
        public AudioClip AudioClip = null;

        /// <summary>
        /// The AudioMixerGroup to route the audio through (e.g., SFX, Music, UI).
        /// </summary>
        public AudioMixerGroup AudioMixer = null;

        /// <summary>
        /// The transform to parent the audio GameObject to.
        /// Useful for following objects (like characters or UI).
        /// </summary>
        public Transform Parent = null;

        /// <summary>
        /// Whether the audio should loop continuously.
        /// </summary>
        public bool Loop = false;

        /// <summary>
        /// Playback volume (0 to 1).
        /// </summary>
        public float Volume = 1;

        /// <summary>
        /// Whether to use audio source pooling.
        /// If true, will reuse audio sources to improve performance.
        /// </summary>
        public bool UsePooling = true;

        /// <summary>
        /// Optional 3D audio configuration.
        /// If null, audio is played in 2D mode.
        /// </summary>
        public Audio3D AudioThreeD = null;

        /// <summary>
        /// Optional settings for playing audio as 3D spatial sound.
        /// </summary>
        public class Audio3D
        {
            /// <summary>
            /// The world position where the audio should play from.
            /// </summary>
            public Vector3? Position = null;

            /// <summary>
            /// Spatial blend between 2D and 3D.
            /// 0 = fully 2D, 1 = fully 3D.
            /// </summary>
            public float SpatialBlend = 1;
        }
    }

    public class S : MonoBehaviour
    {
        [Header("Pooling Settings")]
        [SerializeField] private bool usePooling = true;

        [ShowIf(nameof(usePooling))]
        [SerializeField] private int poolSize = 10;

        [Header("Audio Mixers")]
        public AudioMixerGroup audioMixerGroupMusic;
        public AudioMixerGroup audioMixerGroupSFX;
        public AudioMixerGroup audioMixerGroupUI;

        [Header("Audio Clips")]
        public AudioClip audioClip1;
        public AudioClip audioClip2;
        public AudioClip audioClip3;
        public AudioClip audioClip4;
        public AudioClip audioClip5;

        private List<AudioSource> _listNotPooledAudioSource = new List<AudioSource>();
        private Queue<AudioSource> _queuePooledAudioSource = new Queue<AudioSource>();

        public static S I;

        private void OnValidate()
        {
            if (poolSize <= 1)
            {
                poolSize = 1;
            }
        }

        private void Awake()
        {
            if (I == null)
            {
                I = this;

                if (usePooling)
                {
                    InitializePool();
                }

                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializePool()
        {
            _queuePooledAudioSource = new Queue<AudioSource>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = new GameObject("PooledAudio");
                AudioSource AddedAudioSource = obj.AddComponent<AudioSource>();
                obj.transform.SetParent(this.transform);
                obj.SetActive(false);
                _queuePooledAudioSource.Enqueue(AddedAudioSource);
            }
        }
        private IEnumerator ReturnToPool(AudioSource audioSource, float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);
            audioSource.transform.SetParent(this.transform);

            if (!_queuePooledAudioSource.Contains(audioSource))
            {
                _queuePooledAudioSource.Enqueue(audioSource);
            }
        }

        private void DestroyAudioSource(AudioSource audioSource, float delay = 0)
        {
            _listNotPooledAudioSource.Remove(audioSource);
            Destroy(audioSource.gameObject, delay);
        }

        /// <summary>
        /// Use this method to stop all the audios
        /// </summary>
        public void StopAllSounds()
        {
            foreach (AudioSource pooledAudioSource in _queuePooledAudioSource)
            {
                pooledAudioSource.Stop();
                pooledAudioSource.gameObject.SetActive(false);
            }

            foreach (AudioSource notPooledAudioSource in _listNotPooledAudioSource)
            {
                DestroyAudioSource(notPooledAudioSource);
            }
        }

        /// <summary>
        /// Plays a sound with full config (3D, looping, pooling), and returns an Action to manually stop it early.
        /// </summary>
        /// <param name="playSoundData">All configuration for the audio playback</param>
        /// <returns>Action to stop this audio before it finishes</returns>
        public Action PlaySound(PlaySoundData playSoundData)
        {
            if (playSoundData.AudioClip == null)
            {
                Debug.LogWarning("PlaySoundData has null AudioClip");
                return null;
            }

            Coroutine returnToPoolRoutine = null;
            AudioSource AudioSource;

            if (playSoundData.UsePooling && usePooling) // POOLING MODE
            {
                AudioSource = _queuePooledAudioSource.Dequeue();
                AudioSource.name = "Audio (Pooled): " + playSoundData.AudioClip.name;
                AudioSource.gameObject.SetActive(true);

                // Automatically return to pool if not looping
                if (!playSoundData.Loop)
                {
                    returnToPoolRoutine = StartCoroutine(ReturnToPool(AudioSource, playSoundData.AudioClip.length));
                }
            }
            else // NON-POOLING MODE: Create fresh GameObject and destroy it after playback
            {
                GameObject Audio = new GameObject("Audio: " + playSoundData.AudioClip.name);
                AudioSource = Audio.AddComponent<AudioSource>();

                if (playSoundData.Parent == null)
                {
                    playSoundData.Parent = this.transform;
                }

                _listNotPooledAudioSource.Add(AudioSource);

                if (!playSoundData.Loop)
                {
                    if (AudioSource != null)
                    {
                        DestroyAudioSource(AudioSource, playSoundData.AudioClip.length);
                    }
                }
            }

            // CONFIGURE AUDIO SOURCE
            AudioSource.clip = playSoundData.AudioClip;
            AudioSource.volume = playSoundData.Volume;
            AudioSource.outputAudioMixerGroup = playSoundData.AudioMixer;
            AudioSource.transform.SetParent(playSoundData.Parent);
            AudioSource.loop = playSoundData.Loop;

            // Handle 3D or 2D spatial audio
            if (playSoundData.AudioThreeD != null)
            {
                AudioSource.spatialBlend = playSoundData.AudioThreeD.SpatialBlend;
                AudioSource.transform.position = playSoundData.AudioThreeD.Position.Value;
            }
            else
            {
                AudioSource.spatialBlend = 0; // 2D sound
                AudioSource.transform.position = Vector3.zero;
            }

            // PLAY SOUND
            AudioSource.Play();

            // Return stop action to let the developer stop it early
            if (playSoundData.UsePooling && usePooling)
            {
                return () =>
                {
                    if (returnToPoolRoutine != null)
                    {
                        StopCoroutine(returnToPoolRoutine);
                    }

                    ReturnToPool(AudioSource);
                };
            }
            else
            {
                return () =>
                {
                    DestroyAudioSource(AudioSource);
                };
            }
        }
    }

}