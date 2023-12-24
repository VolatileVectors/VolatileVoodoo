using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Events.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugEventsTab : VoodooDebugTab<BaseEvent>
    {
        private Label currentSelection;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            VoodooElementsList = rootVisualElement.Q<ListView>("eventsList");
            currentSelection = rootVisualElement.Q<Label>("selection");

            base.CreateGUI(rootVisualElement);

            VoodooElementsList.selectionChanged += OnItemSelected;
        }

        private void OnItemSelected(IEnumerable<object> items)
        {
            var selectedEvent = (BaseEvent)items.First();
            currentSelection.text = selectedEvent.DebugEventListeners();
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

        protected override bool FilterCheck(BaseEvent item, string filterText, FilterType filterType) =>
            ((filterType & FilterType.Name) == FilterType.Name && item.name.Contains(filterText)) |
            ((filterType & FilterType.Type) == FilterType.Type && item.GetType().Name.Contains(filterText)) |
            ((filterType & FilterType.Description) == FilterType.Description && item.description.Contains(filterText));
    }
}