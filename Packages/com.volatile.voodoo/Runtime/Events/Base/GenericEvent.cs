using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace VolatileVoodoo.Events.Base
{
    public abstract class BaseEvent : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        [LabelWidth(100)]
        public string description = "";

        [Button("Raise")]
        [HideInEditorMode]
        protected abstract void RaiseButton();

        public struct SubscriberInfo
        {
            public GameObject GameObject;
            public string Component;
            public string Method;
        }

        public abstract IEnumerable<SubscriberInfo> DebugSubscribers();

        public static Action<BaseEvent> DebugSubscribersChanged;
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

#if UNITY_EDITOR
            DebugSubscribersChanged?.Invoke(this);
#endif
        }

        public void UnregisterListener(TResponse listener)
        {
            if (!EventListeners.Contains(listener))
                return;

            EventListeners.Remove(listener);

#if UNITY_EDITOR
            DebugSubscribersChanged?.Invoke(this);
#endif
        }

#if UNITY_EDITOR
        public override IEnumerable<SubscriberInfo> DebugSubscribers()
        {
            List<SubscriberInfo> subscribers = new();
            for (var listenerIndex = EventListeners.Count - 1; listenerIndex >= 0; listenerIndex--) {
                if (EventListeners[listenerIndex] == null)
                    continue;

                foreach (var unityAction in EventListeners[listenerIndex].GetInvocationList()) {
                    if (unityAction.Target is not UnityEventBase target)
                        continue;

                    for (var eventIndex = target.GetPersistentEventCount() - 1; eventIndex >= 0; eventIndex--) {
                        if (target.GetPersistentTarget(eventIndex) is not Component component)
                            continue;

                        subscribers.Add(new SubscriberInfo() {
                            GameObject = component.gameObject,
                            Component = component.GetType().ToString(),
                            Method = target.GetPersistentMethodName(eventIndex)
                        });
                    }
                }
            }

            return subscribers;
        }
#endif
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
            for (var i = EventListeners.Count - 1; i >= 0; i--)
                EventListeners[i]?.Invoke();
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
            for (var i = EventListeners.Count - 1; i >= 0; i--)
                EventListeners[i]?.Invoke(payloadA);
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
            for (var i = EventListeners.Count - 1; i >= 0; i--)
                EventListeners[i]?.Invoke(payloadA, payloadB);
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
            for (var i = EventListeners.Count - 1; i >= 0; i--)
                EventListeners[i]?.Invoke(payloadA, payloadB, payloadC);
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
            for (var i = EventListeners.Count - 1; i >= 0; i--)
                EventListeners[i]?.Invoke(payloadA, payloadB, payloadC, payloadD);
        }

#if UNITY_EDITOR
        protected override void RaiseButton()
        {
            Raise(DebugPayloadA, DebugPayloadB, DebugPayloadC, DebugPayloadD);
        }
#endif
    }
}