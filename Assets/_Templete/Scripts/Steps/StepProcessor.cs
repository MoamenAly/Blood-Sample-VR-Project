//#if UNITY_EDITOR
//using Sirenix.OdinInspector;
//using Sirenix.OdinInspector.Editor;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;

//public class StepProcessor : OdinAttributeProcessor<Step>
//{
//    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
//    {
//        // Check if parentProperty contains only one target
//        if (parentProperty.Tree.WeakTargets.Count == 1)
//        {
//            // Get the list of steps from the parent property
//            var stepList = parentProperty.Tree.WeakTargets[0] as List<Step>;
//            if (stepList != null)
//            {
//                for (int i = 0; i < stepList.Count; i++)
//                {
//                    var step = stepList[i];
//                    var groupName = GetGroupName(step, i);
//                    attributes.Add(new FoldoutGroupAttribute(groupName));
//                }
//                return;
//            }
//        }

//        // Get the current step and its index
//        var index = parentProperty.Index;
//        var currentStep = parentProperty.ValueEntry.WeakSmartValue as Step;
//        var groupNameForStep = GetGroupName(currentStep, index);

//        // Add foldout group attribute for the current step
//        attributes.Add(new FoldoutGroupAttribute(groupNameForStep));
//    }

//    private string GetGroupName(Step step, int index)
//    {
//        var groupNameBuilder = new StringBuilder();
//        groupNameBuilder.Append("Step ").Append(index);

//        if (step != null && !string.IsNullOrEmpty(step.stepDescription))
//        {
//            var firstLine = step.stepDescription.Split(new[] { '\n' }, StringSplitOptions.None)[0];
//            groupNameBuilder.Append(" -> ").Append(firstLine);
//        }

//        return groupNameBuilder.ToString();
//    }
//}
//#endif
