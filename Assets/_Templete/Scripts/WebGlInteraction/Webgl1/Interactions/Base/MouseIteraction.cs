using BNG;
using UnityEngine;

public abstract class MouseIteraction : MonoBehaviour, IMouseInteraction
{
    [SerializeField] protected LayerMask interactionLayer;
    public abstract void OnMouseInteract();
    protected Grabbable grabbable;
    protected GrabbableUnityEvents grabbableUnityEvents;

    void Start()
    {
        GetComponent<RestOnUnGrab>().enabled = false;
        grabbable = GetComponent<Grabbable>();
        grabbableUnityEvents = GetComponent<GrabbableUnityEvents>();
    }

    //protected virtual void Update()
    //{
    //    if (grabbable == null || !grabbable.enabled) return;

    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hitInfo;

    //        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, interactionLayer))
    //        {
    //            var clickedObject = hitInfo.collider.gameObject;
    //            if (clickedObject == this.gameObject)
    //            {
    //                OnMouseInteract();
    //            }
    //        }
    //    }
    //}

    protected virtual void FireGrab()
    {
        if (grabbableUnityEvents != null)
            grabbableUnityEvents.onGrab?.Invoke(null);
    }

    protected virtual void FireRelase()
    {
        if (grabbableUnityEvents != null)
            grabbableUnityEvents.onRelease?.Invoke();
    }

    public void OnWebMouseDown()
    {
        OnMouseInteract();
    }

    public void OnWebMouseUp()
    {
       
    }
}