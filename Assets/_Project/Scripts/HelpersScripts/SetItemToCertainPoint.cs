using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetItemToCertainPoint : MonoBehaviour
{

    public Transform targetPoint;




    public bool cangotoCertainpoint = false;
    private void Update()
    {

        if (!cangotoCertainpoint) return;

        transform.position = new Vector3(targetPoint.position.x, targetPoint.position.y, targetPoint.position.z);
        transform.rotation =  targetPoint.rotation;
    }


    public void GoTocertainpoint(bool state)
    {
        cangotoCertainpoint = state;
    }


}
