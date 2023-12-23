using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Utils;
using Random = UnityEngine.Random;

namespace VolatileVoodoo.Audio.Base
{
    public enum AudioClipOrder
    {
        Continuous,
        FlipFlop,
        Random
    }

    public enum SpatialType
    {
        [InspectorName("2D")]
        TwoD,

        [InspectorName("3D")]
        ThreeD
    }

    public abstract class BaseAudioEffect : ScriptableObject
    {
        [MinMaxSlider(0f, 1f)]
        public Vector2 volume = new(1f, 1f);

        [MinMaxSlider(-10f, 10f)]
        public Vector2Int semitones = new(0, 0);

        public SpatialType spatialType = SpatialType.ThreeD;

        [SerializeField]
        [HideInInspector]
        protected bool audioClipsValid;

        public virtual bool Init(AudioSource source)
        {
            source.enabled = true;
            source.volume = Random.Range(volume.x, volume.y);
            source.pitch = Mathf.Pow(Voodoo.SemitonesToPitchConversionUnit, Random.Range(semitones.x, semitones.y + 1));
            source.spatialBlend = spatialType switch {
                SpatialType.TwoD => 0f,
                SpatialType.ThreeD => 1f,
                _ => throw new ArgumentOutOfRangeException()
            };

            return true;
        }

#if UNITY_EDITOR
        private AudioSource previewSource;

        [Title("Preview")]
        [ShowInInspector]
        [DisplayAsString]
        [PropertyOrder(1f)]
        private string lastPlayed = "-";

        [ButtonGroup("Preview")]
        [Button("Play")]
        [PropertyOrder(2f)]
        [HideInPlayMode]
        private void Preview()
        {
            if (previewSource == null)
                previewSource = EditorUtility.CreateGameObjectWithHideFlags("AudioEffectPreview", HideFlags.HideAndDontSave, typeof(AudioSource)).GetComponent<AudioSource>();

            if (!Init(previewSource))
                return;

            lastPlayed = previewSource.clip.name;
            previewSource.Play();
        }

        [ButtonGroup("Preview")]
        [Button("Stop")]
        [PropertyOrder(2f)]
        [EnableIf(nameof(PreviewIsPlaying))]
        [HideInPlayMode]
        private void StopPreview()
        {
            if (previewSource == null)
                return;

            if (previewSource.isPlaying)
                previewSource.Stop();

            DestroyImmediate(previewSource);
            previewSource = null;
        }

        private void AutoPreview()
        {
            if (!Application.isPlaying && AutoPlayEffects && Selection.Contains(this)) {
                if (Selection.GetFiltered<BaseAudioEffect>(SelectionMode.Assets).Length == 1)
                    Preview();
            } else if (previewSource != null) {
                StopPreview();
            }
        }

        private bool PreviewIsPlaying => previewSource != null && previewSource.isPlaying;

        public static Action<bool> AutoPlayEffectsStateChanged;

        [Title("Settings")]
        [ToggleButton("Auto Play preview on asset selection.")]
        [ShowInInspector]
        [PropertyOrder(3f)]
        private bool AutoPlayEffects {
            get => EditorPrefs.GetBool(Voodoo.GetAutoPlayEffectsKey, false);
            set {
                EditorPrefs.SetBool(Voodoo.GetAutoPlayEffectsKey, value);
                if (value == false && PreviewIsPlaying)
                    StopPreview();

                AutoPlayEffectsStateChanged?.Invoke(value);
            }
        }

        protected virtual void OnEnable()
        {
            Selection.selectionChanged += AutoPreview;
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= AutoPreview;
        }
#endif
    }
}