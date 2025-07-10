using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LanguagesWindow : OdinMenuEditorWindow
{
    //private const float FixedMenuWidth = 250.0f; // Set the fixed width for the menu

    //[MenuItem("Scivr/Window/Language")]
    //private static void OpenWindow()
    //{
    //    var window = GetWindow<LanguagesWindow>();
    //    window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    //}

    //protected override OdinMenuTree BuildMenuTree()
    //{
    //    var tree = new OdinMenuTree(supportsMultiSelect: true)
    //    {

    //        DefaultMenuStyle = { IconSize = 30.0f },
    //        Config = { DrawSearchToolbar = true, SearchToolbarHeight = 20 }
    //    };

    //    var languageHolders = AssetDatabase.FindAssets("t:LanguageHolder")
    //        .Select(guid => AssetDatabase.LoadAssetAtPath<LanguageHolder>(AssetDatabase.GUIDToAssetPath(guid)))
    //        .ToList();

    //    Add LanguageHolder assets to the tree
    //    foreach (var languageHolder in languageHolders)
    //    {
    //        tree.Add(languageHolder.name, languageHolder);
    //    }

    //    tree.SortMenuItemsByName();
    //    return tree;
    //}

    //private static void CreateNewLanguageHolder()
    //{
    //    Show the input dialog to get the name of the new LanguageHolder
    //    InputDialog.Show((languageName) =>
    //    {
    //        if (string.IsNullOrEmpty(languageName)) return;

    //        Find the path of an existing LanguageHolder
    //        string existingAssetPath = AssetDatabase.FindAssets("t:LanguageHolder")
    //            .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
    //            .FirstOrDefault();

    //        string folderPath = "Assets"; // Default folder if no existing asset is found

    //        if (!string.IsNullOrEmpty(existingAssetPath))
    //        {
    //            folderPath = Path.GetDirectoryName(existingAssetPath);
    //        }

    //        var newLanguageHolder = ScriptableObject.CreateInstance<LanguageHolder>();
    //        newLanguageHolder.name = languageName;

    //        string path = Path.Combine(folderPath, $"{languageName}.asset");
    //        AssetDatabase.CreateAsset(newLanguageHolder, path);
    //        AssetDatabase.SaveAssets();

    //        Open the window and select the new asset
    //        var window = GetWindow<LanguagesWindow>();
    //        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
    //        window.TrySelectMenuItemWithObject(newLanguageHolder);
    //    });
    //}

    //protected override void OnBeginDrawEditors()
    //{
    //    var toolbarHeight = this.MenuTree.Config.SearchToolbarHeight;
    //    SirenixEditorGUI.BeginHorizontalToolbar(toolbarHeight);

    //    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Refresh")))
    //    {
    //        ForceMenuTreeRebuild();
    //    }
    //    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Language")))
    //    {
    //        CreateNewLanguageHolder();
    //    }
    //    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create")))
    //    {
    //        ScriptableObjectCreator.ShowDialog<LanguageHolder>("Assets/_Project/LanguageSo", obj =>
    //        {
    //            base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
    //        });
    //    }

    //    GUILayout.FlexibleSpace();
    //    SirenixEditorGUI.EndHorizontalToolbar();
    //}
    //protected override void DrawMenu()
    //{
    //    GUILayout.MinWidth(100f);

    //    base.DrawMenu();
    //}
    protected override OdinMenuTree BuildMenuTree()
    {
        throw new System.NotImplementedException();
    }
}
