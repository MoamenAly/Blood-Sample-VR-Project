using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectsSnapzone : MonoBehaviour
{
    SnapZone _snapZone;
    int currentIndex = 0;
    public int totalSnapped;

    public UnityEvent2 OnAllSnapped;

    private void Start()
    {
        _snapZone = GetComponent<SnapZone>();
    }

    public void OnSnapPotato()
    {
        currentIndex++;
        _snapZone.HeldItem.GetComponent<Outline>().enabled = false;
        _snapZone.HeldItem = null;
        if (currentIndex == totalSnapped)
        {
            OnAllSnapped?.Invoke();
        }
    }
}
