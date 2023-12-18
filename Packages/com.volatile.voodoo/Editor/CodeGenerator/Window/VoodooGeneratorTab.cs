using Sirenix.Utilities;
using UnityEngine.UIElements;

namespace VolatileVoodoo.Editor.CodeGenerator.Window
{
    public abstract class VoodooGeneratorTab
    {
        protected Toggle AutoSuggest;
        private Button create;
        private Toggle force;
        protected bool IsValid;
        protected TextField Name;
        private string savedName = "";

        public virtual void CreateGUI(VisualElement rootVisualElement)
        {
            Name = rootVisualElement.Q<TextField>("name");
            Name.RegisterValueChangedCallback(_ => Validate());
            AutoSuggest = rootVisualElement.Q<Toggle>("autoSuggest");
            AutoSuggest.RegisterValueChangedCallback(ToggleSuggestions);

            force = rootVisualElement.Q<Toggle>("force");
            force.RegisterValueChangedCallback(_ => Validate());
            create = rootVisualElement.Q<Button>("create");
            create.SetEnabled(false);
            create.clicked += OnCreate;

            AutoSuggest.value = true;
            Validate();
        }

        private void ToggleSuggestions(ChangeEvent<bool> status)
        {
            if (status.newValue) {
                savedName = Name.value;
                UpdateSuggestion();
            } else {
                Name.value = savedName;
            }

            Validate();
        }

        protected abstract void UpdateSuggestion();

        protected virtual void Validate()
        {
            if (Name.value.IsNullOrWhitespace() || !System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(Name.value)) {
                Name.AddToClassList("invalid");
                IsValid = false;
            } else {
                Name.RemoveFromClassList("invalid");
            }

            create.SetEnabled(force.value || IsValid);
        }

        protected abstract void OnCreate();
    }
}