using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Capybutler.Editor.Utils
{
    [InitializeOnLoad]
    public static class StartUpPlayMode
    {
        private const string PlayModeMenu = "Capybutler/Enter PlayMode on Root Scene %q";

        private static bool isEnteringPlaymode;

        static StartUpPlayMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;
        }

        [MenuItem(PlayModeMenu, false, 0)]
        public static void EnterPlayMode()
        {
            isEnteringPlaymode = true;
            // Set up the default scene
            SetSceneByPath(EditorBuildSettings.scenes[0].path);
            // Force-enter the playmode.
            EditorApplication.EnterPlaymode();
        }

        private static void OnPlayModeStateChange(PlayModeStateChange state)
        {
            // Restore the default scene (if this playmode has not been triggered using shortcut)
            if (isEnteringPlaymode) {
                isEnteringPlaymode = false;
                return;
            }

            SetSceneByPath(SceneManager.GetActiveScene().path);
        }

        private static void SetSceneByPath(string path) => EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
    }
}