using UnityEditor;
using UnityEngine;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Demos.RPGEditor;
using Sirenix.Utilities;
using Sirenix.OdinInspector;

public class LocalizationWindow : OdinMenuEditorWindow
{
    //private const float FixedMenuWidth = 250.0f; // Set the fixed width for the menu

    //[MenuItem("Scivr/Window/Localization")]
    //private static void OpenWindow()
    //{
    //    var window = GetWindow<LocalizationWindow>();
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

    //    var audiosHolders = AssetDatabase.FindAssets("t:AudioLocoliztion")
    //        .Select(guid => AssetDatabase.LoadAssetAtPath<AudioLocoliztion>(AssetDatabase.GUIDToAssetPath(guid)))
    //        .ToList();

    //    // Load icons for each category

    //    SdfIconType languageIcon = SdfIconType.TextareaT;
    //    SdfIconType audioIcon = SdfIconType.Soundwave;
    //    // Add LanguageHolder assets to the tree with icon
    //    foreach (var languageHolder in languageHolders)
    //    {
    //        OdinMenuItem menuItem = new OdinMenuItem(tree, languageHolder.name, languageHolder);
    //        menuItem.SdfIcon = languageIcon;
    //        tree.AddMenuItemAtPath("Language/", menuItem);
    //    }

    //    // Add AudioLocalization assets to the tree with icon
    //    foreach (var audioHolder in audiosHolders)
    //    {
    //        OdinMenuItem menuItem = new OdinMenuItem(tree, audioHolder.name, audioHolder); 
    //        menuItem.SdfIconColor = Color.yellow;
    //        menuItem.SdfIcon = audioIcon;
    //        tree.AddMenuItemAtPath("Audios/", menuItem);
    //    }

    //    tree.SortMenuItemsByName();
    //    return tree;
    //}

    //private static void CreateNewLanguageHolder()
    //{
    //    // Show the input dialog to get the name of the new LanguageHolder
    //    InputDialog.Show((languageName) =>
    //    {
    //        if (string.IsNullOrEmpty(languageName)) return;

    //        // Find the path of an existing LanguageHolder
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

    //        // Open the window and select the new asset
    //        var window = GetWindow<LocalizationWindow>();
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
    //        ScriptableObjectCreator.ShowDialog<LanguageHolder>("Assets/_Project/Localization/LanguagesSo", obj =>
    //        {
    //            base.TrySelectMenuItemWithObject(obj); // Selects the newly created item in the editor
    //        });
    //    }
    //    if (SirenixEditorGUI.ToolbarButton(new GUIContent("Create Audio")))
    //    {
    //        ScriptableObjectCreator.ShowDialog<AudioLocoliztion>("Assets/_Project/Localization/AudiosSo", obj =>
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
