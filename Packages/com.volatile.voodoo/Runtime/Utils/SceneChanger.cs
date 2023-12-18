using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using VolatileVoodoo.Runtime.Events;

namespace VolatileVoodoo.Runtime.Utils
{
    public class SceneChanger : MonoBehaviour
    {
        public SceneReference nextScene;

        [Tooltip("Loading progress from 0 to 100")]
        public IntEventSource progressEventSource;

        [Title("Settings")]
        [ToggleButton("Load additive")]
        public bool loadAdditive;

        [ToggleButton("Activate immediately")]
        public bool activateImmediately = true;

        private AsyncOperation sceneLoadOperation;
        private bool isLoading;

        public void LoadNext()
        {
            if (isLoading) {
                return;
            }

            isLoading = true;

            sceneLoadOperation = SceneManager.LoadSceneAsync(nextScene, loadAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
            sceneLoadOperation.completed += OnFinishedLoading;
            sceneLoadOperation.allowSceneActivation = activateImmediately;
        }

        private void OnFinishedLoading(AsyncOperation sceneLoadedOperation)
        {
            if (!sceneLoadedOperation.isDone) {
                return;
            }

            progressEventSource.Raise(100);
        }

        private void Update()
        {
            if (!isLoading) {
                return;
            }

            progressEventSource.Raise(Mathf.CeilToInt(sceneLoadOperation.progress * 100f));
        }

        public void Ready()
        {
            sceneLoadOperation.allowSceneActivation = true;
        }
    }
}