using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "IntEvent", menuName = "Voodoo/Events/IntEvent")]
    public class IntEvent : GenericEvent<int> { }

    [Serializable]
    public class IntEventSource : GenericEventSource<IntEvent, int> { }
}