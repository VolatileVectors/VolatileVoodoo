using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace VolatileVoodoo.Runtime.Events.Base
{
    public abstract class BaseEvent : ScriptableObject
    {
#if UNITY_EDITOR
        [Button("Raise")]
        [HideInEditorMode]
        protected abstract void RaiseButton();
#endif
    }

    public abstract class BaseEvent<TResponse>
        : BaseEvent where TResponse : MulticastDelegate
    {
        protected readonly List<TResponse> EventListeners = new();

        public void RegisterListener(TResponse listener)
        {
            if (EventListeners.Contains(listener))
                return;

            EventListeners.Add(listener);
        }

        public void UnregisterListener(TResponse listener)
        {
            if (!EventListeners.Contains(listener))
                return;

            EventListeners.Remove(listener);
        }
    }

    public abstract class BaseEvent<TResponse, TPayloadA>
        : BaseEvent<TResponse> where TResponse : MulticastDelegate
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        protected TPayloadA DebugPayloadA;
#endif
    }

    public abstract class BaseEvent<TResponse, TPayloadA, TPayloadB>
        : BaseEvent<TResponse, TPayloadA> where TResponse : MulticastDelegate
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        protected TPayloadB DebugPayloadB;
#endif
    }

    public abstract class BaseEvent<TResponse, TPayloadA, TPayloadB, TPayloadC>
        : BaseEvent<TResponse, TPayloadA, TPayloadB> where TResponse : MulticastDelegate
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        protected TPayloadC DebugPayloadC;
#endif
    }

    public abstract class BaseEvent<TResponse, TPayloadA, TPayloadB, TPayloadC, TPayloadD>
        : BaseEvent<TResponse, TPayloadA, TPayloadB, TPayloadC> where TResponse : MulticastDelegate
    {
#if UNITY_EDITOR
        [ShowInInspector]
        [HideInEditorMode]
        protected TPayloadD DebugPayloadD;
#endif
    }

    public abstract class GenericEvent
        : BaseEvent<UnityAction>
    {
        public void Raise()
        {
            for (var i = EventListeners.Count - 1; i >= 0; i--) EventListeners[i]?.Invoke();
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise();
        }
#endif
    }

    public abstract class GenericEvent<TPayloadA>
        : BaseEvent<UnityAction<TPayloadA>, TPayloadA>
    {
        public void Raise(TPayloadA payloadA)
        {
            for (var i = EventListeners.Count - 1; i >= 0; i--) EventListeners[i]?.Invoke(payloadA);
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise(DebugPayloadA);
        }
#endif
    }

    public abstract class GenericEvent<TPayloadA, TPayloadB>
        : BaseEvent<UnityAction<TPayloadA, TPayloadB>, TPayloadA, TPayloadB>
    {
        public void Raise(TPayloadA payloadA, TPayloadB payloadB)
        {
            for (var i = EventListeners.Count - 1; i >= 0; i--) EventListeners[i]?.Invoke(payloadA, payloadB);
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise(DebugPayloadA, DebugPayloadB);
        }
#endif
    }

    public abstract class GenericEvent<TPayloadA, TPayloadB, TPayloadC>
        : BaseEvent<UnityAction<TPayloadA, TPayloadB, TPayloadC>, TPayloadA, TPayloadB, TPayloadC>
    {
        public void Raise(TPayloadA payloadA, TPayloadB payloadB, TPayloadC payloadC)
        {
            for (var i = EventListeners.Count - 1; i >= 0; i--) EventListeners[i]?.Invoke(payloadA, payloadB, payloadC);
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise(DebugPayloadA, DebugPayloadB, DebugPayloadC);
        }
#endif
    }

    public abstract class GenericEvent<TPayloadA, TPayloadB, TPayloadC, TPayloadD>
        : BaseEvent<UnityAction<TPayloadA, TPayloadB, TPayloadC, TPayloadD>, TPayloadA, TPayloadB, TPayloadC, TPayloadD>
    {
        public void Raise(TPayloadA payloadA, TPayloadB payloadB, TPayloadC payloadC, TPayloadD payloadD)
        {
            for (var i = EventListeners.Count - 1; i >= 0; i--) EventListeners[i]?.Invoke(payloadA, payloadB, payloadC, payloadD);
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise(DebugPayloadA, DebugPayloadB, DebugPayloadC, DebugPayloadD);
        }
#endif
    }
}