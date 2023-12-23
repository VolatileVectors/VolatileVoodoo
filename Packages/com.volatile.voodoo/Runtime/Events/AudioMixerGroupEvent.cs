using System;
using UnityEngine;
using UnityEngine.Audio;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "AudioMixerGroupEvent", menuName = "Voodoo/Events/AudioMixerGroupEvent")]
    public class AudioMixerGroupEvent : GenericEvent<AudioMixerGroup> { }

    [Serializable]
    public class AudioMixerGroupEventSource : GenericEventSource<AudioMixerGroupEvent, AudioMixerGroup> { }
}