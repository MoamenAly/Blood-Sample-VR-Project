using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
public class WebGlGrab : MonoBehaviour
{
    private GrabbableUnityEvents grabbableUnityEvents;
    private GrabbableItem grabbableItem;


    private void Awake()
    {
        grabbableUnityEvents = GetComponent<GrabbableUnityEvents>();
        grabbableItem = GetComponent<GrabbableItem>();
    }

    public void FireGrabAction()
    {
        if (grabbableUnityEvents != null)
        {
            grabbableUnityEvents?.onGrab?.Invoke(null);
            grabbableItem?.OnGrabEvent?.Invoke();
        }
    }
}
