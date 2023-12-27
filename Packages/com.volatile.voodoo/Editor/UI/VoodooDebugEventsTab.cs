using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugEventsTab : VoodooDebugTab<BaseEvent>
    {
        private static readonly List<BaseEvent.SubscriberInfo> VoodooSubscribers = new();
        private static ListView voodooSubscribersList = new();
        private BaseEvent selected;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            VoodooElementsList = rootVisualElement.Q<ListView>("eventsList");

            base.CreateGUI(rootVisualElement);

            VoodooElementsList.selectionChanged += OnItemSelected;

            voodooSubscribersList = rootVisualElement.Q<ListView>("subscribersList");
            voodooSubscribersList.itemsSource = VoodooSubscribers;
            voodooSubscribersList.bindItem = OnBindSubscriber;

            BaseEvent.DebugSubscribersChanged = OnSubscribersChanged;
        }

        private void OnItemSelected(IEnumerable<object> items)
        {
            // TODO does not fire in case all elements are deselected/ListView lost focus
            VoodooSubscribers.Clear();
            foreach (var item in items) {
                selected = (BaseEvent)item;
                VoodooSubscribers.AddRange(selected.DebugSubscribers());
                voodooSubscribersList.Rebuild();
                return;
            }

            selected = null;
            voodooSubscribersList.Rebuild();
        }

        private void OnSubscribersChanged(BaseEvent item)
        {
            if (item != selected)
                return;

            VoodooSubscribers.Clear();
            VoodooSubscribers.AddRange(selected.DebugSubscribers());
            voodooSubscribersList.Rebuild();
        }

        protected override void OnBindValue(VisualElement element, int index)
        {
            void OnSelectClicked()
            {
                Selection.activeObject = FilteredVoodooElements[index];
                EditorGUIUtility.PingObject(FilteredVoodooElements[index]);
            }

            element.Q<Button>("selectValue").clicked += OnSelectClicked;
            element.Q<Label>("eventName").text = FilteredVoodooElements[index].name;
            element.Q<Label>("eventType").text = $"({FilteredVoodooElements[index].GetType().Name})";
            element.Q<TextField>("eventDescription").value = FilteredVoodooElements[index].description;
        }

        protected override void OnUnbindValue(VisualElement element, int index) { }

        private void OnBindSubscriber(VisualElement element, int index)
        {
            void OnSelectClicked()
            {
                Selection.activeObject = VoodooSubscribers[index].GameObject;
                EditorGUIUtility.PingObject(VoodooSubscribers[index].GameObject);
            }

            element.Q<Button>("selectSubscriber").clicked += OnSelectClicked;
            element.Q<Label>("subscriberName").text = VoodooSubscribers[index].GameObject.name;
            element.Q<Label>("subscriberComponent").text = VoodooSubscribers[index].Component;
            element.Q<Label>("subscriberMethod").text = VoodooSubscribers[index].Method;
        }

        protected override bool FilterCheck(BaseEvent item, string filterText, FilterType filterType) =>
            ((filterType & FilterType.Name) == FilterType.Name && item.name.Contains(filterText)) |
            ((filterType & FilterType.Type) == FilterType.Type && item.GetType().Name.Contains(filterText)) |
            ((filterType & FilterType.Description) == FilterType.Description && item.description.Contains(filterText));
    }
}