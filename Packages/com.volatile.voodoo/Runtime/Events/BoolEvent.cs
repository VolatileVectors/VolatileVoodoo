using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Voodoo/Events/BoolEvent")]
    public class BoolEvent : GenericEvent<bool> { }

    [Serializable]
    public class BoolEventSource : GenericEventSource<BoolEvent, bool> { }
}