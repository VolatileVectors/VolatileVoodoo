using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugEventsTab : VoodooDebugTab<BaseEvent>
    {
        private static readonly List<BaseEvent.SubscriberInfo> VoodooSubscribers = new();
        private static ListView voodooSubscribersList = new();
        private BaseEvent selected;

        private static readonly List<BaseEvent.LogEntry> FilteredVoodooEventLogEntries = new();
        private ListView voodooEventLogList;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            VoodooElementsList = rootVisualElement.Q<ListView>("eventsList");
            
            voodooSubscribersList = rootVisualElement.Q<ListView>("subscribersList");
            voodooSubscribersList.itemsSource = VoodooSubscribers;
            voodooSubscribersList.bindItem = OnBindSubscriber;
            
            voodooEventLogList = rootVisualElement.Q<ListView>("eventLogList");
            voodooEventLogList.itemsSource = FilteredVoodooEventLogEntries;
            voodooEventLogList.bindItem = OnBindLogEntry;
            
            base.CreateGUI(rootVisualElement);
            
            VoodooElementsList.selectionChanged += OnEventSelected;
            voodooEventLogList.selectionChanged += OnLogEntrySelected;
            
            BaseEvent.DebugSubscribersChanged = OnSubscribersChanged;
            BaseEvent.EventLogUpdated = OnElementsChanged;

            VoodooElementsList.Rebuild();

            FilteredVoodooEventLogEntries.Clear();
            FilteredVoodooEventLogEntries.AddRange(BaseEvent.EventLog.Where(entry => FilteredVoodooElements.Contains(entry.RaisedEvent)));

            voodooEventLogList.Rebuild();
        }

        private void OnEventSelected(IEnumerable<object> items)
        {
            // TODO does not fire in case all elements are deselected/ListView lost focus
            VoodooSubscribers.Clear();
            selected = (BaseEvent)items.FirstOrDefault();
            if (selected != null) {
                VoodooSubscribers.AddRange(selected.DebugSubscribers());
            }

            voodooSubscribersList.Rebuild();
        }

        private void OnLogEntrySelected(IEnumerable<object> items)
        {
            // TODO does not fire in case all elements are deselected/ListView lost focus
            var firstEntry = items.FirstOrDefault();
            if (firstEntry == null)
                return;

            var selectedEntry = (BaseEvent.LogEntry)firstEntry;
            var eventIndex = FilteredVoodooElements.IndexOf(selectedEntry.RaisedEvent);
            if (eventIndex >= 0)
                VoodooElementsList.SetSelection(eventIndex);
        }

        private void OnSubscribersChanged(BaseEvent item)
        {
            if (item != selected)
                return;

            VoodooSubscribers.Clear();
            VoodooSubscribers.AddRange(selected.DebugSubscribers());
            voodooSubscribersList.Rebuild();
        }

        protected override void OnElementsChanged()
        {
            FilteredVoodooEventLogEntries.Clear();
            FilteredVoodooEventLogEntries.AddRange(BaseEvent.EventLog.Where(entry => FilteredVoodooElements.Contains(entry.RaisedEvent)));
            voodooEventLogList.Rebuild();
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

        private void OnBindLogEntry(VisualElement element, int index)
        {
            element.Q<Label>("timestamp").text = FilteredVoodooEventLogEntries[index].Timestamp.ToShortTimeString();
            element.Q<Label>("eventName").text = FilteredVoodooEventLogEntries[index].RaisedEvent.name;
            element.Q<Label>("eventParameters").text = FilteredVoodooEventLogEntries[index].Parameters;
        }

        protected override bool FilterCheck(BaseEvent item, string filterText, FilterType filterType) =>
            ((filterType & FilterType.Name) == FilterType.Name && item.name.Contains(filterText)) |
            ((filterType & FilterType.Type) == FilterType.Type && item.GetType().Name.Contains(filterText)) |
            ((filterType & FilterType.Description) == FilterType.Description && item.description.Contains(filterText));
    }
}