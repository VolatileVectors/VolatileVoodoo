using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "TransformEvent", menuName = "Voodoo/Events/TransformEvent")]
    public class TransformEvent : GenericEvent<Transform> { }

    [Serializable]
    public class TransformEventSource : GenericEventSource<TransformEvent, Transform> { }
}