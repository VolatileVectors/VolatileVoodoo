using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "PositionRotationEvent", menuName = "Voodoo/Events/PositionRotationEvent")]
    public class PositionRotationEvent : GenericEvent<Vector3, Quaternion> { }

    [Serializable]
    public class PositionRotationEventSource : GenericEventSource<PositionRotationEvent, Vector3, Quaternion> { }
}