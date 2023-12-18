using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VolatileVoodoo.Runtime.Events;

namespace VolatileVoodoo.Runtime.Utils
{
    [CreateAssetMenu(fileName = "GameLoadPrefabSpawner", menuName = "Voodoo/GameLoadPrefabSpawner")]
    public class GameLoadPrefabSpawner : ScriptableObject
    {
        public GameEventSource prefabsReadySource;

        [AssetsOnly]
        public List<GameObject> prefabs;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnLoadFirstScene()
        {
            if (!Application.isPlaying) return;

            Resources.Load<GameLoadPrefabSpawner>("GameLoadPrefabSpawner")?.LoadPrefabs();
        }

        private void LoadPrefabs()
        {
            foreach (var prefab in prefabs) {
                if (prefab == null)
                    continue;

                var go = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                go.name = $"[{prefab.name}]";
                go.hideFlags = HideFlags.DontSave;
                DontDestroyOnLoad(go);
            }

            prefabsReadySource.Raise();
        }
    }
}