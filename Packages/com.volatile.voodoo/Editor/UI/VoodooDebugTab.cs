using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace VolatileVoodoo.Editor.UI
{
    public abstract class VoodooDebugTab
    {
        protected TextField ListFilter = new();

        public virtual void CreateGUI(VisualElement rootVisualElement)
        {
            ListFilter = rootVisualElement.Q<TextField>("listFilter");
            ListFilter.RegisterValueChangedCallback(OnFilterChanged);
        }

        protected abstract void OnFilterChanged(ChangeEvent<string> textChange);

        protected static void FindAll<T>(List<T> itemList) where T : UnityEngine.Object
        {
            var filter = $"t:{typeof(T).Name}";
            var assetGuids = AssetDatabase.FindAssets(filter);
            var assets = assetGuids
                .Select(assetGuid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(assetGuid)))
                .Where(asset => asset != null);
            itemList.Clear();
            itemList.AddRange(assets);
        }
    }
}