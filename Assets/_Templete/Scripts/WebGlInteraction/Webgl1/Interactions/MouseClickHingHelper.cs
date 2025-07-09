using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseClickHingHelper : MonoBehaviour
{
    public LayerMask grabbableLayer;

    public UnityEvent OnClicked;

    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            // Check if the ray hits an object on the grabbable layer
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, grabbableLayer))
            {
                // Perform actions on the grabbed object
                GameObject grabbedObject = hitInfo.collider.gameObject;
                if (grabbedObject == this.gameObject)
                {
                    HingeHelper hingeHelper = gameObject.GetComponent<HingeHelper>();
                    if (hingeHelper != null)
                    {
                        hingeHelper.onHingeSnapChange?.Invoke(100);
                    }
                    Debug.Log("Clicked on object on the grabbable layer: " + grabbedObject.name);
                    Onclick();
                }

                // Add your custom logic here, such as picking up the object, etc.
            }
        }
    }


    void Onclick()
    {
        OnClicked?.Invoke();
    }
}
