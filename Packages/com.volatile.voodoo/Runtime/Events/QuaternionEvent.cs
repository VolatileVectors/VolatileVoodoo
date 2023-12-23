using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "QuaternionEvent", menuName = "Voodoo/Events/QuaternionEvent")]
    public class QuaternionEvent : GenericEvent<Quaternion> { }

    [Serializable]
    public class QuaternionEventSource : GenericEventSource<QuaternionEvent, Quaternion> { }
}