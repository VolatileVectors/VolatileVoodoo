using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodoo.Runtime.Events.Base
{
    public abstract class BaseEventListener : MonoBehaviour
    {
#if UNITY_EDITOR
        public static Action<bool> InlineEditorStateChanged;
#endif
    }

    public abstract class BaseEventListener<TEvent, TReceiver> : BaseEventListener
        where TEvent : BaseEvent
        where TReceiver : UnityEventBase
    {
        [Tooltip("Event to register with.")]
        [HorizontalGroup("Event")]
        [Required]
        [OnValueChanged(nameof(UpdateSenderViewer))]
        public TEvent source;

        [Tooltip("Response to invoke when Event is raised.")]
        public TReceiver listeners;

        private void UpdateSenderViewer()
        {
#if UNITY_EDITOR
            senderViewer = source;
#endif
        }

#if UNITY_EDITOR
        [HideInEditorMode]
        [HorizontalGroup("Event", MaxWidth = 25)]
        [ToggleButton("Show in inline editor", SdfIconType.EyeFill, SdfIconType.EyeSlashFill)]
        [HideLabel]
        [ShowInInspector]
        private bool Show {
            get => EditorPrefs.GetBool(Voodoo.GetShowGenericEventDebuggerKey, false);
            set {
                EditorPrefs.SetBool(Voodoo.GetShowGenericEventDebuggerKey, value);
                InlineEditorStateChanged?.Invoke(value);
            }
        }

        [HideInEditorMode]
        [ShowIf("@" + nameof(source) + " != null && " + nameof(Show) + " == true")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        [OnInspectorInit(nameof(UpdateSenderViewer))]
        [OnValueChanged(nameof(UpdateSenderViewer))]
        private BaseEvent senderViewer;
#endif
    }

    public abstract class GenericEventListener<TEvent>
        : BaseEventListener<TEvent, UnityEvent>
        where TEvent : GenericEvent
    {
        private void OnEnable()
        {
            source.RegisterListener(listeners.Invoke);
        }

        private void OnDisable()
        {
            source.UnregisterListener(listeners.Invoke);
        }
    }

    public abstract class GenericEventListener<TEvent, TPayloadA>
        : BaseEventListener<TEvent, UnityEvent<TPayloadA>>
        where TEvent : GenericEvent<TPayloadA>
    {
        private void OnEnable()
        {
            source.RegisterListener(listeners.Invoke);
        }

        private void OnDisable()
        {
            source.UnregisterListener(listeners.Invoke);
        }
    }

    public abstract class GenericEventListener<TEvent, TPayloadA, TPayloadB>
        : BaseEventListener<TEvent, UnityEvent<TPayloadA, TPayloadB>>
        where TEvent : GenericEvent<TPayloadA, TPayloadB>
    {
        private void OnEnable()
        {
            source.RegisterListener(listeners.Invoke);
        }

        private void OnDisable()
        {
            source.UnregisterListener(listeners.Invoke);
        }
    }

    public abstract class GenericEventListener<TEvent, TPayloadA, TPayloadB, TPayloadC>
        : BaseEventListener<TEvent, UnityEvent<TPayloadA, TPayloadB, TPayloadC>>
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC>
    {
        private void OnEnable()
        {
            source.RegisterListener(listeners.Invoke);
        }

        private void OnDisable()
        {
            source.UnregisterListener(listeners.Invoke);
        }
    }

    public abstract class GenericEventListener<TEvent, TPayloadA, TPayloadB, TPayloadC, TPayloadD>
        : BaseEventListener<TEvent, UnityEvent<TPayloadA, TPayloadB, TPayloadC, TPayloadD>>
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC, TPayloadD>
    {
        private void OnEnable()
        {
            source.RegisterListener(listeners.Invoke);
        }

        private void OnDisable()
        {
            source.UnregisterListener(listeners.Invoke);
        }
    }
}