using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "StringEvent", menuName = "Voodoo/Events/StringEvent")]
    public class StringEvent : GenericEvent<string> { }

    [Serializable]
    public class StringEventSource : GenericEventSource<StringEvent, string> { }
}