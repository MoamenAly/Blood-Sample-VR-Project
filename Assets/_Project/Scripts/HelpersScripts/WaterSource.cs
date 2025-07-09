using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BNG;
using Sirenix.OdinInspector;

public class WaterSource : MonoBehaviour
{
    [SerializeField] bool IsUseKnob;
    [ShowIf("IsUseKnob")]
    [SerializeField] float halfTargetValue;

    [SerializeField] bool IsUseTrigger;
    [ShowIf("IsUseTrigger")]
    [SerializeField] Item[] itemToCollide;

    [SerializeField] UnityEvent2 OnWaterSourceOpened, OnWaterSourceClosed;

    [SerializeField, ReadOnly] bool isPlayeing = false;

    public void OnWaterSourceCHanged(float value)
    {
        if (value >= halfTargetValue)
        {
            if (!isPlayeing)
            {
                OnWaterSourceOpened?.Invoke();
                isPlayeing = true;
            }
        }
        else
        {

            OnWaterSourceClosed?.Invoke();
            isPlayeing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("On Trigger Enter Called");
        if (other.TryGetComponent<Item>(out Item item))
        {
            foreach(var i in itemToCollide)
            {
                if (item.id == i.id)
                {
                    Debug.Log("Item" + item.id);
                    if (!isPlayeing)
                    {
                        OnWaterSourceOpened?.Invoke();
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
                    OnWaterSourceClosed?.Invoke();
                    isPlayeing = false;
                }
            }
        }
    }

}
