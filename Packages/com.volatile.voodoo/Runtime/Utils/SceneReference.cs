using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace VolatileVoodoo.Runtime.Utils
{
    [Serializable]
    public class SceneReference : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField]
        private SceneAsset sceneAsset;

        private string GetPathFromSceneAsset => sceneAsset == null ? string.Empty : AssetDatabase.GetAssetPath(sceneAsset);
        private SceneAsset GetSceneAssetFromPath => string.IsNullOrEmpty(scenePath) ? null : AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
#endif

        [SerializeField]
        private string scenePath = string.Empty;

        public string ScenePath {
            get {
#if UNITY_EDITOR
                return GetPathFromSceneAsset;
#else
                return scenePath;
#endif
            }
            set {
                scenePath = value;
#if UNITY_EDITOR
                sceneAsset = GetSceneAssetFromPath;
#endif
            }
        }

        public static implicit operator string(SceneReference sceneReference)
        {
            return sceneReference.ScenePath;
        }


        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (sceneAsset == null && !string.IsNullOrEmpty(scenePath)) {
                sceneAsset = GetSceneAssetFromPath;
                if (sceneAsset == null) {
                    scenePath = string.Empty;
                }

                EditorSceneManager.MarkAllScenesDirty();
            } else {
                scenePath = GetPathFromSceneAsset;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            EditorApplication.update += HandleAfterDeserialize;
#endif
        }

#if UNITY_EDITOR
        private void HandleAfterDeserialize()
        {
            EditorApplication.update -= HandleAfterDeserialize;

            if (sceneAsset != null || string.IsNullOrEmpty(scenePath)) {
                return;
            }

            sceneAsset = GetSceneAssetFromPath;
            if (sceneAsset == null) {
                scenePath = string.Empty;
            }

            if (!Application.isPlaying) {
                EditorSceneManager.MarkAllScenesDirty();
            }
        }
#endif
    }
}