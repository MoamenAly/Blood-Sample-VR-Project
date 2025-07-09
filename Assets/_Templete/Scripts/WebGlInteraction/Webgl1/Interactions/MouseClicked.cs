using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseClicked : MonoBehaviour, IMouseInteraction
{
    public UnityEvent2 OnClicked;
    public UnityEvent2 OnClickedUp;

    public virtual void OnWebMouseDown()
    {
        OnMouseInteract();
    }    

    public virtual void OnMouseInteract()
    {
        OnClicked?.Invoke();
    }

    public void OnWebMouseUp()
    {
        OnClickedUp?.Invoke();
    }
}
