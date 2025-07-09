#if UNITY_EDITOR
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System;
using System.Collections.Generic;
using System.Reflection;

//public class CollisionSequenceDataProcessor : OdinAttributeProcessor<ColliosionSequenceData>
//{
//    public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
//    {
//        var index = parentProperty.Index;
//        var groupName = $"Collision {index}";
//        attributes.Add(new FoldoutGroupAttribute(groupName));
//    }
//}
namespace Scivr.CollisionSequence
{
    public class CollisionSequenceDataProcessor : OdinAttributeProcessor<CollisionSequenceData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var index = parentProperty.Index;
            var groupName = $"Collision {index}";
            attributes.Add(new FoldoutGroupAttribute(groupName));
        }
    }
}
#endif

