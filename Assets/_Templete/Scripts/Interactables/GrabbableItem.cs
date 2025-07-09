using BNG;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GrabbableItem : Item , IIteractableItem
{
    CustomGrabbable grabbable;

    [SerializeField]internal UnityEvent2 OnGrabEvent;
    [SerializeField] internal UnityEvent2 OnRelaseEvent;

    [SerializeField] bool isIntractionEnabled = true;



    private void Awake()
    {
        grabbable = GetComponent<CustomGrabbable>();
        grabbable.enabled = isIntractionEnabled;
    }

    private void OnEnable()
    {
        if (grabbable != null)
        {
            grabbable.OnGrabAction+=OnGrab; 
            grabbable.OnGrabAction+=OnRelease; 
        }
    }

    private void OnRelease(Grabber grabber)
    {
        OnGrabEvent?.Invoke();
    }

    private void OnGrab(Grabber grabber)
    {
        OnRelaseEvent?.Invoke();
    }

    public void _DisableInteraction()
    {
        isIntractionEnabled  = false;
        grabbable.enabled = isIntractionEnabled;
    }

    public void _EnableInteraction()
    {
        isIntractionEnabled = true;
        grabbable.enabled = isIntractionEnabled;
    }

    public bool _IsIntractiobEnabled()
    {
        return grabbable.enabled;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (grabbable == null)
        {
            grabbable = GetComponent<CustomGrabbable>();
        }       
    }


}
