using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "TransformEvent", menuName = "Voodoo/Events/TransformEvent")]
    public class TransformEvent : GenericEvent<Transform> { }

    [Serializable]
    public class TransformEventSource : GenericEventSource<TransformEvent, Transform> { }
}