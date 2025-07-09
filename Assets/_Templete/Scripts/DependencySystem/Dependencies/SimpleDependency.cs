using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using static Step;

public class SimpleDependency : MonoBehaviour,IDepndency
{
   [SerializeField][ReadOnly] private bool isDepndencyDone = false;


    [SerializeField] public DelayType _delay = DelayType.None;
    [ShowIf(nameof(HasDelay))]
    [SerializeField] public float _delayInSeconds = 1;

 

    public enum DelayType
    {
         Delay = 0 << 1, None = 1 << 1
    }

    private bool HasDelay()
    {
        return _delay != DelayType.None;
    }
    public bool IsCompleted() {
        //if (autoComplte) { 
        //  autoComplte = false;
        //  _Complete();
        //}
        return isDepndencyDone; 
    }
    public void _Complete()
    {
        if (_delay != DelayType.None)
        {
            DOVirtual.DelayedCall(_delayInSeconds, () =>
            {
                isDepndencyDone = true;
            });
        }
        else
        {
            isDepndencyDone = true;
        }
    }
    public void _UnComplete() {
        isDepndencyDone = false;
    }

}

