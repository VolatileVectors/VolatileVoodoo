using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Voodoo/Events/IntEvent")]
    public class IntEvent : GenericEvent<int> { }

    [Serializable]
    public class IntEventSource : GenericEventSource<IntEvent, int> { }
}