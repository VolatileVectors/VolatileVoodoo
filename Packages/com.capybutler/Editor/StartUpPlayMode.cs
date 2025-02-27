using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Capybutler.Editor
{
    [InitializeOnLoad]
    public static class StartUpPlayMode
    {
        private const string PlayModeMenu = "Capybutler/Enter PlayMode on StartUp scene #p";
        private const string PlayModeOverride = "Capybutler/Default PlayMode on StartUp scene";

        private static bool enterPlayModeOverride;

        public static bool DefaultPlayModeOverrideEnabled
        {
            get => EditorPrefs.GetBool(PathUtils.GetEditorKey(nameof(DefaultPlayModeOverrideEnabled)), false);
            private set => EditorPrefs.SetBool(PathUtils.GetEditorKey(nameof(DefaultPlayModeOverrideEnabled)), value);
        }

        static StartUpPlayMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        [MenuItem(PlayModeMenu, false, 10)]
        public static void EnterPlayMode()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            enterPlayModeOverride = true;
            EditorApplication.EnterPlaymode();
        }

        [MenuItem(PlayModeOverride, false, 20)]
        public static void ToggleDefaultPlayModeOverride()
        {
            DefaultPlayModeOverrideEnabled = !DefaultPlayModeOverrideEnabled;
        }

        [MenuItem(PlayModeOverride, true)]
        public static bool ValidateDefaultPlayModeOverride()
        {
            Menu.SetChecked(PlayModeOverride, DefaultPlayModeOverrideEnabled);
            return true;
        }

        private static void OnPlayModeStateChange(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
                SetSceneByPath(SceneManager.GetActiveScene().path);
            else if (state == PlayModeStateChange.ExitingEditMode) {
                SetSceneByPath(EditorBuildSettings.scenes.Length > 0 && (DefaultPlayModeOverrideEnabled || enterPlayModeOverride) ? EditorBuildSettings.scenes[0].path : SceneManager.GetActiveScene().path);
                enterPlayModeOverride = false;
            }
        }

        private static void SetSceneByPath(string path) => EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
    }
}