using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]

public class ColliosionSequenceData {


    [FoldoutGroup("$CollisionName")]
    [TextArea(3,10)] public string _descriptions;
    [FoldoutGroup("$CollisionName")]

    [Space] public Elements _targetElementId;
    [FoldoutGroup("$CollisionName")]

    [Space] public CollisonType collisonType;
    [FoldoutGroup("$CollisionName")]

    [Space] public UnityEvent2 OnStart;
    [FoldoutGroup("$CollisionName")]

    [Space] public UnityEvent2 unityEvent;
    [FoldoutGroup("$CollisionName")]

    [Space] public UnityEvent2 OnUpdate;

    [FoldoutGroup("$CollisionName")]

    [ShowIf(nameof(IsTriggerStayOrStayWithMoveOrStayWithRoation))]
    public float timeSpentInTrigger = 1f;

    [FoldoutGroup("$CollisionName")]

    [ShowIf(nameof(collisonType), CollisonType.OnTriggerStayWithMove)]

    [FoldoutGroup("$CollisionName")]

    public float moveMinvalue = 1f;

    [FoldoutGroup("$CollisionName")]

    [ShowIf(nameof(collisonType), CollisonType.OnTriggerStayWithRotaion)]

    public float rotationMinvalue = 1f;

    [FoldoutGroup("$CollisionName")]

    [ShowIf(nameof(IsStayWithMoveOrStayWithRotate))]
    public bool showGizmos = false;

    internal string CollisionName = "Collision";

    internal bool IsTriggerStayOrStayWithMoveOrStayWithRoation()
    {
        return (collisonType & (CollisonType.OnTriggerStay | CollisonType.OnTriggerStayWithMove| CollisonType.OnTriggerStayWithRotaion)) != 0;
    }

  
    internal bool IsStayWithMoveOrStayWithRotate()
    {
        return (collisonType & (CollisonType.OnTriggerStayWithMove | CollisonType.OnTriggerStayWithRotaion)) != 0;
    }


}


