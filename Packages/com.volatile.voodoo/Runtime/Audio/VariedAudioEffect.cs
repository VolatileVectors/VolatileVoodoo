using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using VolatileVoodoo.Runtime.Audio.Base;
using Random = UnityEngine.Random;

namespace VolatileVoodoo.Runtime.Audio
{
    [CreateAssetMenu(fileName = "VariedAudioEffect", menuName = "Voodoo/Audio/VariedAudioEffect")]
    public class VariedAudioEffect : BaseAudioEffect
    {
        [EnableIf("@this.clips.Length > 2")]
        public AudioClipOrder order = AudioClipOrder.Continuous;

        [Required]
        public AudioClip[] clips;

        [SerializeField]
        [HideInInspector]
        private int maxClipIndex = 1;

        private int clipIndex = -1;

#if UNITY_EDITOR
        private void OnValidate()
        {
            audioClipsValid = clips.Length > 0 && clips.Select(item => item != null).Aggregate(true, (a, b) => a && b);
            if (clips.Length <= 2)
                order = AudioClipOrder.Continuous;

            maxClipIndex = clips.Length > 1 ? maxClipIndex = clips.Length - 1 : 1;
        }
#endif

        public override bool Init(AudioSource source)
        {
            if (!audioClipsValid)
                return false;

            base.Init(source);

            switch (order) {
                case AudioClipOrder.Continuous:
                    clipIndex = ++clipIndex % clips.Length;
                    source.clip = clips[clipIndex];
                    return true;
                case AudioClipOrder.FlipFlop:
                    clipIndex = ++clipIndex % (maxClipIndex * 2);
                    source.clip = clips[maxClipIndex - Math.Abs(clipIndex - maxClipIndex)];
                    return true;
                case AudioClipOrder.Random:
                default:
                    clipIndex = (clipIndex % clips.Length + Random.Range(1, maxClipIndex)) % clips.Length;
                    source.clip = clips[clipIndex];
                    return true;
            }
        }
    }
}