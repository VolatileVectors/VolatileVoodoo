﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VolatileVoodoo.Setters
{
    public class RenderFeaturesSetter : MonoBehaviour
    {
        public List<ScriptableRendererFeatureSelector> features;

        private void Awake()
        {
            foreach (var item in features)
                item.feature.SetActive(item.enabled);
        }

        [Serializable]
        public struct ScriptableRendererFeatureSelector
        {
            public ScriptableRendererFeature feature;
            public bool enabled;
        }
    }
}