using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleMouseIntraction : WebGL_MouseIteraction
{
    [SerializeField] UnityEvent onComplete;

    public override void OnMouseInteract()
    {
        if (grabbable == null || !grabbable.enabled) return;

        FireGrabEvent();
        onComplete?.Invoke();
        FireRelaseEvent();
        grabbable.enabled = false;

    }

    public override void OnRestInteract()
    {
        throw new System.NotImplementedException();
    }
}
