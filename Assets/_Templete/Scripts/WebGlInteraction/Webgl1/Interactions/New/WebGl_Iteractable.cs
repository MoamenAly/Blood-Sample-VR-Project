using DG.Tweening;
using System;
using UnityEngine;

public class WebGl_Iteractable : WebGl_Grabbable
{
    [SerializeField] float motionTime = 0.5f;  

    public override void OnMouseInteract()
    {
        SelectionManager.Select(this);
        transform.DOMoveY(transform.position.y + hoverDistance, hoverTime).OnComplete(() =>
        {
            OnRestInteract();
        });
    }
       

    public override void OnRestInteract()
    {
        transform.DOMove(restOnUnGrab.defaultPosition, motionTime).OnComplete(() => {
            FireGrabEvent();
            FireRelaseEvent();
            SelectionManager.SelectedGameObject = null;
        });
    }
}



