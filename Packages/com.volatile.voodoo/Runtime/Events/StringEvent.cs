using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "Voodoo/Events/StringEvent")]
    public class StringEvent : GenericEvent<string> { }

    [Serializable]
    public class StringEventSource : GenericEventSource<StringEvent, string> { }
}