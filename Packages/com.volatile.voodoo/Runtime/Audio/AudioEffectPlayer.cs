using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using VolatileVoodoo.Runtime.Audio.Base;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodoo.Runtime.Audio
{
    public class AudioEffectPlayer : MonoBehaviour
    {
        private static Queue<AudioSource> audioSources;

        public AudioMixerGroup audioGroup;

#if UNITY_EDITOR
        [OnValueChanged(nameof(OnRolloffModeChanged))]
#endif
        public AudioRolloffMode rolloffMode = AudioRolloffMode.Logarithmic;

#if UNITY_EDITOR
        [OnValueChanged(nameof(OnMinDistanceChanged))]
#endif
        [PropertyRange(1f, nameof(maxDistance))]
        public float minDistance = 1f;

#if UNITY_EDITOR
        [OnValueChanged(nameof(OnMaxDistanceChanged))]
#endif
        [PropertyRange(nameof(minDistance), 500f)]
        public float maxDistance = 500f;

        public List<BaseAudioEffect> effects;
        private List<PlayingSource> playingSources;

        private void Awake()
        {
            playingSources = new List<PlayingSource>();
        }

        private void LateUpdate()
        {
            // clean up non looping not playing sources, looping sources must explicitly be set to !enabled to allow recovery of paused looping sources on focus loss  
            var finished = playingSources.Where(item => !item.Source.enabled || (!item.Source.loop && !item.Source.isPlaying)).Select(PutIntoAudioSourcePool).ToArray();
            foreach (var item in finished)
                playingSources.Remove(item);
        }

        private void OnDestroy()
        {
            playingSources.ForEach(source => PutIntoAudioSourcePool(source));
            playingSources.Clear();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                return;

            // unpause potentially paused looping sources
            var paused = playingSources.Where(item => item.Source.enabled && item.Source.loop && !item.Source.isPlaying).ToArray();
            foreach (var item in paused)
                item.Source.UnPause();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateAudioSourcePool()
        {
            if (!Application.isPlaying)
                return;

            audioSources = new Queue<AudioSource>(Voodoo.AudioSourcePoolSize);
            for (var i = 0; i < Voodoo.AudioSourcePoolSize; ++i) {
                var go = new GameObject("AudioEffect") {
                    hideFlags = HideFlags.HideAndDontSave
                };
                DontDestroyOnLoad(go);

                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                source.loop = false;
                source.spatialBlend = 1f;
                source.enabled = false;
                audioSources.Enqueue(source);
            }
        }

        private bool GrabFromAudioSourcePool(out AudioSource source)
        {
            if (!audioSources.TryDequeue(out source))
                return false;

            source.enabled = true;
            source.outputAudioMixerGroup = audioGroup;
            source.rolloffMode = rolloffMode;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;

            var tf = source.transform;
            tf.parent = transform;
            tf.localPosition = Vector3.zero;

            return true;
        }

        private static PlayingSource PutIntoAudioSourcePool(PlayingSource playingSource)
        {
            var source = playingSource.Source;
            source.clip = null;
            source.outputAudioMixerGroup = null;
            source.loop = false;
            source.volume = 1f;
            source.pitch = 1f;
            source.spatialBlend = 0f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            source.minDistance = 1f;
            source.maxDistance = 500f;
            source.enabled = false;

            var tf = source.transform;
            tf.parent = null;
            tf.position = Vector3.zero;

#if UNITY_EDITOR
            var go = source.gameObject;
            go.hideFlags |= HideFlags.HideInHierarchy;
            go.name = go.name[..go.name.IndexOf('[')];
#endif

            audioSources.Enqueue(source);
            return playingSource;
        }

        public void PlayFirst()
        {
            Play();
        }

        public void Play(string effectName = "")
        {
            if (!GrabFromAudioSourcePool(out var source))
                return;

#if UNITY_EDITOR
            var go = source.gameObject;
            go.hideFlags &= ~HideFlags.HideInHierarchy;
            go.name += "[" + effectName + "]";
#endif

            if (effects.FirstOrDefault(item => string.IsNullOrWhiteSpace(effectName) || item.name.Equals(effectName))?.Init(source) ?? false)
                source.Play();

            playingSources.Add(new PlayingSource { EffectName = effectName, Source = source });
        }

        public void Stop(string effectName)
        {
            var playing = playingSources.Where(item => item.EffectName.Equals(effectName)).ToArray();
            foreach (var item in playing) {
                item.Source.Stop();
                item.Source.enabled = false;
            }
        }

        public void StopAll()
        {
            foreach (var item in playingSources) {
                item.Source.Stop();
                item.Source.enabled = false;
            }
        }

        private struct PlayingSource
        {
            public string EffectName;
            public AudioSource Source;
        }

#if UNITY_EDITOR
        private void OnRolloffModeChanged()
        {
            if (playingSources is not { Count: > 0 })
                return;

            var currentlyPlaying = playingSources.Where(item => item.Source != null && item.Source.enabled);
            foreach (var item in currentlyPlaying)
                item.Source.rolloffMode = rolloffMode;
        }

        public void OnMinDistanceChanged()
        {
            if (playingSources is not { Count: > 0 })
                return;

            var currentlyPlaying = playingSources.Where(item => item.Source != null && item.Source.enabled);
            foreach (var item in currentlyPlaying)
                item.Source.minDistance = minDistance;
        }

        public void OnMaxDistanceChanged()
        {
            if (playingSources is not { Count: > 0 })
                return;

            var currentlyPlaying = playingSources.Where(item => item.Source != null && item.Source.enabled);
            foreach (var item in currentlyPlaying)
                item.Source.maxDistance = maxDistance;
        }
#endif
    }
}