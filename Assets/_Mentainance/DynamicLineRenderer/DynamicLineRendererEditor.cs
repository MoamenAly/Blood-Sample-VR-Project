#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DynamicLineRenderer))]
public class DynamicLineRendererEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DynamicLineRenderer script = (DynamicLineRenderer)target;
        if (GUILayout.Button("Add Point"))
        {
            script.AddPoint();
        }

        if (GUILayout.Button("Update Line Renderer"))
        {
            script.UpdateLineRenderer();
        }
    }
}
#endif
