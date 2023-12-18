using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "UintEvent", menuName = "Voodoo/Events/UintEvent")]
    public class UintEvent : GenericEvent<uint> { }

    [Serializable]
    public class UintEventSource : GenericEventSource<UintEvent, uint> { }
}