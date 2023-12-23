using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "StringFloatEvent", menuName = "Voodoo/Events/StringFloatEvent")]
    public class StringFloatEvent : GenericEvent<string, float> { }

    [Serializable]
    public class StringFloatEventSource : GenericEventSource<StringFloatEvent, string, float> { }
}