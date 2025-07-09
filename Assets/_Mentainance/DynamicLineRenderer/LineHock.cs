using BNG;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineHock : MonoBehaviour
{
    [SerializeField] Vector3 startPositin;
    [SerializeField] Quaternion startRotation;

    [SerializeField] CustomGrabbable grabbable;

    // Start is called before the first frame update
    void Start()
    {
        startPositin = transform.position;    
        startRotation = transform.rotation;
       

        grabbable = GetComponent<CustomGrabbable>();

        grabbable.OnGrabAction += OnGrab;
        grabbable.OnReleaseAction += OnRelase;
    }

    private void OnRelase()
    {
        if (enabled)
        {
            DOVirtual.DelayedCall
             (0.1f,
             ()=>
             {
                 if (enabled)
                 {
                     transform.DOMove(startPositin, 0.25f);
                     transform.DORotateQuaternion(startRotation, 0.25f);
                 }
             });
        }
    }

    private void OnGrab(Grabber grabber)
    {
       
    }
}
