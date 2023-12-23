using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "LongEvent", menuName = "Voodoo/Events/LongEvent")]
    public class LongEvent : GenericEvent<long> { }

    [Serializable]
    public class LongEventSource : GenericEventSource<LongEvent, long> { }
}