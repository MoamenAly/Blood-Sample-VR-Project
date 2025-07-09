using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClipFinished : MonoBehaviour
{
    public UnityEvent OnClipOneFinished;
    public UnityEvent OnClipTwoFinished;
    public UnityEvent OnClipThreeFinished;

  

    int index = 0;

    public void OnClipFinished() {
        Debug.Log("called " + index);
       switch (index) { 
        case 0:
                OnClipOneFinished?.Invoke();
                break;
            case 1:
                OnClipTwoFinished?.Invoke();

                break;
                case 2:
                OnClipThreeFinished?.Invoke();

                break;
        }
        index++;
    }

    public void OnRestSimulation()
    {
       
    }


}
