using System;
using UnityEngine;

namespace VolatileVoodoo.Runtime.Events.Base
{
    [Serializable]
    public abstract class GenericEventSource : ISerializationCallbackReceiver
    {
        [HideInInspector]
        [SerializeField]
        protected bool isValid;

        public abstract void OnBeforeSerialize();

        public void OnAfterDeserialize() { }
    }

    [Serializable]
    public abstract class GenericEventSource<TEvent> : GenericEventSource
        where TEvent : GenericEvent
    {
        [SerializeField]
        public TEvent sender;

        public void Raise()
        {
            if (isValid) sender.Raise();
        }

        public override void OnBeforeSerialize() => isValid = sender != null;
    }

    [Serializable]
    public abstract class GenericEventSource<TEvent, TPayloadA> : GenericEventSource
        where TEvent : GenericEvent<TPayloadA>
    {
        public TEvent sender;

        public void Raise(TPayloadA payloadA)
        {
            if (isValid) sender.Raise(payloadA);
        }

        public override void OnBeforeSerialize() => isValid = sender != null;
    }

    [Serializable]
    public abstract class GenericEventSource<TEvent, TPayloadA, TPayloadB> : GenericEventSource
        where TEvent : GenericEvent<TPayloadA, TPayloadB>
    {
        public TEvent sender;

        public void Raise(TPayloadA payloadA, TPayloadB payloadB)
        {
            if (isValid) sender.Raise(payloadA, payloadB);
        }

        public override void OnBeforeSerialize() => isValid = sender != null;
    }

    [Serializable]
    public abstract class GenericEventSource<TEvent, TPayloadA, TPayloadB, TPayloadC> : GenericEventSource
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC>
    {
        public TEvent sender;

        public void Raise(TPayloadA payloadA, TPayloadB payloadB, TPayloadC payloadC)
        {
            if (isValid) sender.Raise(payloadA, payloadB, payloadC);
        }

        public override void OnBeforeSerialize() => isValid = sender != null;
    }


    [Serializable]
    public abstract class GenericEventSource<TEvent, TPayloadA, TPayloadB, TPayloadC, TPayloadD> : GenericEventSource
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC, TPayloadD>
    {
        public TEvent sender;

        public void Raise(TPayloadA payloadA, TPayloadB payloadB, TPayloadC payloadC, TPayloadD payloadD)
        {
            if (isValid) sender.Raise(payloadA, payloadB, payloadC, payloadD);
        }

        public override void OnBeforeSerialize() => isValid = sender != null;
    }
}