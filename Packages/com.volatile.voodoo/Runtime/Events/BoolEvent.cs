using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "BoolEvent", menuName = "Voodoo/Events/BoolEvent")]
    public class BoolEvent : GenericEvent<bool> { }

    [Serializable]
    public class BoolEventSource : GenericEventSource<BoolEvent, bool> { }
}