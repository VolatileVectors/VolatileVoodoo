using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "Vector2Event", menuName = "Voodoo/Events/Vector2Event")]
    public class Vector2Event : GenericEvent<Vector2> { }

    [Serializable]
    public class Vector2EventSource : GenericEventSource<Vector2Event, Vector2> { }
}