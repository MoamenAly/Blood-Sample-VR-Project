using BNG;
using UnityEngine;
using UnityEngine.Events;

public class MouseSnapToSnapZoneV2 : WebGL_MouseIteraction
{
    [SerializeField] SnapZone snapZone;
    [SerializeField] UnityEvent OnSnap;

    public override void OnMouseInteract()
    {
        if (snapZone.OnlyAllowNames.Contains(grabbable.name))
        {
            MotionController motionController = grabbable.GetComponent<MotionController>();
            motionController.StartMotion(OnMotionDone);
            FireGrabEvent();
        }
    }

    public override void OnRestInteract()
    {
        throw new System.NotImplementedException();
    }

    private void OnMotionDone()
    {
        grabbable.GetComponent<GrabbableUnityEvents>().onGrab?.Invoke(null);
        snapZone.OnSnapEvent?.Invoke(grabbable);
        OnSnap?.Invoke();
        FireRelaseEvent();
        grabbable.enabled = false;
    }


}
