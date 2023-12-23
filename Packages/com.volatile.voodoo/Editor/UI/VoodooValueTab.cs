using System;
using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Editor.CodeGenerator;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooValueTab : VoodooGeneratorTab
    {
        private string[] nameSpaces;
        private TextField valuePayload;
        private Type valuePayloadType;

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            valuePayload = rootVisualElement.Q<TextField>("valueType");
            valuePayload.RegisterValueChangedCallback(OnPayloadChanged);

            base.CreateGUI(rootVisualElement);
        }

        private void OnPayloadChanged(ChangeEvent<string> text)
        {
            valuePayloadType = CodeGeneratorUtils.ResolveTypeFromString(text.newValue, out nameSpaces);
            if (valuePayloadType != null) {
                var prettyName = valuePayloadType.PrettyName();
                valuePayload.cursorIndex = CodeGeneratorUtils.AdjustCursor(text.newValue, prettyName, valuePayload.cursorIndex);
                valuePayload.SetValueWithoutNotify(prettyName);
                valuePayload.SelectNone();
            }

            if (AutoSuggest.value)
                UpdateSuggestion();

            Validate();
        }

        protected override void UpdateSuggestion()
        {
            var suggestion = "CustomValue";
            if (valuePayloadType != null)
                suggestion = CodeGeneratorUtils.PrettyIdentifier(valuePayload.value) + "Value";

            Name.value = suggestion;
        }

        protected override void Validate()
        {
            IsValid = true;

            if (valuePayloadType != null) {
                valuePayload.RemoveFromClassList("invalid");
                valuePayload.tooltip = $"Type: {valuePayloadType.FullName}";
            } else {
                valuePayload.tooltip = "Can not resolve type!";
                valuePayload.AddToClassList("invalid");
                IsValid = false;
            }

            base.Validate();
        }

        protected override void OnCreate()
        {
            if (!IsValid)
                return;

            var outputPath = Voodoo.VoodooAssetPath("Values");

            ValueTemplate valueTemplate = new(outputPath) {
                Namespace = Voodoo.AssetPathToNamespace(outputPath),
                Imports = nameSpaces,
                ValueName = Name.value,
                ValueType = valuePayload.value
            };

            var referenceName = Name.value.Replace("Value", "") + "Reference";
            ValueReferenceTemplate valueReferenceTemplate = new(outputPath) {
                Namespace = Voodoo.AssetPathToNamespace(outputPath),
                Imports = nameSpaces,
                ValueName = Name.value,
                ValueReferenceName = referenceName,
                ValueReferenceType = valuePayload.value
            };

            valueTemplate.Create();
            valueReferenceTemplate.Create();
            AssetDatabase.Refresh();
        }
    }
}