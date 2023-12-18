using Sirenix.OdinInspector;
using UnityEngine;
using VolatileVoodoo.Runtime.Audio.Base;

namespace VolatileVoodoo.Runtime.Audio
{
    [CreateAssetMenu(fileName = "LoopingAudioEffect", menuName = "Voodoo/Audio/LoopingAudioEffect")]
    public class LoopingAudioEffect : BaseAudioEffect
    {
        [Required]
        public AudioClip loop;

        public override bool Init(AudioSource source)
        {
            if (!audioClipsValid) {
                return false;
            }

            base.Init(source);

            source.clip = loop;
            source.loop = true;
            return true;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            audioClipsValid = loop != null;
        }
#endif
    }
}