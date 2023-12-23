using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;
using VolatileVoodoo.Editor.VdfTemplates;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor
{
    public class BuildTool : EditorWindow
    {
        public enum BuildType
        {
            Development,
            Beta,
            ReleaseCandidate
        }

        private const string EditorKeyPrefix = "VolatileVoodoo.Build.";

        private static readonly string[] DefaultScriptingDefines = {
            "STEAMWORKS_NET",
            "ODIN_INSPECTOR",
            "ODIN_INSPECTOR_3",
            "ODIN_INSPECTOR_3_1",
            "ODIN_VALIDATOR",
            "ODIN_VALIDATOR_3_1",
            "ODIN_INSPECTOR_EDITOR_ONLY",
            "SHAPES_URP"
        };

        private static readonly string[] DevelopmentScriptingDefines = {
            "ENABLE_LOGGING",
            "EXTENDED_DEBUG"
        };

        private TextField appIdField;

        private EnumField branchField;
        private TextField buildPathField;
        private TextField depotIdField;
        private TextField descriptionField;

        private Button startBuildButton;
        private TextField steamCmdPathField;

        private TextField steamLoginField;
        private TextField steamPasswordField;
        private Toggle uploadToggle;

        public static string GetEditorKeyPrefix => EditorKeyPrefix + Application.productName + ".";

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Voodoo.VoodooPackagePath("com.volatile.voodoo.build", "Editor/BuildTool.uss")));
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Voodoo.VoodooPackagePath("com.volatile.voodoo.build", "Editor/BuildTool.uxml"));
            root.Add(visualTree.Instantiate());

            // Build Configuration
            branchField = root.Q<EnumField>("buildBranch");
            branchField.RegisterValueChangedCallback(changeEvent => EditorPrefs.SetString(GetEditorKeyPrefix + "branch", changeEvent.newValue.ToString()));
            descriptionField = root.Q<TextField>("buildDescription");
            descriptionField.RegisterValueChangedCallback(OnTextFieldChanged);
            appIdField = root.Q<TextField>("buildAppId");
            appIdField.RegisterValueChangedCallback(OnTextFieldChanged);
            depotIdField = root.Q<TextField>("buildDepotId");
            depotIdField.RegisterValueChangedCallback(OnTextFieldChanged);
            buildPathField = root.Q<TextField>("buildPath");
            buildPathField.RegisterValueChangedCallback(OnTextFieldChanged);
            uploadToggle = root.Q<Toggle>("buildUpload");
            uploadToggle.RegisterValueChangedCallback(changeEvent => EditorPrefs.SetBool(GetEditorKeyPrefix + "upload", changeEvent.newValue));
            root.Q<Button>("buildPathBrowse").clicked += () => { OnPathSelect(buildPathField); };

            // Init Build Configuration
            if (Enum.TryParse<BuildType>(EditorPrefs.GetString(GetEditorKeyPrefix + "branch", BuildType.Development.ToString()), out var parsedValue))
                branchField.SetValueWithoutNotify(parsedValue);

            descriptionField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + descriptionField.name, ""));
            appIdField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + appIdField.name, ""));
            depotIdField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + depotIdField.name, ""));
            buildPathField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + buildPathField.name, ""));
            uploadToggle.SetValueWithoutNotify(EditorPrefs.GetBool(GetEditorKeyPrefix + "upload", false));

            // Steam Configuration
            steamLoginField = root.Q<TextField>("steamLogin");
            steamLoginField.RegisterValueChangedCallback(OnTextFieldChanged);
            steamPasswordField = root.Q<TextField>("steamPassword");
            steamPasswordField.RegisterValueChangedCallback(OnTextFieldChanged);
            steamCmdPathField = root.Q<TextField>("steamCmdPath");
            steamCmdPathField.RegisterValueChangedCallback(OnTextFieldChanged);
            root.Q<Button>("steamCmdPathBrowse").clicked += () => { OnFileSelect(steamCmdPathField, "exe"); };

            // Init Steam Configuration
            steamLoginField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + steamLoginField.name, ""));
            steamPasswordField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + steamPasswordField.name, ""));
            steamCmdPathField.SetValueWithoutNotify(EditorPrefs.GetString(GetEditorKeyPrefix + steamCmdPathField.name, ""));

            root.Q<Button>("startBuild").clicked += OnStartBuildClicked;
        }

        [MenuItem("Volatile Voodoo/Steam Build", false, priority = 50)]
        private static void ShowWindow()
        {
            var window = GetWindow<BuildTool>(false, "Build Tool");
            window.minSize = new Vector2(520f, 300f);
            window.maxSize = new Vector2(700f, 300f);
            window.Show();
        }

        private static void OnTextFieldChanged(ChangeEvent<string> changeEvent)
        {
            if (changeEvent.target is not TextField textField)
                return;

            EditorPrefs.SetString(GetEditorKeyPrefix + textField.name, textField.value);
        }

        private static void OnPathSelect(TextField textField)
        {
            var currentPath = Path.GetFullPath(string.IsNullOrWhiteSpace(textField.value) ? Application.dataPath : textField.value);
            var newPath = EditorUtility.OpenFolderPanel("Select " + textField.label, currentPath, "");
            textField.value = string.IsNullOrWhiteSpace(newPath) ? textField.value : Path.GetFullPath(newPath);
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

            CompileSteamDeploymentScriptsForBranch(buildType);
            SetStackTraceLogTypesForBranch(buildType);
            PerformBuild(buildType);
        }

        private void CompileSteamDeploymentScriptsForBranch(BuildType buildType)
        {
            var outputPath = Voodoo.ProjectPathToFullPath("Build/Scripts");

            var app = new AppbuildTemplate(outputPath) {
                AppId = int.TryParse(appIdField.value, out var appIdValue) ? appIdValue : 0,
                DepotId = int.TryParse(depotIdField.value, out var depotIdValue) ? depotIdValue : 0,
                BuildType = buildType,
                Description = descriptionField.value
            };

            var depot = new DepotbuildTemplate(outputPath) {
                DepotId = app.DepotId,
                BuildPath = buildPathField.value
            };

            app.Create();
            depot.Create();

            EditorPrefs.SetString(GetEditorKeyPrefix + "appIdScriptPath", app.FullFileName);
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
            var result = string.Join(';', DefaultScriptingDefines);
            if (buildType == BuildType.Development)
                result += ';' + string.Join(';', DevelopmentScriptingDefines);

            return result;
        }

        private string[] GetScriptingDefineSymbolsArrayForBranch(BuildType buildType)
        {
            var result = DefaultScriptingDefines;
            if (buildType == BuildType.Development) result = result.Concat(DevelopmentScriptingDefines).ToArray();
            return result;
        }

        private void PerformBuild(BuildType buildType)
        {
            var targetGroup = BuildPipeline.GetBuildTargetGroup(BuildTarget.StandaloneLinux64);
            PlayerSettings.SetArchitecture(NamedBuildTarget.FromBuildTargetGroup(targetGroup), 1);
            PlayerSettings.SetScriptingDefineSymbols(NamedBuildTarget.FromBuildTargetGroup(targetGroup), GetScriptingDefineSymbolsForBranch(buildType));

            EditorUserBuildSettings.SwitchActiveBuildTarget(targetGroup, BuildTarget.StandaloneLinux64);

            AddressableAssetSettings.CleanPlayerContent();
            AddressableAssetSettings.BuildPlayerContent();

            var buildOptions = BuildOptions.CompressWithLz4;
            if (buildType == BuildType.Development)
                buildOptions |= BuildOptions.Development |
                                BuildOptions.AllowDebugging |
                                BuildOptions.ConnectWithProfiler;

            var buildPlayerOptions = new BuildPlayerOptions {
                locationPathName = Path.Combine(buildPathField.value, "CapybaraCrashCourses.x86_64"),
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