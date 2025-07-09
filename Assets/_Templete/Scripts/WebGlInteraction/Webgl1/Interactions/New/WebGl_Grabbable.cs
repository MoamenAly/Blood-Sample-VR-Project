using BNG;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class WebGl_Grabbable : WebGL_MouseIteraction
{
    [SerializeField]  internal float hoverDistance  = 0.15f;    
    [SerializeField]  internal float hoverTime  = 0.25f;
    Grabbable _grabbable;

    protected virtual void Start()
    {
        bool web = PlatformManager.Instance.platform == Platform.Webgl;
        if (web)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        _grabbable = GetComponent<Grabbable>();
    }

    public override void OnMouseInteract()
    {
        if (!enabled)
        {
           var interactions =  GetComponentsInChildren<IMouseInteraction>();
            for (int i=0;i<interactions.Length;i++)
            {
                if (interactions[i] != this as IMouseInteraction) { 
                    interactions[i].OnWebMouseDown();
                }
            }
          
            return;
        }

        if (SelectionManager.SelectedGameObject != null
            && SelectionManager.SelectedGameObject.gameObject == this.gameObject)
            return;

        if(!_grabbable.enabled)        
            return;
        

        SelectionManager.Select(this);
        transform.DOMoveY(transform.position.y + hoverDistance, hoverTime).OnComplete(() => { 
           FireGrabEvent();        
        });
    }

    public override void OnRestInteract()
    {
        transform.DOMoveY(transform.position.y - hoverDistance, hoverTime);
    }
}

public class SelectionManager {
    public static WebGL_MouseIteraction SelectedGameObject;
    public static void Select(WebGL_MouseIteraction selectedGameObject) {
        RestSelectedObject();
        SelectedGameObject = selectedGameObject;
    }

    internal static void RestSelectedObject()
    {
        if (SelectedGameObject == null) return;
        SelectedGameObject.OnRestInteract();
    }
}
