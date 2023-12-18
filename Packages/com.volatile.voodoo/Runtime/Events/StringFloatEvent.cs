using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "StringFloatEvent", menuName = "Voodoo/Events/StringFloatEvent")]
    public class StringFloatEvent : GenericEvent<string, float> { }

    [Serializable]
    public class StringFloatEventSource : GenericEventSource<StringFloatEvent, string, float> { }
}