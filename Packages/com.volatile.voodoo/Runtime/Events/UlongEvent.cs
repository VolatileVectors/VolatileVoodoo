using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "UlongEvent", menuName = "Voodoo/Events/UlongEvent")]
    public class UlongEvent : GenericEvent<ulong> { }

    [Serializable]
    public class UlongEventSource : GenericEventSource<UlongEvent, ulong> { }
}