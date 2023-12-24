using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine.UIElements;

namespace VolatileVoodoo.Editor.UI
{
    [Flags]
    public enum FilterType
    {
        Everything = Name | Description | Type,
        Name = 1 << 0,
        Description = 1 << 1,
        Type = 1 << 2
    }

    public abstract class VoodooDebugTab<T> where T : UnityEngine.ScriptableObject
    {
        protected static readonly List<T> VoodooElements = new();
        protected static readonly List<T> FilteredVoodooElements = new();
        protected ListView VoodooElementsList;

        private TextField listFilter = new();
        private EnumField listFilterType = new();

        public virtual void CreateGUI(VisualElement rootVisualElement)
        {
            listFilter = rootVisualElement.parent.Q<TextField>("listFilter");
            listFilter.RegisterValueChangedCallback(_ => OnFilterChanged());

            listFilterType = rootVisualElement.parent.Q<EnumField>("filterType");
            listFilterType.RegisterValueChangedCallback(_ => OnFilterChanged());

            FindAll();

            VoodooElementsList.itemsSource = FilteredVoodooElements;
            VoodooElementsList.bindItem = OnBindValue;
            VoodooElementsList.unbindItem = OnUnbindValue;

            OnFilterChanged();
        }

        private static void FindAll()
        {
            var filter = $"t:{typeof(T).Name}";
            var assetGuids = AssetDatabase.FindAssets(filter);
            var assets = assetGuids
                .Select(assetGuid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assetGuid)))
                .Where(asset => asset != null);
            VoodooElements.Clear();
            VoodooElements.AddRange(assets);
        }

        private void OnFilterChanged()
        {
            FilteredVoodooElements.Clear();
            FilteredVoodooElements.AddRange(listFilter.value.IsNullOrWhitespace()
                ? VoodooElements
                : VoodooElements.Where(item => FilterCheck(item, listFilter.value, (FilterType)listFilterType.value)));

            VoodooElementsList.Rebuild();
        }

        protected abstract bool FilterCheck(T item, string filterText, FilterType filterType);
        protected abstract void OnBindValue(VisualElement element, int index);
        protected abstract void OnUnbindValue(VisualElement element, int index);
    }
}