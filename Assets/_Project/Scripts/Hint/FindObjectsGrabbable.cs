using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindObjectsGrabbable : MonoBehaviour
{
    [HideInInspector] CustomGrabbable[] _CustomGrabbable;

    public void MakeGrabbableObjectsisKinematic()
    {
        _CustomGrabbable = FindObjectsOfType<CustomGrabbable>();

        foreach (CustomGrabbable customGrabbable in _CustomGrabbable)
        {
            Rigidbody rb = customGrabbable.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
    }
}

