#if UNITY_EDITOR
using RTLTMPro;

using UnityEditor;

[CustomEditor(typeof(Theme_Text))]

public class Theme_Text_Editor : RTLTextMeshProEditor//TMPro.EditorUtilities.TMP_EditorPanelUI
{
    SerializedProperty customProperty;


    SerializedProperty TextIdProperty;

    protected override void OnEnable()
    {
        customProperty = serializedObject.FindProperty("ThemeType");
        TextIdProperty = serializedObject.FindProperty("TextId");
        base.OnEnable();
        // Initialize other custom properties here...
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(customProperty);
        EditorGUILayout.PropertyField(TextIdProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space(); // Add some space between default inspector and custom properties

        base.OnInspectorGUI();


        // Draw other custom properties here...

    }
}
#endif