using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnSnapzoneAction : MonoBehaviour
{
    private SnapZone _SnapZone;

    private void Start()
    {
        _SnapZone = GetComponent<SnapZone>();
    }

   public void OnSnapped(Grabbable grabObject)
    {

        _SnapZone.GrabGrabbable(grabObject);
        //_SnapZone.HeldItem = grabObject;
        //grabObject.transform.parent = _SnapZone.transform;
        //_SnapZone.OnSnapEvent.Invoke(_SnapZone.HeldItem);
    }
}
