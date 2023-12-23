using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine.UIElements;
using VolatileVoodoo.Editor.CodeGenerator;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooEventTab : VoodooGeneratorTab
    {
        private readonly TextField[] eventPayloads = new TextField[4];
        private readonly Type[] eventPayloadTypes = { null, null, null, null };
        private readonly string[][] nameSpaces = { null, null, null, null };

        public override void CreateGUI(VisualElement rootVisualElement)
        {
            var source = 'A';
            for (var i = 0; i < 4; ++i) {
                var index = i;
                eventPayloads[i] = rootVisualElement.Q<TextField>("payload" + source++);
                eventPayloads[i].RegisterValueChangedCallback(text => OnPayloadChanged(index, text.newValue));
            }

            base.CreateGUI(rootVisualElement);
        }

        private void OnPayloadChanged(int index, string text)
        {
            eventPayloadTypes[index] = CodeGeneratorUtils.ResolveTypeFromString(text, out nameSpaces[index]);
            if (eventPayloadTypes[index] != null) {
                var prettyName = eventPayloadTypes[index].PrettyName();
                eventPayloads[index].cursorIndex = CodeGeneratorUtils.AdjustCursor(text, prettyName, eventPayloads[index].cursorIndex);
                eventPayloads[index].SetValueWithoutNotify(prettyName);
                eventPayloads[index].SelectNone();
            }

            if (AutoSuggest.value)
                UpdateSuggestion();

            Validate();
        }

        protected override void UpdateSuggestion()
        {
            var suggestion = "";
            for (var i = 0; i < 4; ++i) {
                if (eventPayloadTypes[i] == null)
                    continue;

                suggestion += CodeGeneratorUtils.PrettyIdentifier(eventPayloads[i].value);
            }

            if (suggestion.IsNullOrWhitespace())
                suggestion = "Custom";

            Name.value = suggestion + "Event";
        }

        protected override void Validate()
        {
            IsValid = true;
            for (var i = 0; i < 4; ++i) {
                var unused = eventPayloads[i].value.IsNullOrWhitespace();
                if (unused || eventPayloadTypes[i] != null) {
                    eventPayloads[i].tooltip = unused ? "" : $"Type: {eventPayloadTypes[i].FullName}";
                    eventPayloads[i].RemoveFromClassList("invalid");
                } else {
                    eventPayloads[i].tooltip = "Can not resolve type!";
                    eventPayloads[i].AddToClassList("invalid");
                    IsValid = false;
                }
            }

            base.Validate();
        }

        protected override void OnCreate()
        {
            if (!IsValid)
                return;

            var payloads = eventPayloads.Select(textField => textField.value).Where(item => !item.IsNullOrWhitespace()).ToList().ConvertAll(item => item.Replace(" ", "")).ToArray();
            var includes = nameSpaces.Where(nameSpace => nameSpace != null).Aggregate(new List<string>(), (strings, list) => list.Union(strings).ToList()).Distinct().ToArray();
            var outputPath = Voodoo.VoodooAssetPath("Events");

            EventTemplate eventTemplate = new(outputPath) {
                Namespace = Voodoo.AssetPathToNamespace(outputPath),
                Imports = includes,
                EventName = Name.value,
                EventPayloads = payloads
            };

            EventListenerTemplate eventListenerTemplate = new(outputPath) {
                Namespace = Voodoo.AssetPathToNamespace(outputPath),
                Imports = includes,
                EventName = Name.value,
                EventPayloads = payloads
            };

            eventTemplate.Create();
            eventListenerTemplate.Create();
            AssetDatabase.Refresh();
        }
    }
}