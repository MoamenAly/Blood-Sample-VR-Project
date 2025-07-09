using BNG;
using Sirenix.OdinInspector;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ChildGrabbable : MonoBehaviour
{
    [SerializeField] bool DrawGizmos = true;
    [SerializeField] CustomGrabbable parent;
    [SerializeField] private float snapToParentValue = 0.1f;

    [SerializeField] bool isUseCustomPosition;

    [ShowIf(nameof(isUseCustomPosition))]
    [SerializeField] Vector3 customPosition;

    CustomGrabbable child;
    Rigidbody childPhyscis;
    Vector3 defultLocalPosition;
    Quaternion defultLoaclRotaion;
    private bool isAttachedToPrent;

    public bool AttachedToPrent { get => isAttachedToPrent; private set => isAttachedToPrent = value; }

    private void Awake()
    {
        child = GetComponent<CustomGrabbable>();
        childPhyscis = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        child.OnReleaseAction += OnChildRelase;
    }


    private void OnDisable()
    {
        child.OnReleaseAction -= OnChildRelase;
    }

    private void Start()
    {
        defultLocalPosition = transform.localPosition;
        defultLoaclRotaion = transform.localRotation;
        MakeItChild();
    }

    [Button("Test Grab")]
    private void TestGrab()
    {
        childPhyscis.isKinematic = !childPhyscis.isKinematic;
    }


    [Button("Test Relase")]
    private void OnChildRelase()
    {
        if (!child.enabled) return;

        float distanceToDefalPosition = Vector3.Distance(parent.transform.TransformPoint(defultLocalPosition), transform.position);
        if (distanceToDefalPosition < snapToParentValue)
        {
            //make it child
            MakeItChild();
        }
        else
        {
            MakeItFree();
        }
    }


    private void MakeItFree()
    {
        childPhyscis.isKinematic = false;
        childPhyscis.transform.parent = null;
        // child.GrabPhysics = GrabPhysics.FixedJoint;
    }

    private void MakeItChild()
    {
        transform.parent = parent.transform;
        if (isUseCustomPosition)
        {
            transform.localPosition = customPosition;
        }
        else
        {
            transform.localPosition = defultLocalPosition;
        }

        transform.localRotation = defultLoaclRotaion;

        //child.GrabPhysics = GrabPhysics.Kinematic;
        childPhyscis.isKinematic = true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!DrawGizmos) return;
        float distanceToDefalPosition = Vector3.Distance(parent.transform.TransformPoint(defultLocalPosition), transform.position);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * distanceToDefalPosition);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * snapToParentValue);
        Handles.Label(transform.position, "snap value");
    }
#endif
}


public enum PhycisState
{
    kinmetic, NonKinemtic
}