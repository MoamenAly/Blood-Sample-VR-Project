using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateFoldersWindow : OdinEditorWindow
{
    [Title("Project Folder Setup")]
    [InfoBox("Enter the project name and click Generate to create default folders.")]
    [PropertyTooltip("The name of the project. Folders will be created under Assets/ with this root name.")]
    public string projectName = "PROJECT_NAME";

    [FolderPath(RequireExistingPath = true)]
    [PropertyTooltip("Base path under which the project folders will be created.")]
    public string basePath = "Assets";

    [Title("Folder Structure")]
    public List<FolderStructure> folderStructures = new List<FolderStructure>
    {
        new FolderStructure { folderName = "Animations" },
        new FolderStructure { folderName = "Audio" },
        new FolderStructure { folderName = "Editor" },
        new FolderStructure { folderName = "Materials" },
        new FolderStructure { folderName = "Meshes" },
        new FolderStructure { folderName = "Prefabs" },
        new FolderStructure { folderName = "Scripts" },
        new FolderStructure { folderName = "Scenes" },
        new FolderStructure { folderName = "Shaders" },
        new FolderStructure { folderName = "Textures" },
        
        new FolderStructure { folderName = "UI", subfolders = new List<string> { "Assets", "Fonts", "Icons" } },
    };

    [Button(ButtonSizes.Large)]
    [GUIColor(0, 1, 0)]
    public void GenerateFolders()
    {
        CreateFolders();
        Debug.Log("Folders created successfully!");
    }

    private void CreateFolders()
    {
        string projectFolderPath = Path.Combine(basePath, projectName);
        Debug.Log($"Creating project folder at: {projectFolderPath}");
        Directory.CreateDirectory(projectFolderPath);  // This will create the directory if it does not exist

        foreach (var structure in folderStructures)
        {
            string mainFolderPath = Path.Combine(projectFolderPath, structure.folderName);
            Debug.Log($"Creating main folder at: {mainFolderPath}");
            Directory.CreateDirectory(mainFolderPath);  // No need to check if exists, CreateDirectory checks this

            foreach (var subfolder in structure.subfolders)
            {
                string subfolderPath = Path.Combine(mainFolderPath, subfolder);
                Debug.Log($"Creating subfolder at: {subfolderPath}");
                Directory.CreateDirectory(subfolderPath);  // Creates subfolder
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("Scivr/Window/Create Default Folders")]
    private static void OpenWindow()
    {
        GetWindow<CreateFoldersWindow>().Show();
    }
}

[System.Serializable]
public class FolderStructure
{
    public string folderName = "New Folder";
    [Space]
    public List<string> subfolders = new List<string>();
}
