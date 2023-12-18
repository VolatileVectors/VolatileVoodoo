using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "ByteEvent", menuName = "Voodoo/Events/ByteEvent")]
    public class ByteEvent : GenericEvent<byte> { }

    [Serializable]
    public class ByteEventSource : GenericEventSource<ByteEvent, byte> { }
}