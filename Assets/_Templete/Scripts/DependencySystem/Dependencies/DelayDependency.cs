using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using ReadOnlyAttribute = Sirenix.OdinInspector.ReadOnlyAttribute;


public class DelayDependency : MonoBehaviour ,IDepndency
{
    [SerializeField] float delayTimeInSeconds;
    [ReadOnly] [SerializeField] bool isCompleted = false;
    private bool isTimerStarted = false;

    public bool IsCompleted()
    {
        if (!isTimerStarted) {
            StartTimer(delayTimeInSeconds);
            isTimerStarted = true;
        }

        return isCompleted;
    }

    private void StartTimer(float delayTime) {
        DOVirtual.DelayedCall(delayTime,()=> { isCompleted = true; });
    }

}
