using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Values.Base;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebugValuesTab : VoodooDebugTab
    {
        private static readonly List<GenericValue> voodooValues = new();
        private static readonly List<GenericValue> filteredVoodooValues = new();
        private ListView voodooValuesList;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            base.CreateGUI(rootVisualElement);

            FindAll(voodooValues);

            filteredVoodooValues.Clear();
            filteredVoodooValues.AddRange(ListFilter.value.IsNullOrWhitespace()
                ? voodooValues
                : voodooValues.Where(item => item.name.Contains(ListFilter.value)));

            voodooValuesList = rootVisualElement.Q<ListView>("valuesList");
            voodooValuesList.itemsSource = filteredVoodooValues;
            voodooValuesList.bindItem = OnBindValue;
            voodooValuesList.unbindItem = OnUnbindValue;
        }

        private static void OnBindValue(VisualElement element, int index)
        {
            void OnSelectClicked()
            {
                Selection.activeObject = filteredVoodooValues[index];
                EditorGUIUtility.PingObject(filteredVoodooValues[index]);
            }

            element.Q<Button>("selectValue").clicked += OnSelectClicked;
            element.Q<Label>("valueName").text = filteredVoodooValues[index].name;
            element.Q<TextField>("initialValue").value = filteredVoodooValues[index].InitialValueDebug;

            var currentValueField = element.Q<TextField>("currentValue");
            currentValueField.value = filteredVoodooValues[index].CurrentValueDebug;
            filteredVoodooValues[index].debugValueChanged = newValue => currentValueField.value = newValue;
        }

        private static void OnUnbindValue(VisualElement element, int index)
        {
            voodooValues.FindAll(item => item.name == element.Q<Label>("valueName").text).ForEach(item => item.debugValueChanged = null);
        }

        protected override void OnFilterChanged(ChangeEvent<string> textChange)
        {
            filteredVoodooValues.Clear();
            filteredVoodooValues.AddRange(textChange.newValue.IsNullOrWhitespace()
                ? voodooValues
                : voodooValues.Where(item => item.name.Contains(textChange.newValue)));

            voodooValuesList.Rebuild();
        }
    }
}