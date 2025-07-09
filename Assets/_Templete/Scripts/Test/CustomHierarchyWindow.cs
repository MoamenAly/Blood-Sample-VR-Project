#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CustomHierarchyWindow : EditorWindow
{
    [MenuItem("Window/Custom Hierarchy")]
    public static void ShowWindow()
    {
        GetWindow<CustomHierarchyWindow>("Custom Hierarchy");
    }

    void OnGUI()
    {
        // Here you will draw your custom hierarchy logic
        DrawHierarchy();
    }

    private void DrawHierarchy()
    {
        foreach (GameObject go in Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[])
        {
            if (go.hideFlags == HideFlags.None && string.IsNullOrEmpty(AssetDatabase.GetAssetPath(go)))
            {
                EditorGUILayout.LabelField(go.name);
            }
        }
    }
}

#endif