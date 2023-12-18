using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "Vector2IntEvent", menuName = "Voodoo/Events/Vector2IntEvent")]
    public class Vector2IntEvent : GenericEvent<Vector2Int> { }

    [Serializable]
    public class Vector2IntEventSource : GenericEventSource<Vector2IntEvent, Vector2Int> { }
}