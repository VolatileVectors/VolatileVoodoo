using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Voodoo/Events/FloatEvent")]
    public class FloatEvent : GenericEvent<float> { }

    [Serializable]
    public class FloatEventSource : GenericEventSource<FloatEvent, float> { }
}