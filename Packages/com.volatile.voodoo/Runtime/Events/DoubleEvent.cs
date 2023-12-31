using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "DoubleEvent", menuName = "Voodoo/Events/DoubleEvent")]
    public class DoubleEvent : GenericEvent<double> { }

    [Serializable]
    public class DoubleEventSource : GenericEventSource<DoubleEvent, double> { }
}