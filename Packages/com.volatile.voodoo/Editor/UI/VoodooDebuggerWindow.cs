using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VolatileVoodoo.Utils;

namespace VolatileVoodoo.Editor.UI
{
    public class VoodooDebuggerWindow : EditorWindow
    {
        private const string TabClassname = "tab";
        private const string TabSuffix = "Tab";
        private const string ContentSuffix = "Content";
        private const string SelectedTab = "selectedTab";
        private const string UnselectedTab = "unselectedTab";
        private const string UnselectedContent = "unselectedContent";

        private VoodooDebugEventsTab eventTab;
        private VoodooDebugValuesTab valueTab;

        private void CreateGUI()
        {
            var root = rootVisualElement;
            root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>(Voodoo.VoodooPackagePath("com.volatile.voodoo", "Editor/UI/Voodoo.uss")));
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Voodoo.VoodooPackagePath("com.volatile.voodoo", "Editor/UI/VoodooDebugger.uxml"));
            root.Add(visualTree.Instantiate());

            eventTab = new();
            valueTab = new();
            eventTab.CreateGUI(root.Q<VisualElement>("debugEventsContent"));
            valueTab.CreateGUI(root.Q<VisualElement>("debugValuesContent"));

            root.Query<Label>(className: TabClassname).ForEach(tab => tab.RegisterCallback<ClickEvent>(OnTabClicked));
            root.Query<Button>("create").ForEach(tab => tab.clicked += Close);
        }

        [MenuItem("Volatile Voodoo/Debug/Live Voodoo Events", false, priority = 200)]
        private static void OpenEventWindow()
        {
            CreateWindow("debugEventsTab");
        }

        [MenuItem("Volatile Voodoo/Debug/Live Voodoo Values", false, priority = 201)]
        private static void OpenValueWindow()
        {
            CreateWindow("debugValuesTab");
        }

        private static void CreateWindow(string activeAtStart)
        {
            var window = GetWindow<VoodooDebuggerWindow>(false, "Voodoo Events & Values Inspector");
            window.minSize = new Vector2(640, 480);
            window.maxSize = new Vector2(1920, 1200);
            window.SetStartTab(activeAtStart);
            window.Show();
        }

        private void SetStartTab(string tabName)
        {
            UpdateTabBar(rootVisualElement.Q<Label>(tabName));
        }

        private void OnTabClicked(ClickEvent click)
        {
            UpdateTabBar(click.target as Label);
        }

        private void UpdateTabBar(Label tabLabel)
        {
            if (tabLabel?.ClassListContains(SelectedTab) ?? true)
                return;

            rootVisualElement
                .Query<Label>(className: TabClassname)
                .Where(tab => tab != tabLabel && tab.ClassListContains(SelectedTab))
                .ForEach(UnselectTab);
            SelectTab(tabLabel);
        }

        private void UnselectTab(VisualElement tab)
        {
            tab.RemoveFromClassList(SelectedTab);
            tab.AddToClassList(UnselectedTab);
            var tabContent = rootVisualElement.Q(tab.name.Replace(TabSuffix, ContentSuffix));
            tabContent.AddToClassList(UnselectedContent);
        }

        private void SelectTab(VisualElement tab)
        {
            tab.RemoveFromClassList(UnselectedTab);
            tab.AddToClassList(SelectedTab);
            var tabContent = rootVisualElement.Q(tab.name.Replace(TabSuffix, ContentSuffix));
            tabContent.RemoveFromClassList(UnselectedContent);
        }
    }
}