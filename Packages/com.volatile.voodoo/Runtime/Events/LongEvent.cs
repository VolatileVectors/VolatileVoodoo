using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "LongEvent", menuName = "Voodoo/Events/LongEvent")]
    public class LongEvent : GenericEvent<long> { }

    [Serializable]
    public class LongEventSource : GenericEventSource<LongEvent, long> { }
}