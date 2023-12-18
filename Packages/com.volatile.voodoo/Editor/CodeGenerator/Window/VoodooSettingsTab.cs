using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VolatileVoodoo.Runtime.Utils;

namespace VolatileVoodoo.Editor.CodeGenerator.Window
{
    public class VoodooSettingsTab
    {
        private TextField path;
        private Toggle referenceInlineEditorState;
        private Toggle eventListenerInlineEditorState;
        private Toggle autoPlayEffectsState;

        public void CreateGUI(VisualElement rootVisualElement)
        {
            path = rootVisualElement.Q<TextField>("path");
            referenceInlineEditorState = rootVisualElement.Q<Toggle>("referenceInlineEditor");
            eventListenerInlineEditorState = rootVisualElement.Q<Toggle>("eventListenerInlineEditor");
            autoPlayEffectsState = rootVisualElement.Q<Toggle>("autoPlayEffects");

            path.value = EditorPrefs.GetString(Voodoo.GetSettingsSavePath, Voodoo.DefaultSavePath);
            
            referenceInlineEditorState.value = EditorPrefs.GetBool(Voodoo.GetShowGenericReferenceValueKey, false);
            eventListenerInlineEditorState.value = EditorPrefs.GetBool(Voodoo.GetShowGenericEventDebuggerKey, false);
            autoPlayEffectsState.value = EditorPrefs.GetBool(Voodoo.GetAutoPlayEffectsKey, false);

            rootVisualElement.Q<Button>("selectPath").clicked += OnPathSelect;
            path.RegisterValueChangedCallback(status => EditorPrefs.SetString(Voodoo.GetSettingsSavePath, status.newValue));
            referenceInlineEditorState.RegisterValueChangedCallback(status => EditorPrefs.SetBool(Voodoo.GetShowGenericReferenceValueKey, status.newValue));
            eventListenerInlineEditorState.RegisterValueChangedCallback(status => EditorPrefs.SetBool(Voodoo.GetShowGenericEventDebuggerKey, status.newValue));
            autoPlayEffectsState.RegisterValueChangedCallback(status => EditorPrefs.SetBool(Voodoo.GetAutoPlayEffectsKey, status.newValue));
        }

        private void OnPathSelect()
        {
            var currentPath = Path.GetFullPath(string.IsNullOrWhiteSpace(path.value) ? Voodoo.DefaultSavePath : path.value);
            var newPath = EditorUtility.OpenFolderPanel("Select " + path.label, currentPath, "");
            path.value = string.IsNullOrWhiteSpace(newPath) ? path.value : Path.GetFullPath(newPath);
        }

        public void OnReferenceInlineEditorStateChanged(bool value) => referenceInlineEditorState.SetValueWithoutNotify(value);

        public void OnEventListenerInlineEditorStateChanged(bool value) => eventListenerInlineEditorState.SetValueWithoutNotify(value);

        public void OnAutoPlayEffectsStateChanged(bool value) => autoPlayEffectsState.SetValueWithoutNotify(value);
    }
}