using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public partial class EnumsWindow : OdinMenuEditorWindow
{
    private static Type[] typesToDisplay = TypeCache.GetTypesWithAttribute<EnumAttribute>().OrderBy(x => x.Name).ToArray();
    private bool autoSaveOnLostFocus = false;
    private bool toggleScriptArea;

    [MenuItem("Scivr/Window/Enums")]
    private static void OpenWindow()
    {
        var window = GetWindow<EnumsWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        EditorWindowStateHelper.OnEditorMinimized += OnEditorMinimized;
        EditorWindowStateHelper.OnEditorRestored += OnEditorRestored;
        CompilationTracker.OncompilationStarted += OnCompilationStarted;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        EditorWindowStateHelper.OnEditorMinimized -= OnEditorMinimized;
        EditorWindowStateHelper.OnEditorRestored -= OnEditorRestored;
        CompilationTracker.OncompilationStarted -= OnCompilationStarted;
    }
    

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree(supportsMultiSelect: true)
        {
            DefaultMenuStyle = { IconSize = 28.0f },
            Config = { DrawSearchToolbar = true,SearchToolbarHeight = 25 }
        };
        foreach (Type type in typesToDisplay)
        {
            if (type != null)
            {
                tree.Add("Enums/" + type.FullName, new EnumViewer(type));
            }
        }

        tree.SortMenuItemsByName();
        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var selectedScriptViewer = selected?.Value as EnumViewer;
        var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;

        SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);
        {
            if (selected != null)
            {
                GUILayout.Label(selected.Name);
            }

            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
            {
                ForceMenuTreeRebuild();
            }
            
            if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Enum")))
            {
                InputDialog.Show((enumName) =>
                {
                    selectedScriptViewer.EnumName = enumName;
                    selectedScriptViewer?.SaveEnum();
                });
            }

            if (SirenixEditorGUI.ToolbarButton(SdfIconType.Trash))
            {

                string selectedNameColored = $"*{selected.Name}*";
                string dialogMessage = $"Are you sure you want to delete {selectedNameColored} enum?";
                bool userConfirmed = EditorUtility.DisplayDialog("Confirm Clear",dialogMessage,"Yes","No");

                // Check if the user clicked 'Yes'
                if (userConfirmed)
                {
                    selectedScriptViewer?.DeleteEnum(selected.Name);
                }

            }

            GUILayout.FlexibleSpace();

            autoSaveOnLostFocus = GUILayout.Toggle(autoSaveOnLostFocus, new GUIContent("AutoSave"), "Button");
            bool newToggleScriptArea = GUILayout.Toggle(toggleScriptArea, new GUIContent("ToggleScriptArea"), "Button");
            if (newToggleScriptArea != toggleScriptArea)
            {
                toggleScriptArea = newToggleScriptArea;
                selectedScriptViewer?.ToggleScriptVisibility();
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

    private void OnLostFocus()
    {
        if (!autoSaveOnLostFocus) return;

        var selected = this.MenuTree.Selection.FirstOrDefault();
        var selectedScriptViewer = selected?.Value as EnumViewer;
        selectedScriptViewer?.SaveEnum();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var selectedScriptViewer = selected?.Value as EnumViewer;
        selectedScriptViewer?.SaveEnum();
    }

    private void OnEditorMinimized()
    {
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var selectedScriptViewer = selected?.Value as EnumViewer;
        selectedScriptViewer?.SaveEnum();
    }

    private void OnEditorRestored()
    {
        Debug.Log("Editor window restored");
        // Perform actions on restore
    }

    private void OnCompilationStarted()
    {
        var selected = this.MenuTree.Selection.FirstOrDefault();
        var selectedScriptViewer = selected?.Value as EnumViewer;
        selectedScriptViewer?.SaveEnum();
    }
}
