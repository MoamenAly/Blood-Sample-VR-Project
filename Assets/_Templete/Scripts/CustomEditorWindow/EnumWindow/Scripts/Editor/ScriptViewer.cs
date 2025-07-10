//using System;
//using System.IO;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;
//using Sirenix.OdinInspector;
//using Sirenix.Utilities.Editor;
//using System.Collections.Generic;
//using System.Text.RegularExpressions;

//public partial class EnumsWindow
//{
//    public class ScriptViewer
//    {
//        internal MonoScript Script { get; private set; }
//        private List<string> enumValues;
//        internal string enumName;

//        public ScriptViewer(MonoScript script)
//        {
//            Script = script;
//            enumValues = ParseEnumValues(Script.text);
//            enumName = script.name;
//        }
//        public ScriptViewer()
//        {

//        }

//        internal ScriptViewer Initiate(MonoScript script)
//        {
//            Script = script;
//            enumValues = ParseEnumValues(Script.text);
//            enumName = script.name;
//            return this;
//        }

//        [HorizontalGroup]
//        [ShowInInspector, HideLabel, Title("Script Content")]
//        [MultiLineProperty(50)]
//        public string ScriptContent => Script != null ? Script.text : string.Empty;

//        [HorizontalGroup]
//        [ValidateInput(nameof(AreValidEnumValues), "Enum values must be valid C# identifiers.")]
//        [ListDrawerSettings(CustomAddFunction = nameof(AddNewItem), OnTitleBarGUI = "DrawRefreshButton")]
//        [ShowInInspector, PropertyOrder(1), Title("Enum Values")]
//        public List<string> EnumValues
//        {
//            get => enumValues;
//            set
//            {
//                if (value != enumValues)
//                {
//                    enumValues = value;
//                }
//            }
//        }

//        private List<string> ParseEnumValues(string scriptText)
//        {
//            List<string> enumValues = new List<string>();


//            var enumDeclarationMatch = Regex.Match(scriptText, @"public enum \w+\s*{([^}]*)}", RegexOptions.Singleline);
//            if (enumDeclarationMatch.Success)
//            {

//                var valuesBlock = enumDeclarationMatch.Groups[1].Value;


//                enumValues = valuesBlock.Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
//                                        .Select(v => v.Trim())
//                                        .ToList();
//            }

//            return enumValues;
//        }

//        private void AddNewItem()
//        {

//            string defaultValue = "Item" + (EnumValues.Count);
//            EnumValues.Add(defaultValue);
//        }

//        private bool IsValidEnumName(string name)
//        {
//            return !string.IsNullOrEmpty(name) && System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(name);
//        }


//        private bool AreValidEnumValues(List<string> values)
//        {
//            foreach (var value in values)
//            {
//                if (string.IsNullOrEmpty(value) || !System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(value))
//                    return false;
//            }

//            return true;
//        }

//        internal void SaveEnum()
//        {
//            SaveEnum(enumName);
//        }


//        internal void SaveEnum(string enumName)
//        {
//            Debug.Log(enumName);
//            //_Project/Scripts/CustomEditorWindow/EnumWindow/EnumFolder
//            string filePath =
//                Application.dataPath + "/_Project/Scripts/CustomEditorWindow/EnumWindow/EnumFolder/" + enumName + ".cs";


//            if (File.Exists(filePath))
//            {
//                File.Delete(filePath);
//            }


//            try
//            {
//                using (StreamWriter writer = new StreamWriter(filePath))
//                {
//                    writer.WriteLine("public enum " + enumName);
//                    writer.WriteLine("{");
//                    foreach (string value in enumValues)
//                    {
//                        writer.WriteLine("\t" + value + ",");
//                    }
//                    writer.WriteLine("}");
//                }

//                AssetDatabase.Refresh();
//            }
//            catch (System.Exception ex)
//            {
//                Debug.LogError("Failed to create enum file: " + ex.Message);
//            }
//        }

//        private void DrawRefreshButton()
//        {
//            if (SirenixEditorGUI.ToolbarButton(SdfIconType.SaveFill))
//            {
//                SaveEnum();
//            }
//            if (SirenixEditorGUI.ToolbarButton(SdfIconType.Trash))
//            {
//                bool userConfirmed = EditorUtility.DisplayDialog(
//        "Confirm Clear",                                // Dialog title
//        "Are you sure you want to clear all enum values?", // Dialog message
//        "Yes",                                           // Yes button text
//        "No"                                             // No button text
//    );

//                // Check if the user clicked 'Yes'
//                if (userConfirmed)
//                {
//                    EnumValues.Clear();
//                    SaveEnum();
//                }
//            }
//        }
//    }
    
// }

