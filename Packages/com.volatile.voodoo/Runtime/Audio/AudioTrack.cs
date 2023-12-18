using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using VolatileVoodoo.Runtime.Audio.Base;

namespace VolatileVoodoo.Runtime.Audio
{
    [CreateAssetMenu(fileName = "AudioTrack", menuName = "Voodoo/Audio/AudioTrack")]
    public class AudioTrack : BaseAudioEffect
    {
        [Required]
        public AudioClip track;

        [Required]
        public AudioMixerGroup outputMixerGroup;

        public float fadeOutDuration = 1f;

        [HideInInspector]
        public string trackName;

        public override bool Init(AudioSource source)
        {
            if (!audioClipsValid) {
                return false;
            }

            base.Init(source);

            source.clip = track;
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            trackName = name;
            audioClipsValid = track != null;
        }
#endif
    }
}