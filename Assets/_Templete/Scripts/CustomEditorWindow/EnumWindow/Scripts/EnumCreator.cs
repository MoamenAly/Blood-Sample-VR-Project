#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor; 

public class EnumCreator : MonoBehaviour
{
    [SerializeField]
    [ValidateInput(nameof(IsValidEnumName), "Enum name must be a valid C# identifier and cannot be empty.")]
    public string enumName = "DynamicEnum";

    [SerializeField]
    [ListDrawerSettings(AddCopiesLastElement = true)]
    [ValidateInput(nameof(AreValidEnumValues), "Enum values must be valid C# identifiers.")]
    public List<string> enumValues = new List<string>();

    // Validation for enum name
    private bool IsValidEnumName(string name)
    {
        return !string.IsNullOrEmpty(name); //&& System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);
    }

    // Validation for enum values
    private bool AreValidEnumValues(List<string> values)
    {
        foreach (var value in values)
        {
            if (string.IsNullOrEmpty(value)) //|| !System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(value))
                return false;
        }
        return true;
    }

    [Button("Create Enum", ButtonSizes.Large)]
    public void CreateEnum()
    {
        string filePath = Application.dataPath + "/_Project/Scripts/CustomEditorWindow/EnumWindow/EnumFolder/" + enumName + ".cs";

        // Check if the file already exists
        if (File.Exists(filePath))
        {
            if (EditorUtility.DisplayDialog("Enum File Exists",
                "The enum file already 0. Do you want to overwrite it?", "Yes", "No"))
            {
                File.Delete(filePath);
                Debug.Log("Existing enum file deleted: " + filePath);
            }
            else
            {
                Debug.Log("Enum creation aborted.");
                return;
            }
        }

        // Try to create a new enum file
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                //writer.WriteLine("// This enum is auto-generated. Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.");
                writer.WriteLine("public enum " + enumName);
                writer.WriteLine("{");
                foreach (string value in enumValues)
                {
                    writer.WriteLine("\t" + value + ",");
                }
                writer.WriteLine("}");
            }
            Debug.Log("Enum file created: " + filePath);
            AssetDatabase.Refresh(); // Refresh the Asset Database to include the new file
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to create enum file: " + ex.Message);
        }
    }
}

#endif