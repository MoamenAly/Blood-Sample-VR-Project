using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

public partial class EnumsWindow
{
    public class EnumViewer
    {
        internal Type EnumType { get; private set; }
        private List<EnumEntry> enumEntries;
        internal string EnumName;
        private static bool isScriptHidden;
        private Dictionary<string, int> enumNameToValueMap;
        private Array enumValuesArray;

        public EnumViewer(Type enumType)
        {
            Initialize(enumType);
        }

        internal EnumViewer Initialize(Type enumType)
        {
            EnumType = enumType;
            enumValuesArray = Enum.GetValues(EnumType);
            var enumFields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);

            enumEntries = enumFields
                .Select(field => new EnumEntry
                {
                    previousName = field.Name,
                    displayName = field.Name
                })
                .ToList();

            UpdateEnumEntries();
            EnumName = enumType.Name;
            EnumScript = EnumType != null ? GenerateEnumScript(EnumName) : string.Empty;
            return this;
        }

        private void UpdateEnumEntries()
        {
            enumNameToValueMap = Enum.GetNames(EnumType)
                .ToDictionary(name => name, name => (int)Enum.Parse(EnumType, name));

            // Ensure EnumEntries keeps the order from the enum file
            EnumEntries = enumEntries
                .Select(entry => new EnumEntry
                {
                    previousName = entry.previousName,
                    displayName = entry.displayName
                })
                .ToList();
        }

        [HorizontalGroup]
        [ShowInInspector, ReadOnly, HideLabel, Title("Enum Script")]
        [DynamicTextArea, ShowIf(nameof(isScriptHidden))]
        public string EnumScript;

        [Space, InlineButton(nameof(SaveEnum), SdfIconType.SaveFill, ""), InlineButton(nameof(AddEntry), SdfIconType.Plus, "")]
        [PropertyOrder(0)]
        public string NewItem;

        [HorizontalGroup]
        [ValidateInput(nameof(ValidateEnumEntries), "Enum values must be valid C# identifiers.")]
        [ListDrawerSettings(ShowPaging = false, CustomAddFunction = nameof(AddDefaultEntry), OnTitleBarGUI = nameof(RenderToolbar))]
        [ShowInInspector, Searchable, PropertyOrder(1), Title("Enum Entries")]
        public List<EnumEntry> EnumEntries = new List<EnumEntry>();

        private void AddDefaultEntry()
        {
            NewItem = TrimEnumString(NewItem);
            // Use the input from the text field if not empty, otherwise fall back to the default naming convention
            string entryName = string.IsNullOrWhiteSpace(NewItem) ? "Item" + EnumEntries.Count : NewItem;
            EnumEntries.Add(new EnumEntry { displayName = entryName, previousName = entryName });
            NewItem = string.Empty; // Reset the input field
        }

        private void AddEntry()
        {
            NewItem = TrimEnumString(NewItem);
            string entryName = string.IsNullOrWhiteSpace(NewItem) ? "Item" + EnumEntries.Count : NewItem;
            EnumEntries.Add(new EnumEntry { displayName = entryName, previousName = entryName });
            //add = string.Empty;
        }

        private string TrimEnumString(string txt)
        {
            return txt.Replace("'", "")
                     .Trim()
                     .Replace(":", "")
                     .Replace(" ", "_")
                     .Replace("-", "_");
        }

        private bool ValidateEnumEntries(List<EnumEntry> entries)
        {
            foreach (var entry in entries)
            {
                entry.displayName = TrimEnumString(entry.displayName);
            }
            return entries.All(entry => !string.IsNullOrEmpty(entry.displayName) && System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(entry.displayName));
        }

        internal void SaveEnum()
        {
            UpdateEnumEntriesBeforeSave();
            SaveEnumToFile(EnumName);
        }

        internal void DeleteEnum(string enumName)
        {
            string filePath = Path.Combine(Application.dataPath, "_Templete/Scripts/CustomEditorWindow/EnumWindow/EnumFolder", $"{enumName}.cs");
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                AssetDatabase.Refresh();
            }
        }

        internal void SaveEnumToFile(string enumName)
        {
            string filePath = Path.Combine(Application.dataPath, "_Templete/Scripts/CustomEditorWindow/EnumWindow/EnumFolder", $"{enumName}.cs");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    string scriptText = GenerateEnumScript(enumName);
                    writer.Write(scriptText);
                }
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create enum file: {ex.Message}");
            }
        }

        internal void UpdateEnumEntriesBeforeSave()
        {
            var duplicateGroups = EnumEntries
                .GroupBy(entry => entry.previousName)
                .Where(group => group.Count() > 1);

            foreach (var group in duplicateGroups)
            {
                var entries = group.ToList();
                var originalEntry = entries.First();
                bool hasNewValues = entries.Any(entry => entry.displayName != originalEntry.previousName);

                if (hasNewValues)
                {
                    for (int i = 1; i < entries.Count; i++)
                    {
                        entries[i].previousName = entries[i].displayName;
                    }
                }
                else
                {
                    for (int i = 1; i < entries.Count; i++)
                    {
                        entries[i].previousName = originalEntry.displayName;
                    }
                }
            }
        }
        public string GenerateEnumScript(string enumName)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[EnumAttribute]");
            builder.AppendLine("[System.Serializable]");
            builder.AppendLine($"public enum {enumName}");
            builder.AppendLine("{");

            // Fix: Sort entries by their existing enum values if available, otherwise add new entries at the end
            int maxIndex = enumNameToValueMap.Values.Count > 0 ? enumNameToValueMap.Values.Max() : 0;
            int currentMaxIndex = maxIndex + 1;

            foreach (var entry in EnumEntries)
            {
                if (!enumNameToValueMap.TryGetValue(entry.previousName, out var index))
                {
                    index = currentMaxIndex++;
                    enumNameToValueMap[entry.previousName] = index;
                }
                builder.AppendLine($"\t{entry.displayName} = {index},");
            }

            builder.AppendLine("}");
            return builder.ToString();
        }

        private void RenderToolbar()
        {
            // Render the text field for the new item name and handle Enter key press
            GUI.SetNextControlName("AddTextField");
            NewItem = EditorGUILayout.TextField(NewItem, GUILayout.Width(150));
            Event e = Event.current;
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "AddTextField")
            {
                AddEntry();
                GUI.FocusControl(null); // Unfocus the text field to avoid continuous adding
                e.Use(); // Consume the event
            }

            // Render the save button
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.SaveFill))
            {
                SaveEnum();
            }

            // Render the clear button
            if (SirenixEditorGUI.ToolbarButton(SdfIconType.Trash))
            {
                bool userConfirmed = EditorUtility.DisplayDialog(
                    "Confirm Clear",
                    "Are you sure you want to clear all enum values?",
                    "Yes",
                    "No");

                if (userConfirmed)
                {
                    EnumEntries.Clear();
                    SaveEnum();
                }
            }
        }

        internal void ToggleScriptVisibility()
        {
            isScriptHidden = !isScriptHidden;
        }

        // **Added `OnGUI` Method**:
        public void OnGUI()
        {
            // Ensure the New Item bar stays at the bottom
            GUILayout.BeginVertical();

            // Render the list of enum entries
            foreach (var entry in EnumEntries)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(entry.displayName);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    EnumEntries.Remove(entry);
                    break;
                }
                GUILayout.EndHorizontal();
            }

            // **Added `GUILayout.FlexibleSpace()`** to push the New Item input field to the bottom
            GUILayout.FlexibleSpace();

            // Render the New Item input field and Add button
            GUILayout.BeginHorizontal();
            NewItem = GUILayout.TextField(NewItem, GUILayout.Width(150));
            if (GUILayout.Button("Add", GUILayout.Width(50)))
            {
                AddEntry();
            }
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
        }
    }
}

[Serializable]
public class EnumEntry
{
    [HideLabel] public string displayName;
    [HideInInspector,ReadOnly] public string previousName;
}
