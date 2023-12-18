using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VolatileVoodoo.Runtime.Setters
{
    public class RenderFeaturesSetter : MonoBehaviour
    {
        [Serializable]
        public struct ScriptableRendererFeatureSelector
        {
            public ScriptableRendererFeature feature;
            public bool enabled;
        }

        public List<ScriptableRendererFeatureSelector> features;

        private void Awake()
        {
            foreach (var item in features)
            {
                item.feature.SetActive(item.enabled);
            }
        }
    }
}