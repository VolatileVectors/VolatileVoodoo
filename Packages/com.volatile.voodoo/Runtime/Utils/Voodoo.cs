using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Events.Base;
using VolatileVoodoo.Values.Base;

namespace VolatileVoodoo.Utils
{
    public static class Voodoo
    {
        // Constants
        public const double DspWarmStartDelay = 1d / 6d;
        public const byte AudioSourcePoolSize = 30;

        // OdinValidators
        public const string IsPlaying = "@UnityEngine.Application.isPlaying";
        public const string IsNotPlaying = "@!UnityEngine.Application.isPlaying";

#if UNITY_EDITOR
        // Editor Preferences keys
        private const string SettingsPrefix = "VolatileVoodoo";
        private const string ShowGenericReferenceValueKey = "ShowGenericReferenceValue";
        private const string ShowGenericEventDebuggerKey = "ShowGenericEventDebugger";
        private const string DebuggerFilterKey = "LastDebuggerFilter";
        private const string DebuggerFilterTypeKey = "LastDebuggerFilterType";
        private const string SettingsSavePath = "SettingsSavePath";

        // Custom script icons
        public static readonly Dictionary<Type, string> TypeToIconMap = new() {
            [typeof(BaseEvent)] = "BaseEvent",
            [typeof(BaseEventListener)] = "BaseEventListener",
            [typeof(GenericValue)] = "GenericValue"
        };

        // Path helper
        public static string ApplicationProjectPath => Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;

        public static string DefaultSavePath => Path.Combine(Application.dataPath, "VolatileVoodoo").Replace("\\", "/");

        public static string ProjectPathToFullPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(ApplicationProjectPath, relativePath.Trim('\\', '/', ' ')));
        }

        public static string PersistentDataPathToFullPath(string relativePath)
        {
            return Path.GetFullPath(Path.Combine(Application.persistentDataPath, relativePath.Trim('\\', '/', ' ')));
        }

        public static string AssetPathToProjectPath(string relativePath)
        {
            return Path.GetRelativePath(ApplicationProjectPath, Path.Combine("Assets", relativePath.Trim('\\', '/', ' ')));
        }

        public static string AssetPathToNamespace(string relativePath)
        {
            return Path.GetRelativePath(Application.dataPath, relativePath.Trim('\\', '/', ' ')).Replace("\\", "/").Replace("/", ".");
        }

        public static string VoodooPackagePath(string packageName, string relativePath)
        {
            return Path.Combine($"Packages/{packageName}", relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
        }

        public static string VoodooAssetPath(string relativePath)
        {
            var path = Path.Combine(Path.GetFullPath(EditorPrefs.GetString(GetSettingsSavePath, DefaultSavePath)), relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        public static string GetShowGenericReferenceValueKey => string.Join('.', SettingsPrefix, Application.productName, ShowGenericReferenceValueKey);
        public static string GetShowGenericEventDebuggerKey => string.Join('.', SettingsPrefix, Application.productName, ShowGenericEventDebuggerKey);
        public static string GetSettingsSavePath => string.Join('.', SettingsPrefix, Application.productName, SettingsSavePath);
        public static string GetDebuggerFilterKey => string.Join('.', SettingsPrefix, Application.productName, DebuggerFilterKey);
        public static string GetDebuggerFilterTypeKey => string.Join('.', SettingsPrefix, Application.productName, DebuggerFilterTypeKey);
#endif
    }
}