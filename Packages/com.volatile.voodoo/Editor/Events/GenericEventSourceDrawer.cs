using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;

namespace VolatileVoodoo.Editor.Events
{
    public class GenericEventSourceDrawer<TReference, TEvent> : OdinValueDrawer<TReference>
        where TReference : GenericEventSource<TEvent>
        where TEvent : GenericEvent
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), label, ValueEntry.SmartValue.sender, typeof(TEvent), false) as TEvent;
            if (!EditorGUI.EndChangeCheck()) return;

            var child = Property.Children.Get(nameof(ValueEntry.SmartValue.sender));
            child.ValueEntry.WeakSmartValue = selected;
            child.ValueEntry.ApplyChanges();
        }
    }

    public class GenericEventSourceDrawerPayloadA<TReference, TEvent, TPayloadA>
        : OdinValueDrawer<TReference>
        where TReference : GenericEventSource<TEvent, TPayloadA>
        where TEvent : GenericEvent<TPayloadA>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), label, ValueEntry.SmartValue.sender, typeof(TEvent), false) as TEvent;
            if (!EditorGUI.EndChangeCheck()) return;

            var child = Property.Children.Get(nameof(ValueEntry.SmartValue.sender));
            child.ValueEntry.WeakSmartValue = selected;
            child.ValueEntry.ApplyChanges();
        }
    }

    public class GenericEventSourceDrawerPayloadAPayloadB<TReference, TEvent, TPayloadA, TPayloadB>
        : OdinValueDrawer<TReference>
        where TReference : GenericEventSource<TEvent, TPayloadA, TPayloadB>
        where TEvent : GenericEvent<TPayloadA, TPayloadB>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), label, ValueEntry.SmartValue.sender, typeof(TEvent), false) as TEvent;
            if (!EditorGUI.EndChangeCheck()) return;

            var child = Property.Children.Get(nameof(ValueEntry.SmartValue.sender));
            child.ValueEntry.WeakSmartValue = selected;
            child.ValueEntry.ApplyChanges();
        }
    }

    public class GenericEventSourceDrawerPayloadAPayloadBPayloadC<TReference, TEvent, TPayloadA, TPayloadB, TPayloadC>
        : OdinValueDrawer<TReference>
        where TReference : GenericEventSource<TEvent, TPayloadA, TPayloadB, TPayloadC>
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), label, ValueEntry.SmartValue.sender, typeof(TEvent), false) as TEvent;
            if (!EditorGUI.EndChangeCheck()) return;

            var child = Property.Children.Get(nameof(ValueEntry.SmartValue.sender));
            child.ValueEntry.WeakSmartValue = selected;
            child.ValueEntry.ApplyChanges();
        }
    }

    public class GenericEventSourceDrawerPayloadAPayloadBPayloadCPayloadD<TReference, TEvent, TPayloadA, TPayloadB, TPayloadC, TPayloadD>
        : OdinValueDrawer<TReference>
        where TReference : GenericEventSource<TEvent, TPayloadA, TPayloadB, TPayloadC, TPayloadD>
        where TEvent : GenericEvent<TPayloadA, TPayloadB, TPayloadC, TPayloadD>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            EditorGUI.BeginChangeCheck();
            var selected = EditorGUI.ObjectField(EditorGUILayout.GetControlRect(), label, ValueEntry.SmartValue.sender, typeof(TEvent), false) as TEvent;
            if (!EditorGUI.EndChangeCheck()) return;

            var child = Property.Children.Get(nameof(ValueEntry.SmartValue.sender));
            child.ValueEntry.WeakSmartValue = selected;
            child.ValueEntry.ApplyChanges();
        }
    }
}