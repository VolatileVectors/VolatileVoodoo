using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace VolatileVoodoo.Runtime.Setters
{
    public class AudioMixerVolumeSetter : MonoBehaviour
    {
        [Tooltip("AudioMixer to set parameter on.")]
        [Required]
        public AudioMixer mixer;

        public void SetVolume(string parameterName, float value)
        {
            var dB = 20.0f * Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f));
            mixer.SetFloat(parameterName, dB);
        }
    }
}