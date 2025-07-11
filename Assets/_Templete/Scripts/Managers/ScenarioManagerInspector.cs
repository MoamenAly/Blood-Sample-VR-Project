#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(StepsManager))]

public class ScenarioManagerInspector : OdinEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Ensure the base class draws the default inspector
        base.OnInspectorGUI();

        // Force the inspector to update
        //if (GUI.changed)
        //{
        //    foreach (var target in targets)
        //    {
        //        EditorUtility.SetDirty(target);
        //    }

        //    // Force the inspector to repaint
        //   // UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
        //}
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            Repaint();
        }
        serializedObject.ApplyModifiedProperties();

    }
}
#endif
