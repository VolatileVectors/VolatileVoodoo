using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Values.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugValuesTab : VoodooDebugTab<GenericValue>
    {
        public override void CreateGUI(VisualElement rootVisualElement)
        {
            VoodooElementsList = rootVisualElement.Q<ListView>("valuesList");
            base.CreateGUI(rootVisualElement);
        }

        protected override void OnBindValue(VisualElement element, int index)
        {
            void OnSelectClicked()
            {
                Selection.activeObject = FilteredVoodooElements[index];
                EditorGUIUtility.PingObject(FilteredVoodooElements[index]);
            }

            element.Q<Button>("selectValue").clicked += OnSelectClicked;
            element.Q<Label>("valueName").text = FilteredVoodooElements[index].name;
            element.Q<Label>("valueType").text = $"({FilteredVoodooElements[index].GetType().Name})";
            element.Q<TextField>("valueDescription").value = FilteredVoodooElements[index].description;
            element.Q<TextField>("initialValue").value = FilteredVoodooElements[index].InitialValueDebug;

            var currentValueField = element.Q<TextField>("currentValue");
            currentValueField.value = FilteredVoodooElements[index].CurrentValueDebug;
            FilteredVoodooElements[index].debugValueChanged = newValue => currentValueField.value = newValue;
        }

        protected override void OnUnbindValue(VisualElement element, int index)
        {
            VoodooElements.FindAll(item => item.name == element.Q<Label>("valueName").text).ForEach(item => item.debugValueChanged = null);
        }

        protected override bool FilterCheck(GenericValue item, string filterText, FilterType filterType) =>
            ((filterType & FilterType.Name) == FilterType.Name && item.name.Contains(filterText)) |
            ((filterType & FilterType.Type) == FilterType.Type && item.GetType().Name.Contains(filterText)) |
            ((filterType & FilterType.Description) == FilterType.Description && item.description.Contains(filterText));
    }
}