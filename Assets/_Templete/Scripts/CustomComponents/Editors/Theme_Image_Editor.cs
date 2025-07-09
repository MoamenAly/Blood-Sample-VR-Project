#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[CustomEditor(typeof(Theme_Image))]
public class Theme_Image_Editor : UnityEditor.UI.ImageEditor
{
    SerializedProperty customProperty;

    protected override void OnEnable()
    {
        customProperty = serializedObject.FindProperty("ThemeType");
        base.OnEnable();
        // Initialize other custom properties here...
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(customProperty);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space(); // Add some space between default inspector and custom properties

        base.OnInspectorGUI();

      
        // Draw other custom properties here...

    }
}
#endif