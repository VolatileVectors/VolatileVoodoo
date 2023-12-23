using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "UintEvent", menuName = "Voodoo/Events/UintEvent")]
    public class UintEvent : GenericEvent<uint> { }

    [Serializable]
    public class UintEventSource : GenericEventSource<UintEvent, uint> { }
}