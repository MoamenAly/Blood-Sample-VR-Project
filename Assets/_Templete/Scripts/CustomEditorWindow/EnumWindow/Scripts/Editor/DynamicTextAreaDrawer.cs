using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;

public class DynamicTextAreaAttributeDrawer : OdinAttributeDrawer<DynamicTextAreaAttribute, string>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        string text = ValueEntry.SmartValue ?? "";

        // Ensure the text area wraps text when it exceeds the width
        GUIStyle textAreaStyle = new GUIStyle(GUI.skin.textArea)
        {
            wordWrap = true
        };

        // Calculate the height of the text area based on the content
        float height = GetTextAreaHeight(text, textAreaStyle);

        // Draw the text area with the custom style and dynamic height
        string newText = EditorGUILayout.TextArea(text, textAreaStyle, GUILayout.Height(height));

        if (newText != ValueEntry.SmartValue)
        {
            ValueEntry.SmartValue = newText;
        }

        //if (string.IsNullOrEmpty(text))
        //{
        //    EditorGUILayout.LabelField("Enter text below:");
        //}
    }

    private float GetTextAreaHeight(string text, GUIStyle style)
    {
        // Calculate the width of the text area taking into account padding and inspector window width
        float contentWidth = EditorGUIUtility.currentViewWidth - 30; // Adjust 30 for inspector padding
        GUIContent content = new GUIContent(text);

        // Calculate height based on wrapped content
        float height = style.CalcHeight(content, contentWidth);

        return Mathf.Max(height, EditorGUIUtility.singleLineHeight * 3); // Minimum height
    }
}
