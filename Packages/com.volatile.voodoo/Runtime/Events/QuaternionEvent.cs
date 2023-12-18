using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "QuaternionEvent", menuName = "Voodoo/Events/QuaternionEvent")]
    public class QuaternionEvent : GenericEvent<Quaternion> { }

    [Serializable]
    public class QuaternionEventSource : GenericEventSource<QuaternionEvent, Quaternion> { }
}