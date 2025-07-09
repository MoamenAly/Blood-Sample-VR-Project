using BNG;
using UnityEngine;

public abstract class WebGL_MouseIteraction : MonoBehaviour, IMouseInteraction
{
    internal Grabbable grabbable;
    protected GrabbableUnityEvents grabbableUnityEvents;
    protected GrabbableItem grabbableItem;
    protected RestOnUnGrab restOnUnGrab;

    protected virtual void Awake()
    {
        restOnUnGrab = GetComponent<RestOnUnGrab>();
        grabbable = GetComponent<Grabbable>();
        grabbableUnityEvents = GetComponent<GrabbableUnityEvents>();
        grabbableItem = GetComponent<GrabbableItem>();
    }
    void Start()
    {
        restOnUnGrab.enabled = false;
    }

    public virtual void OnWebMouseDown()
    {
        if (grabbable == null || !grabbable.enabled) return;
        OnMouseInteract();
    }

    public abstract void OnMouseInteract();

    public abstract void OnRestInteract();


    internal virtual void FireGrabEvent()
    {
        if (grabbableUnityEvents != null)
        {
            grabbableUnityEvents.onGrab?.Invoke(null);
            grabbableItem.OnGrabEvent?.Invoke();
        }
    }



    internal virtual void FireRelaseEvent()
    {
        if (grabbableUnityEvents != null)
        {
            grabbableUnityEvents.onRelease?.Invoke();
            grabbableItem.OnRelaseEvent?.Invoke();
        }
    }

    public void OnWebMouseUp()
    {
      
    }
}