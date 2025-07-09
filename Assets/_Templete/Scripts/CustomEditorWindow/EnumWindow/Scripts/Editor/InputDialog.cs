using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InputDialog : EditorWindow
{
    public string fileName = "";
    public Action<string> OnSubmit;

    private void OnGUI()
    {
        GUILayout.Label("Enter Name:", EditorStyles.boldLabel);
        fileName = EditorGUILayout.TextField(fileName);

        if (GUILayout.Button("Create"))
        {
            if (OnSubmit != null)
            {
                OnSubmit(fileName);
            }
            this.Close();
        }
    }

    private void OnLostFocus()
    {
        this.Close();
    }

    public static void Show(Action<string> onSubmit)
    {
        InputDialog window = ScriptableObject.CreateInstance<InputDialog>();
        float windowWidth = 250;
        float windowHeight = 80;
        float x = (Screen.currentResolution.width - windowWidth) * 0.5f;
        float y = (Screen.currentResolution.height - windowHeight) * 0.5f;
        window.position = new Rect(x, y, windowWidth, windowHeight);
        window.OnSubmit = onSubmit;
        window.ShowPopup();
    }

    public static string Show(string title, string message, string defaultValue)
    {
        string result = defaultValue;
        EditorGUIUtility.editingTextField = false;

        bool confirmed = EditorUtility.DisplayDialog(title, message, "OK", "Cancel");

        if (confirmed)
        {
            result = EditorGUILayout.TextField("Enter name:", result);
        }

        return result;
    }
}

