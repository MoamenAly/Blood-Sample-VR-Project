#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;
using UnityEngine;

[OdinDrawer]
public class StepGroupDrawer : OdinValueDrawer<Step>
{
    private bool isExpanded;

    protected override void DrawPropertyLayout(GUIContent label)
    {
        Step step = this.ValueEntry.SmartValue;

        // Create the foldout group dynamically based on the step description
        string groupName = GetGroupName(step, this.Property.Index);
        SirenixEditorGUI.BeginBox();
        // Draw the foldout header
        isExpanded = SirenixEditorGUI.Foldout(isExpanded, new GUIContent(groupName));

        if (isExpanded)
        {
            SirenixEditorGUI.BeginBox();
            {
                this.CallNextDrawer(label);

            }
            SirenixEditorGUI.EndBox();
        }
        SirenixEditorGUI.EndBox();
    }

    private string GetGroupName(Step step, int index)
    {
        string groupName = $"Step {index}";
        if (!string.IsNullOrEmpty(step.stepDescription))
        {
            string firstLine = step.stepDescription.Split(new[] { '\n' }, StringSplitOptions.None)[0];
            groupName += $" -> {firstLine}";
        }
        return groupName;
    }
}
#endif
