using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodoo.Runtime.Values.Base
{
    [Serializable]
    public abstract class GenericReference
    {
#if UNITY_EDITOR
        public static Action<bool> InlineEditorStateChanged;
#endif

        [HideLabel]
        [SerializeField]
        [HorizontalGroup("Reference", 75f)]
        [ValueDropdown(nameof(literalList))]
        protected bool useLiteral = true;

        private static ValueDropdownList<bool> literalList = new() {
            { "by Val", true },
            { "by Ref", false }
        };
    }

    [Serializable]
    [InlineProperty]
    public abstract class GenericReference<TValue, TData> : GenericReference where TValue : GenericValue<TData>
    {
        [HideLabel]
        [SerializeField]
        [ShowIf(nameof(useLiteral), Animate = false)]
        [HorizontalGroup("Reference")]
        private TData literalValue;

        [HideLabel]
        [SerializeField]
        [HideIf(nameof(useLiteral), Animate = false)]
        [HorizontalGroup("Reference")]
        [OnValueChanged(nameof(UpdateValueEditor))]
        private TValue value;

        public TData Value => useLiteral || value == null ? literalValue : value.Value;

        public event Action<TData> OnValueChanged {
            add {
                if (!useLiteral && this.value != null) {
                    this.value.OnValueChanged += value;
                }
            }
            remove {
                if (!useLiteral && this.value != null) {
                    this.value.OnValueChanged -= value;
                }
            }
        }

#if UNITY_EDITOR
        [HideLabel]
        [ShowInInspector]
        [ShowIf("@" + nameof(value) + " != null && " + nameof(useLiteral) + " == false", Animate = false)]
        [HorizontalGroup("Reference", 25f)]
        [ToggleButton("Edit in inline editor", SdfIconType.PencilFill, SdfIconType.Pencil)]
        private bool Edit {
            get => EditorPrefs.GetBool(Voodoo.GetShowGenericReferenceValueKey, false);
            set {
                EditorPrefs.SetBool(Voodoo.GetShowGenericReferenceValueKey, value);
                InlineEditorStateChanged?.Invoke(value);
            }
        }

        [ShowIf("@" + nameof(value) + " != null && " + nameof(useLiteral) + " == false && " + nameof(Edit) + " == true")]
        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        [OnInspectorInit(nameof(UpdateValueEditor))]
        private TValue valueEditor;
#endif


        private void UpdateValueEditor()
        {
#if UNITY_EDITOR
            valueEditor = value;
#endif
        }

        public static implicit operator TData(GenericReference<TValue, TData> reference)
        {
            return reference.Value;
        }
    }
}