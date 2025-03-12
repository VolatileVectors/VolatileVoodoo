using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Capybutler.Editor.Build.VdfTemplates;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Capybutler.Editor.Build
{
    public class BuildButler : EditorWindow
    {
        public enum BuildType
        {
            Development,
            Beta,
            ReleaseCandidate
        }

        private struct ScriptingDefineSymbol
        {
            public string Define;
            public bool Debug;
        }

        private TextField appIdField;

        private EnumField branchField;
        private TextField depotIdField;
        private TextField descriptionField;

        private Button startBuildButton;
        private TextField steamCmdPathField;

        private TextField steamLoginField;
        private TextField steamPasswordField;
        private Toggle uploadToggle;

        // Create string list of defines
        private List<ScriptingDefineSymbol> scriptingDefines;
        private ListView scriptingDefinesListView;

        [MenuItem("Capybutler/Build Butler", false, priority = 50)]
        private static void ShowWindow()
        {
            var window = GetWindow<BuildButler>(false, "Build Butler");
            window.minSize = new Vector2(520f, 400f);
            window.maxSize = new Vector2(700f, 610f);
            window.Show();
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(PathUtils.PackagePath("com.capybutler", "Editor/Build/BuildButler.uss")));
            root.Add(AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(PathUtils.PackagePath("com.capybutler", "Editor/Build/BuildButler.uxml")).Instantiate());

            // Build Configuration
            branchField = root.Q<EnumField>("buildBranch");
            branchField.RegisterValueChangedCallback(changeEvent => EditorPrefs.SetString(PathUtils.GetEditorKey("branch"), changeEvent.newValue.ToString()));
            descriptionField = root.Q<TextField>("buildDescription");
            descriptionField.RegisterValueChangedCallback(OnTextFieldChanged);
            appIdField = root.Q<TextField>("buildAppId");
            appIdField.RegisterValueChangedCallback(OnTextFieldChanged);
            depotIdField = root.Q<TextField>("buildDepotId");
            depotIdField.RegisterValueChangedCallback(OnTextFieldChanged);
            uploadToggle = root.Q<Toggle>("buildUpload");
            uploadToggle.RegisterValueChangedCallback(changeEvent => EditorPrefs.SetBool(PathUtils.GetEditorKey("upload"), changeEvent.newValue));

            if (Enum.TryParse<BuildType>(EditorPrefs.GetString(PathUtils.GetEditorKey("branch"), BuildType.Development.ToString()), out var parsedValue))
                branchField.SetValueWithoutNotify(parsedValue);

            descriptionField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(descriptionField.name), ""));
            appIdField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(appIdField.name), ""));
            depotIdField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(depotIdField.name), ""));
            uploadToggle.SetValueWithoutNotify(EditorPrefs.GetBool(PathUtils.GetEditorKey("upload"), false));

            // Scripting Defines
            scriptingDefines = LoadDefineSymbols();
            scriptingDefinesListView = root.Q<ListView>("scriptingDefines");
            scriptingDefinesListView.itemsSource = scriptingDefines;
            scriptingDefinesListView.itemsAdded += _ => SaveDefineSymbols();
            scriptingDefinesListView.itemsRemoved += _ => SaveDefineSymbols();
            scriptingDefinesListView.itemIndexChanged += (_, _) => SaveDefineSymbols();
            scriptingDefinesListView.RegisterCallback<ChangeEvent<string>>(_ => SaveDefineSymbols());
            scriptingDefinesListView.RegisterCallback<ChangeEvent<bool>>(_ => SaveDefineSymbols());
            scriptingDefinesListView.RefreshItems();

            // Steam Configuration
            steamLoginField = root.Q<TextField>("steamLogin");
            steamLoginField.RegisterValueChangedCallback(OnTextFieldChanged);
            steamPasswordField = root.Q<TextField>("steamPassword");
            steamPasswordField.RegisterValueChangedCallback(OnTextFieldChanged);
            steamCmdPathField = root.Q<TextField>("steamCmdPath");
            steamCmdPathField.RegisterValueChangedCallback(OnTextFieldChanged);
            root.Q<Button>("steamCmdPathBrowse").clicked += () => { OnFileSelect(steamCmdPathField, "exe"); };

            steamLoginField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(steamLoginField.name), ""));
            steamPasswordField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(steamPasswordField.name), ""));
            steamCmdPathField.SetValueWithoutNotify(EditorPrefs.GetString(PathUtils.GetEditorKey(steamCmdPathField.name), ""));
            root.Q<Button>("startBuild").clicked += OnStartBuildClicked;
        }

        private async void SaveDefineSymbols()
        {
            try {
                await Task.Delay(250);
                var names = "";
                var debugs = "";

                for (int index = 0; index < scriptingDefines.Count; index++) {
                    names += (index > 0 ? "|" : "") + (scriptingDefines[index].Define ?? "");
                    debugs += (index > 0 ? "|" : "") + scriptingDefines[index].Debug;
                }

                EditorPrefs.SetString(PathUtils.GetEditorKey($"{nameof(scriptingDefines)}__symbols"), names);
                EditorPrefs.SetString(PathUtils.GetEditorKey($"{nameof(scriptingDefines)}__debug"), debugs);
            }
            catch (Exception) {
                LogButler.Warning("Saving define symbols failed");
            }
        }

        private List<ScriptingDefineSymbol> LoadDefineSymbols()
        {
            var result = new List<ScriptingDefineSymbol>();

            var names = EditorPrefs.GetString(PathUtils.GetEditorKey($"{nameof(scriptingDefines)}__symbols"), "");
            var debugs = EditorPrefs.GetString(PathUtils.GetEditorKey($"{nameof(scriptingDefines)}__debug"), "");

            var defineName = names.Split('|');
            var defineDebug = debugs.Split('|');

            var length = Math.Min(defineName.Length, defineDebug.Length);

            for (var index = 0; index < length; index++)
                result.Add(new ScriptingDefineSymbol
                {
                    Define = defineName[index],
                    Debug = defineDebug[index] switch
                    {
                        "True" => true,
                        "False" => false,
                        _ => false
                    }
                });

            return result;
        }

        private static void OnTextFieldChanged(ChangeEvent<string> changeEvent)
        {
            if (changeEvent.target is not TextField textField)
                return;

            EditorPrefs.SetString(PathUtils.GetEditorKey(textField.name), textField.value);
        }

        private static void OnFileSelect(TextField textField, string extension)
        {
            var currentPath = Path.GetFullPath(string.IsNullOrWhiteSpace(textField.value) ? Application.dataPath : textField.value);
            var newPath = EditorUtility.OpenFilePanel("Select " + textField.label, Path.GetDirectoryName(currentPath), extension);
            textField.value = string.IsNullOrWhiteSpace(newPath) ? textField.value : Path.GetFullPath(newPath);
        }

        private void OnStartBuildClicked()
        {
            var buildType = Enum.TryParse<BuildType>(branchField.text, out var parsedBranch) ? parsedBranch : BuildType.Development;

            var buildPath = PathUtils.ProjectPathToFullPath("Build");
            if (!Directory.Exists(buildPath))
                Directory.CreateDirectory(buildPath);

            CompileSteamDeploymentScriptsForBranch(buildType);
            SetStackTraceLogTypesForBranch(buildType);
            PerformBuild(buildType);
        }

        private void CompileSteamDeploymentScriptsForBranch(BuildType buildType)
        {
            var outputPath = PathUtils.ProjectPathToFullPath("Build/Scripts");
            var app = new AppbuildTemplate(outputPath)
            {
                AppId = int.TryParse(appIdField.value, out var appIdValue) ? appIdValue : 0,
                DepotId = int.TryParse(depotIdField.value, out var depotIdValue) ? depotIdValue : 0,
                BuildType = buildType,
                Description = descriptionField.value
            };

            var depot = new DepotbuildTemplate(outputPath)
            {
                DepotId = app.DepotId,
            };

            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            app.Create();
            depot.Create();

            EditorPrefs.SetString(PathUtils.GetEditorKey("appIdScriptPath"), app.FullFileName);
        }

        private void SetStackTraceLogTypesForBranch(BuildType buildType)
        {
            switch (buildType) {
                case BuildType.Development:
                    PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
                    return;
                case BuildType.Beta:
                case BuildType.ReleaseCandidate:
                default:
                    PlayerSettings.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
                    PlayerSettings.SetStackTraceLogType(LogType.Warning, StackTraceLogType.None);
                    PlayerSettings.SetStackTraceLogType(LogType.Error, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Assert, StackTraceLogType.ScriptOnly);
                    PlayerSettings.SetStackTraceLogType(LogType.Exception, StackTraceLogType.ScriptOnly);
                    return;
            }
        }

        private string GetScriptingDefineSymbolsForBranch(BuildType buildType)
        {
            var filteredDefines = scriptingDefines.Where(entry => buildType == BuildType.Development || !entry.Debug).Select(entry => entry.Define).Where(define => !string.IsNullOrEmpty(define));
            return string.Join(';', filteredDefines) + ';';
        }

        private string[] GetScriptingDefineSymbolsArrayForBranch(BuildType buildType)
            => scriptingDefines.Where(entry => buildType == BuildType.Development || !entry.Debug).Select(entry => entry.Define).ToArray();

        private void PerformBuild(BuildType buildType)
        {
            var targetGroup = BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneLinux64);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);
            PlayerSettings.SetArchitecture(namedBuildTarget, 1);
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, GetScriptingDefineSymbolsForBranch(buildType));
            PlayerSettings.SetIl2CppStacktraceInformation(namedBuildTarget, buildType switch
            {
                BuildType.Development => Il2CppStacktraceInformation.MethodFileLineNumber,
                _ => Il2CppStacktraceInformation.MethodOnly
            });
            PlayerSettings.SetIl2CppCompilerConfiguration(namedBuildTarget, buildType switch
            {
                BuildType.Development => Il2CppCompilerConfiguration.Debug,
                BuildType.Beta => Il2CppCompilerConfiguration.Release,
                BuildType.ReleaseCandidate => Il2CppCompilerConfiguration.Master,
                _ => Il2CppCompilerConfiguration.Release
            });

            EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, BuildTarget.StandaloneLinux64);

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();

            var buildOptions = BuildOptions.CompressWithLz4;
            if (buildType == BuildType.Development)
                buildOptions |=
                    BuildOptions.Development |
                    BuildOptions.AllowDebugging |
                    BuildOptions.ConnectWithProfiler |
                    BuildOptions.EnableDeepProfilingSupport;

            var buildPlayerOptions = new BuildPlayerOptions
            {
                locationPathName = PathUtils.ProjectPathToFullPath($"Build/{Application.productName}/{Application.productName}.x86_64"),
                options = buildOptions,
                target = BuildTarget.StandaloneLinux64,
                targetGroup = targetGroup,
                extraScriptingDefines = GetScriptingDefineSymbolsArrayForBranch(buildType),
                scenes = EditorBuildSettings.scenes.Where(scene => scene.enabled).Select(scene => scene.path).ToArray()
            };

            var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            if (report.summary.result != BuildResult.Succeeded)
                Debug.LogError("Error building player: " + report.summary.totalErrors);
        }
    }
}