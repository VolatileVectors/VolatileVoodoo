using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "PositionRotationEvent", menuName = "Voodoo/Events/PositionRotationEvent")]
    public class PositionRotationEvent : GenericEvent<Vector3, Quaternion> { }

    [Serializable]
    public class PositionRotationEventSource : GenericEventSource<PositionRotationEvent, Vector3, Quaternion> { }
}