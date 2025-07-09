using UnityEditor;
using UnityEngine;

public class SaveMotionWindow : EditorWindow
{
    private string motionName;
    private System.Action<string> onSave;

    public static void Init(System.Action<string> onSaveCallback)
    {
        SaveMotionWindow window = GetWindow<SaveMotionWindow>("Save Motion");
        window.onSave = onSaveCallback;
        window.ShowUtility();
    }

    private void OnGUI()
    {
        GUILayout.Label("Enter Motion Name:", EditorStyles.boldLabel);
        motionName = EditorGUILayout.TextField("Motion Name", motionName);

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save", GUILayout.Width(100)))
        {
            onSave?.Invoke(motionName);
            Close();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
    }
}
