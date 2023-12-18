using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "FloatEvent", menuName = "Voodoo/Events/FloatEvent")]
    public class FloatEvent : GenericEvent<float> { }

    [Serializable]
    public class FloatEventSource : GenericEventSource<FloatEvent, float> { }
}