using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Scivr.CollisionSequence
{
    [System.Serializable]
    public class CollisionSequenceData
    {
        [TextArea(3, 10)] public string Descriptions;
        [Space] public Elements TargetElementId;
        [Space] public CollisionType CollisonType;    

        [ShowIf(nameof(IsTriggerStayOrStayWithMoveOrStayWithRotation))]
        public float TimeSpentInTrigger = 1f;

        [ShowIf(nameof(CollisonType), CollisionType.OnTriggerStayWithMove)]
        public float moveMinValue = 1f;

        [ShowIf(nameof(CollisonType), CollisionType.OnTriggerStayWithRotaion)]
        public float RotationMinValue = 1f;

        [ShowIf(nameof(IsStayWithMoveOrStayWithRotate))]
        [Range(1, 1000)] public int samplingRate = 100;

        [ShowIf(nameof(IsStayWithMoveOrStayWithRotate))]
        public bool ShowGizmos = false;

        [Space] public UnityEvent2 OnCollsionStart;
        [Space] public UnityEvent2 OnCollisonUpdate;
        [Space] public UnityEvent2 OnCollsionDone;

        public float MoveMinValue { get => Mathf.Abs(moveMinValue); }

        internal bool IsTriggerStayOrStayWithMoveOrStayWithRotation()
        {
            return CollisonType == CollisionType.OnTriggerStay ||
                   CollisonType == CollisionType.OnTriggerStayWithMove ||
                   CollisonType == CollisionType.OnTriggerStayWithRotaion;
        }

        internal bool IsStayWithMoveOrStayWithRotate()
        {
            return CollisonType == CollisionType.OnTriggerStayWithMove ||
                   CollisonType == CollisionType.OnTriggerStayWithRotaion;
        }

        internal bool IsMove()
        {
            return CollisonType == CollisionType.OnTriggerStayWithMove;
        }

        internal bool IsRotation()
        {
            return CollisonType == CollisionType.OnTriggerStayWithRotaion;
        }
    }
}
