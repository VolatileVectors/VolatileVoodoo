using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "UlongEvent", menuName = "Voodoo/Events/UlongEvent")]
    public class UlongEvent : GenericEvent<ulong> { }

    [Serializable]
    public class UlongEventSource : GenericEventSource<UlongEvent, ulong> { }
}