using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ActionWebgl : MonoBehaviour
{
    [SerializeField] UnityEvent2 DoAction;

    public void DoActionInWebgl()
    {
        if(PlatformManager.Instance.platform == Platform.Webgl)
        {
            DoAction?.Invoke();
        }
    }    
}
