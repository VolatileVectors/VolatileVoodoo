using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "Vector2IntEvent", menuName = "Voodoo/Events/Vector2IntEvent")]
    public class Vector2IntEvent : GenericEvent<Vector2Int> { }

    [Serializable]
    public class Vector2IntEventSource : GenericEventSource<Vector2IntEvent, Vector2Int> { }
}