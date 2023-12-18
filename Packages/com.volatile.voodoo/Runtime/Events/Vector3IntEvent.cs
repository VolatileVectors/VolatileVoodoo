using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "Vector3IntEvent", menuName = "Voodoo/Events/Vector3IntEvent")]
    public class Vector3IntEvent : GenericEvent<Vector3Int> { }

    [Serializable]
    public class Vector3IntEventSource : GenericEventSource<Vector3IntEvent, Vector3Int> { }
}