using System;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Runtime.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Voodoo/Events/GameEvent")]
    public class GameEvent : GenericEvent { }

    [Serializable]
    public class GameEventSource : GenericEventSource<GameEvent> { }
}