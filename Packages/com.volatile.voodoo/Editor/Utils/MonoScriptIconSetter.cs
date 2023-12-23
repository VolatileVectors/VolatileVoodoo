using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor.Utils
{
    [InitializeOnLoad]
    public class MonoScriptIconSetter : AssetPostprocessor
    {
        private static readonly Dictionary<string, Texture2D> Icons = new();
        private static readonly string IconAssetsPath = Voodoo.VoodooPackagePath("com.volatile.voodoo", "Editor/Resources/Icons");
        private const string MonoScriptIconsSessionFlag = "MonoScriptIconsInitialized";

        static MonoScriptIconSetter()
        {
            Icons.Clear();
            var path = Path.GetFullPath(IconAssetsPath);
            var fileInfos = new DirectoryInfo(path).GetFiles("*.png");
            foreach (var fileInfo in fileInfos) {
                var icon = AssetDatabase.LoadAssetAtPath<Texture2D>($"{IconAssetsPath}/{fileInfo.Name}");
                Icons[Path.GetFileNameWithoutExtension(fileInfo.Name)] = icon;
            }

            if (SessionState.GetBool(MonoScriptIconsSessionFlag, false))
                return;

            RegenerateAllIcons();
            SessionState.SetBool(MonoScriptIconsSessionFlag, true);
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!SessionState.GetBool(MonoScriptIconsSessionFlag, false)) {
                RegenerateAllIcons();
                SessionState.SetBool(MonoScriptIconsSessionFlag, true);
                return;
            }

            var changedAssets = importedAssets.Concat(movedAssets);
            foreach (var importedAssetPath in changedAssets) {
                if (AssetImporter.GetAtPath(importedAssetPath) is not MonoImporter monoImporter ||
                    !HasIcon(monoImporter.GetScript(), out var icon) ||
                    monoImporter.GetIcon() == icon)
                    continue;

                monoImporter.SetIcon(icon);
                monoImporter.SaveAndReimport();
            }
        }

        private static void RegenerateAllIcons()
        {
            var assetGuids = AssetDatabase.FindAssets("t:MonoScript");
            foreach (var assetGuid in assetGuids) {
                if (AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(assetGuid)) is not MonoImporter monoImporter ||
                    !HasIcon(monoImporter.GetScript(), out var icon) ||
                    monoImporter.GetIcon() == icon)
                    continue;

                monoImporter.SetIcon(icon);
                monoImporter.SaveAndReimport();
            }
        }

        private static bool HasIcon(MonoScript importedAsset, out Texture2D icon)
        {
            icon = null;
            if (importedAsset == null)
                return false;

            var importedType = importedAsset.GetClass();
            if (importedType == null)
                return false;

            var match = Voodoo.TypeToIconMap.Keys.FirstOrDefault(entry => importedType.IsSubclassOf(entry) && Icons.ContainsKey(Voodoo.TypeToIconMap[entry]));
            if (match == null)
                return false;

            icon = Icons[Voodoo.TypeToIconMap[match]];
            return true;
        }
    }
}