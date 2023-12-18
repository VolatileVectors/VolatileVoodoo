using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "DoubleEvent", menuName = "Voodoo/Events/DoubleEvent")]
    public class DoubleEvent : GenericEvent<double> { }

    [Serializable]
    public class DoubleEventSource : GenericEventSource<DoubleEvent, double> { }
}