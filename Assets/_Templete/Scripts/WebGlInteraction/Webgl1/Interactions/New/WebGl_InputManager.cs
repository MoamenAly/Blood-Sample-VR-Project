using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGl_InputManager : MonoBehaviour
{
    IMouseInteraction clicked;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                clicked = objectHit.GetComponent<IMouseInteraction>();
                if (clicked != null)
                {
                    clicked.OnWebMouseDown();
                }
            }
        }
        else if(Input.GetMouseButtonUp(0)) {
            if (clicked != null)
            {
                clicked.OnWebMouseUp();
                clicked = null;
            }
        }
       
    }

    private void FixedUpdate()
    {
        
    }
}
