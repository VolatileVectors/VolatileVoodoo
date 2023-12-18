using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "Vector3Event", menuName = "Voodoo/Events/Vector3Event")]
    public class Vector3Event : GenericEvent<Vector3> { }

    [Serializable]
    public class Vector3EventSource : GenericEventSource<Vector3Event, Vector3> { }
}