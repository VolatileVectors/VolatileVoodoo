using System;
using UnityEngine;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Voodoo/Events/GameEvent")]
    public class GameEvent : GenericEvent { }

    [Serializable]
    public class GameEventSource : GenericEventSource<GameEvent> { }
}