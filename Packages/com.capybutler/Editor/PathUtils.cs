using System.IO;
using UnityEngine;

namespace Capybutler.Editor
{
    public static class PathUtils
    {
        private const string EditorKeyPrefix = "Capybutler";
        public static string ApplicationProjectPath => Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
        public static string GetEditorKey(string name) => $"{EditorKeyPrefix}.{Application.productName}.{name}";
        public static string ProjectPathToFullPath(string relativePath) => Path.GetFullPath(Path.Combine(ApplicationProjectPath, relativePath.Trim('\\', '/', ' ')));
        public static string PackagePath(string packageName, string relativePath) => Path.Combine($"Packages/{packageName}", relativePath.Trim('\\', '/', ' ')).Replace("\\", "/");
    }
}