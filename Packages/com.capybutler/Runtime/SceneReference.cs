using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Capybutler
{
    [Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
        [SerializeField]
        private string scenePath = string.Empty;

        public string ScenePath
        {
            get
            {
#if UNITY_EDITOR
                return SceneAssetPath;
#else
                return scenePath;
#endif
            }
            set
            {
                scenePath = value;
#if UNITY_EDITOR
                sceneAsset = SceneAssetAtPath;
#endif
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (sceneAsset == null && !string.IsNullOrEmpty(scenePath)) {
                sceneAsset = SceneAssetAtPath;
                if (sceneAsset == null)
                    scenePath = string.Empty;

                EditorSceneManager.MarkAllScenesDirty();
            } else
                scenePath = SceneAssetPath;
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            EditorApplication.update += HandleAfterDeserialize;
#endif
        }

        public static implicit operator string(SceneReference sceneReference) => sceneReference.ScenePath;

#if UNITY_EDITOR
        private void HandleAfterDeserialize()
        {
            EditorApplication.update -= HandleAfterDeserialize;

            if (sceneAsset != null || string.IsNullOrEmpty(scenePath))
                return;

            sceneAsset = SceneAssetAtPath;
            if (sceneAsset == null)
                scenePath = string.Empty;

            if (!Application.isPlaying)
                EditorSceneManager.MarkAllScenesDirty();
        }

        [SerializeField]
        private SceneAsset sceneAsset;

        private string SceneAssetPath => sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
        private SceneAsset SceneAssetAtPath => string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
#endif
    }
}