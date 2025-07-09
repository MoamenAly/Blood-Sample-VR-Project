using BNG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseSnapToSnapZone : MouseIteraction
{
    [SerializeField] SnapZone snapZone;
    [SerializeField] UnityEvent OnSnap;

    public override void OnMouseInteract()
    {
        Debug.Log("Clicked on object on the grabbable layer: " + grabbable.name);
        if (snapZone.OnlyAllowNames.Contains(grabbable.name))
        {
            MotionController motionController = grabbable.GetComponent<MotionController>();
            motionController.StartMotion(OnMotionDone);
            FireGrab();
        }
    }

    private void OnMotionDone()
    {
        grabbable.GetComponent<GrabbableUnityEvents>().onGrab?.Invoke(null);
        snapZone.OnSnapEvent?.Invoke(grabbable);
        OnSnap?.Invoke();
        FireRelase();
        grabbable.enabled = false;
    }


}
