#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEngine;
using UnityEditor;

public class DynamicTextAreaDrawer : OdinAttributeDrawer<DynamicTextAreaAttribute, string>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {
        string text = ValueEntry.SmartValue ?? "";

        float height = Mathf.Max(GetTextAreaHeight(text), EditorGUIUtility.singleLineHeight * 3); 

        string newText = EditorGUILayout.TextArea(text, GUILayout.Height(height));


        if (newText != ValueEntry.SmartValue)
        {
            ValueEntry.SmartValue = newText;
        }


        if (string.IsNullOrEmpty(text))
        {
            EditorGUILayout.LabelField("Enter text below:");
        }
    }

    private float GetTextAreaHeight(string text)
    {
        GUIStyle style = GUI.skin.textArea;
        GUIContent content = new GUIContent(text);
        return style.CalcHeight(content, EditorGUIUtility.currentViewWidth - 20);
    }
}
#endif
