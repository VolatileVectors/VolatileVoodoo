using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Runtime.Events.Base;
using VolatileVoodoo.Runtime.Values.Base;

namespace VolatileVoodoo.Runtime.Utils
{
    public static class Voodoo
    {
        // Constants
        public const float SemitonesToPitchConversionUnit = 1.05946f;
        public const double DspWarmStartDelay = 1d / 6d;
        public const byte AudioSourcePoolSize = 30;

        // OdinValidators
        public const string IsPlaying = "@UnityEngine.Application.isPlaying";
        public const string IsNotPlaying = "@!UnityEngine.Application.isPlaying";

#if UNITY_EDITOR
        // Editor Preferences keys
        private const string SettingsPrefix = "VolatileVoodoo";
        private const string AutoPlayEffectsKey = "AutoPlayEffects";
        private const string ShowGenericReferenceValueKey = "ShowGenericReferenceValue";
        private const string ShowGenericEventDebuggerKey = "ShowGenericEventDebugger";
        private const string SettingsSavePath = "SettingsSavePath";

        // Custom script icons
        public static readonly Dictionary<Type, string> TypeToIconMap = new() {
            [typeof(BaseEvent)] = "BaseEvent",
            [typeof(BaseEventListener)] = "BaseEventListener",
            [typeof(GenericValue)] = "GenericValue"
        };

        // Path helper
        private static string ApplicationProjectPath => Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;

        public static string PackagePath => "Packages/com.volatile.voodoo";
        
        public static string DefaultSavePath => Path.Combine(Application.dataPath, "VolatileVoodoo").Replace("\\", "/");

        public static string ProjectPathToFullPath(string relativePath) => Path.GetFullPath(Path.Combine(ApplicationProjectPath, relativePath.Trim('\\', '/', ' ')));

        public static string PersistentDataPathToFullPath(string relativePath) => Path.GetFullPath(Path.Combine(Application.persistentDataPath, relativePath.Trim('\\', '/', ' ')));

        public static string AssetPathToProjectPath(string relativePath) => Path.GetRelativePath(ApplicationProjectPath, Path.Combine("Assets", relativePath.Trim('\\', '/', ' ')));
        
        public static string AssetPathToNamespace(string relativePath) => Path.GetRelativePath(Application.dataPath, relativePath.Trim('\\', '/', ' ')).Replace("\\", "/").Replace("/", ".");

        public static string VoodooPackagePath(string relativePath) => Path.Combine(PackagePath, relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");

        public static string VoodooAssetPath(string relativePath)
        {
            var path = Path.Combine(Path.GetFullPath(EditorPrefs.GetString(GetSettingsSavePath, DefaultSavePath)), relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            
            return path;
        }

        public static string GetAutoPlayEffectsKey => string.Join('.', SettingsPrefix, Application.productName, AutoPlayEffectsKey);
        public static string GetShowGenericReferenceValueKey => string.Join('.', SettingsPrefix, Application.productName, ShowGenericReferenceValueKey);
        public static string GetShowGenericEventDebuggerKey => string.Join('.', SettingsPrefix, Application.productName, ShowGenericEventDebuggerKey);
        public static string GetSettingsSavePath => string.Join('.', SettingsPrefix, Application.productName, SettingsSavePath);
#endif
    }
}