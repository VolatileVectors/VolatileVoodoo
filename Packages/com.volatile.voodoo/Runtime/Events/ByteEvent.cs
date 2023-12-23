using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "ByteEvent", menuName = "Voodoo/Events/ByteEvent")]
    public class ByteEvent : GenericEvent<byte> { }

    [Serializable]
    public class ByteEventSource : GenericEventSource<ByteEvent, byte> { }
}