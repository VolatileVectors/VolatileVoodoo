using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VolatileVoodoo.Runtime.Values.Base
{
    public abstract class GenericValue : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        [LabelWidth(100)]
        public string description = "";
#endif
    }

    public abstract class GenericValue<TData> : GenericValue
    {
        [SerializeField]
        [LabelWidth(100)]
        [OnValueChanged(nameof(OnInitialValueChanged))]
        private TData initialValue;

        private TData value;
        private Action<TData> valueChanged;

        [ShowInInspector]
        [LabelWidth(100)]
        [ReadOnly]
        public TData Value {
            get => value;
            set {
                if (this.value.Equals(value))
                    return;

                this.value = value;
                valueChanged?.Invoke(this.value);
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
    }
}