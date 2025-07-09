using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;
using Sirenix.OdinInspector;

public class CollisionEvents : MonoBehaviour
{
    [SerializeField] Item[] itemToCollide;

    [SerializeField] UnityEvent2 OnTriggerEnterEvent, OnTriggerExitEvent;

    [SerializeField, ReadOnly] bool isPlayeing = false;

 
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("On Trigger Enter Called");
        if (other.TryGetComponent<Item>(out Item item))
        {
            foreach (var i in itemToCollide)
            {
                if (item.id == i.id)
                {
                    Debug.Log("Item" + item.id);
                    if (!isPlayeing)
                    {
                        OnTriggerEnterEvent?.Invoke();
                        isPlayeing = true;
                    }
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("On Trigger Exit Called");
        if (other.TryGetComponent<Item>(out Item item))
        {
            foreach (var i in itemToCollide)
            {
                if (item.id == i.id)
                {
                    Debug.Log("Item" + item.id);
                    OnTriggerExitEvent?.Invoke();
                    isPlayeing = false;
                }
            }
        }
    }
}
