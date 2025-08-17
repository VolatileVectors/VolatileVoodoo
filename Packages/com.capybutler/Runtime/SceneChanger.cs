using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Capybutler
{
    public class SceneChanger : MonoBehaviour
    {
        public SceneReference nextScene;

        [Tooltip("Loading progress from 0 to 100")]
        public UnityEvent<int> progress;

        [Header("Settings")]
        public bool loadAdditive;

        public bool activateImmediately = true;

        private bool isLoading;

        private AsyncOperation sceneLoadOperation;

        private void Update()
        {
            if (!isLoading)
                return;

            progress?.Invoke(Mathf.CeilToInt(sceneLoadOperation.progress * 100f));
        }

        public void LoadNext()
        {
            if (isLoading)
                return;

            isLoading = true;

            sceneLoadOperation = SceneManager.LoadSceneAsync(nextScene, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            if (sceneLoadOperation == null)
                return;

            sceneLoadOperation.completed += OnFinishedLoading;
            sceneLoadOperation.allowSceneActivation = activateImmediately;
        }

        private void OnFinishedLoading(AsyncOperation sceneLoadedOperation)
        {
            if (!sceneLoadedOperation.isDone)
                return;

            progress?.Invoke(100);
        }

        public void Ready()
        {
            sceneLoadOperation.allowSceneActivation = true;
        }
    }
}