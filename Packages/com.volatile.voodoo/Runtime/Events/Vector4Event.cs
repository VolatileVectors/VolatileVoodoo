using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "Vector4Event", menuName = "Voodoo/Events/Vector4Event")]
    public class Vector4Event : GenericEvent<Vector4> { }

    [Serializable]
    public class Vector4EventSource : GenericEventSource<Vector4Event, Vector4> { }
}