using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor.Utils
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        private const string SceneAssetPropertyString = "sceneAsset";
        private const string ScenePathPropertyString = "scenePath";

        private const float PadSize = 2f;
        private const float IconSize = 14f;
        private const float FooterHeight = 10f;
        private static readonly RectOffset BoxPadding = EditorStyles.helpBox.padding;
        private static readonly float LineHeight = EditorGUIUtility.singleLineHeight;
        private static readonly float PaddedLine = LineHeight + PadSize;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, GUIContent.none, property);
            var sceneAssetProperty = property.FindPropertyRelative(SceneAssetPropertyString);

            position.height -= FooterHeight;
            GUI.Box(EditorGUI.IndentedRect(position), GUIContent.none, EditorStyles.helpBox);
            position = BoxPadding.Remove(position);
            position.height = LineHeight;

            label.tooltip = "The actual Scene Asset reference.\nOn serialize this is also stored as the asset's path.";

            EditorGUI.BeginChangeCheck();
            var sceneControlID = GUIUtility.GetControlID(FocusType.Passive);
            var selectedObject = EditorGUI.ObjectField(position, label, sceneAssetProperty.objectReferenceValue, typeof(SceneAsset), false);
            var buildScene = BuildUtils.GetBuildScene(selectedObject);

            if (EditorGUI.EndChangeCheck()) {
                sceneAssetProperty.objectReferenceValue = selectedObject;
                if (buildScene.Scene == null)
                    property.FindPropertyRelative(ScenePathPropertyString).stringValue = string.Empty;
            }

            position.y += PaddedLine;

            if (!buildScene.AssetGuid.Empty())
                DrawSceneInfoGUI(position, buildScene, sceneControlID + 1);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var lines = 2;
            var sceneAssetProperty = property.FindPropertyRelative(SceneAssetPropertyString);
            if (sceneAssetProperty.objectReferenceValue == null)
                lines = 1;

            return BoxPadding.vertical + LineHeight * lines + PadSize * (lines - 1) + FooterHeight;
        }

        private static void DrawSceneInfoGUI(Rect position, BuildUtils.BuildScene buildScene, int sceneControlID)
        {
            var readOnly = BuildUtils.IsReadOnly();
            var readOnlyWarning = readOnly ? "\n\nWARNING: Build Settings is not checked out and so cannot be modified." : "";

            var labelContent = new GUIContent();
            SdfIconType iconType;
            Color iconColor;

            if (buildScene.BuildIndex == -1) {
                iconType = SdfIconType.Circle;
                iconColor = Color.red;
                labelContent.text = "NOT In Build";
                labelContent.tooltip = "This scene is NOT in build settings.\nIt will be NOT included in builds.";
            } else if (buildScene.Scene.enabled) {
                iconType = SdfIconType.CircleFill;
                iconColor = Color.green;
                labelContent.text = "BuildIndex: " + buildScene.BuildIndex;
                labelContent.tooltip = "This scene is in build settings and ENABLED.\nIt will be included in builds." + readOnlyWarning;
            } else {
                iconType = SdfIconType.CircleHalf;
                iconColor = Color.yellow;
                labelContent.text = "BuildIndex: " + buildScene.BuildIndex;
                labelContent.tooltip = "This scene is in build settings and DISABLED.\nIt will be NOT included in builds.";
            }

            using (new EditorGUI.DisabledScope(readOnly)) {
                var labelRect = GetLabelRect(position);
                var iconRect = labelRect;
                iconRect.width = IconSize;
                iconRect.height = IconSize;
                iconRect.y = labelRect.center.y - IconSize / 2f;
                labelRect.width -= iconRect.width + 2f * PadSize;
                labelRect.x += iconRect.width + 2f * PadSize;
                SdfIcons.DrawIcon(iconRect, iconType, iconColor);
                EditorGUI.PrefixLabel(labelRect, sceneControlID, labelContent);
            }

            var buttonRect = GetFieldRect(position);
            buttonRect.width /= 3;

            string tooltipMsg;
            using (new EditorGUI.DisabledScope(readOnly)) {
                if (buildScene.BuildIndex == -1) {
                    buttonRect.width *= 2;
                    var addIndex = EditorBuildSettings.scenes.Length;
                    tooltipMsg = "Add this scene to build settings. It will be appended to the end of the build scenes as buildIndex: " + addIndex + "." + readOnlyWarning;
                    if (ButtonHelper(buttonRect, "Add...", "Add (buildIndex " + addIndex + ")", EditorStyles.miniButtonLeft, tooltipMsg))
                        BuildUtils.AddBuildScene(buildScene);

                    buttonRect.width /= 2;
                    buttonRect.x += buttonRect.width;
                } else {
                    var isEnabled = buildScene.Scene.enabled;
                    var stateString = isEnabled ? "Disable" : "Enable";
                    tooltipMsg = stateString + " this scene in build settings.\n" + (isEnabled ? "It will no longer be included in builds" : "It will be included in builds") + "." + readOnlyWarning;

                    if (ButtonHelper(buttonRect, stateString, stateString + " In Build", EditorStyles.miniButtonLeft, tooltipMsg))
                        BuildUtils.SetBuildSceneState(buildScene, !isEnabled);

                    buttonRect.x += buttonRect.width;

                    tooltipMsg = "Completely remove this scene from build settings.\nYou will need to add it again for it to be included in builds!" + readOnlyWarning;
                    if (ButtonHelper(buttonRect, "Remove...", "Remove from Build", EditorStyles.miniButtonMid, tooltipMsg))
                        BuildUtils.RemoveBuildScene(buildScene);
                }
            }

            buttonRect.x += buttonRect.width;

            tooltipMsg = "Open the 'Build Settings' Window for managing scenes." + readOnlyWarning;
            if (ButtonHelper(buttonRect, "Settings", "Build Settings", EditorStyles.miniButtonRight, tooltipMsg))
                BuildUtils.OpenBuildSettings();
        }

        private static Rect GetFieldRect(Rect position)
        {
            position.width -= EditorGUIUtility.labelWidth;
            position.x += EditorGUIUtility.labelWidth;
            return position;
        }

        private static Rect GetLabelRect(Rect position)
        {
            position.width = EditorGUIUtility.labelWidth - PadSize;
            return position;
        }

        private static bool ButtonHelper(Rect position, string msgShort, string msgLong, GUIStyle style, string tooltip = null)
        {
            var content = new GUIContent(msgLong) {
                tooltip = tooltip
            };

            var longWidth = style.CalcSize(content).x;
            if (longWidth > position.width)
                content.text = msgShort;

            return GUI.Button(position, content, style);
        }

        private static class BuildUtils
        {
            private const float MinCheckWait = 3;

            private static float lastTimeChecked;
            private static bool cachedReadonlyVal = true;

            public static bool IsReadOnly()
            {
                var curTime = Time.realtimeSinceStartup;
                var timeSinceLastCheck = curTime - lastTimeChecked;

                if (timeSinceLastCheck > MinCheckWait) {
                    lastTimeChecked = curTime;
                    cachedReadonlyVal = QueryBuildSettingsStatus();
                }

                return cachedReadonlyVal;
            }

            private static bool QueryBuildSettingsStatus()
            {
                if (!Provider.enabled || !Provider.hasCheckoutSupport)
                    return false;

                var status = Provider.Status("ProjectSettings/EditorBuildSettings.asset", false);
                status.Wait();

                if (status.assetList is not { Count: 1 })
                    return true;

                return !status.assetList[0].IsState(Asset.States.CheckedOutLocal);
            }

            public static BuildScene GetBuildScene(Object sceneObject)
            {
                var entry = new BuildScene {
                    BuildIndex = -1,
                    AssetGuid = new GUID(string.Empty)
                };

                if (sceneObject as SceneAsset == null)
                    return entry;

                entry.AssetPath = AssetDatabase.GetAssetPath(sceneObject);
                entry.AssetGuid = new GUID(AssetDatabase.AssetPathToGUID(entry.AssetPath));

                for (var index = 0; index < EditorBuildSettings.scenes.Length; ++index) {
                    if (!entry.AssetGuid.Equals(EditorBuildSettings.scenes[index].guid))
                        continue;

                    entry.Scene = EditorBuildSettings.scenes[index];
                    entry.BuildIndex = index;
                    return entry;
                }

                return entry;
            }

            public static void SetBuildSceneState(BuildScene buildScene, bool enabled)
            {
                var modified = false;
                var scenesToModify = EditorBuildSettings.scenes;
                foreach (var curScene in scenesToModify) {
                    if (!curScene.guid.Equals(buildScene.AssetGuid))
                        continue;

                    curScene.enabled = enabled;
                    modified = true;
                    break;
                }

                if (modified) EditorBuildSettings.scenes = scenesToModify;
            }

            public static void AddBuildScene(BuildScene buildScene, bool force = false, bool enabled = true)
            {
                if (!force) {
                    var selection = EditorUtility.DisplayDialogComplex(
                        "Add Scene To Build",
                        "You are about to add scene at " + buildScene.AssetPath + " to the Build Settings.",
                        "Add as Enabled", // option 0
                        "Cancel", // option 1
                        "Add as Disabled"); // option 2

                    switch (selection) {
                        case 0: // Add as Enabled
                            enabled = true;
                            break;
                        case 2: // Add as Disabled
                            enabled = false;
                            break;
                        default: // Cancel
                            return;
                    }
                }

                var newScene = new EditorBuildSettingsScene(buildScene.AssetGuid, enabled);
                var tempScenes = EditorBuildSettings.scenes.ToList();
                tempScenes.Add(newScene);
                EditorBuildSettings.scenes = tempScenes.ToArray();
            }

            public static void RemoveBuildScene(BuildScene buildScene, bool force = false)
            {
                var onlyDisable = false;
                if (!force) {
                    int selection;

                    const string title = "Remove Scene From Build";
                    var message = string.Format("You are about to remove the following scene from build settings:\n    {0}\n    buildIndex: {1}\n\n{2}",
                        buildScene.AssetPath, buildScene.BuildIndex,
                        "This will modify build settings, but the scene asset will remain untouched.");
                    const string ok = "Remove From Build";
                    const string alt = "Just Disable";
                    const string cancel = "Cancel";

                    if (buildScene.Scene.enabled) {
                        message += "\n\nIf you want, you can also just disable it instead.";
                        selection = EditorUtility.DisplayDialogComplex(title, message, ok, cancel, alt);
                    } else {
                        selection = EditorUtility.DisplayDialog(title, message, ok, cancel) ? 0 : 1;
                    }

                    switch (selection) {
                        case 0: // Remove From Build
                            break;
                        case 2: // Just Disable
                            onlyDisable = true;
                            break;
                        default: // Cancel
                            return;
                    }
                }

                if (onlyDisable) {
                    SetBuildSceneState(buildScene, false);
                } else {
                    var tempScenes = EditorBuildSettings.scenes.ToList();
                    tempScenes.RemoveAll(scene => scene.guid.Equals(buildScene.AssetGuid));
                    EditorBuildSettings.scenes = tempScenes.ToArray();
                }
            }

            public static void OpenBuildSettings()
            {
                EditorWindow.GetWindow(typeof(BuildPlayerWindow));
            }

            public struct BuildScene
            {
                public int BuildIndex;
                public GUID AssetGuid;
                public string AssetPath;
                public EditorBuildSettingsScene Scene;
            }
        }
    }
}