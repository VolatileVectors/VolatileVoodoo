using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VolatileVoodoo.Values.Base
{
    public abstract class GenericValue : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        [LabelWidth(100)]
        public string description = "";

        public abstract string InitialValueDebug { get; }
        public abstract string CurrentValueDebug { get; }
        public Action<string> debugValueChanged;
#endif
    }

    public abstract class GenericValue<TData> : GenericValue
    {
        [SerializeField]
        [LabelWidth(100)]
        [OnValueChanged(nameof(OnInitialValueChanged))]
        private TData initialValue;

        private TData currentValue;
        private Action<TData> valueChanged;

        [ShowInInspector]
        [LabelWidth(100)]
        [ReadOnly]
        public TData Value {
            get => currentValue;
            set {
                if (currentValue?.Equals(value) ?? value == null)
                    return;

                currentValue = value;
                valueChanged?.Invoke(currentValue);

#if UNITY_EDITOR
                debugValueChanged?.Invoke(value?.ToString() ?? "null");
#endif
            }
        }

        private void OnEnable()
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
            OnInitialValueChanged();
        }

        public event Action<TData> OnValueChanged {
            add => valueChanged += value;
            remove => valueChanged -= value;
        }

        private void OnInitialValueChanged()
        {
            Value = initialValue;
        }

#if UNITY_EDITOR
        public override string InitialValueDebug => initialValue?.ToString() ?? "null";
        public override string CurrentValueDebug => currentValue?.ToString() ?? "null";
#endif
    }
}