using UnityEngine;
using UnityEngine.Events;

public class EffectHolder : MonoBehaviour
{

    [SerializeField] UnityEvent2 OnPlayAction;
    [SerializeField] UnityEvent2 OnStopAction;

    //public bool test;

    public bool isPlaying;

    public void Play() {
        Debug.Log("played");
        if (!isPlaying)
        {
            OnPlayAction?.Invoke();
        }
        isPlaying = true;
    }

    public void Stop() { 

     OnStopAction?.Invoke();
        isPlaying = false; 
    }   
}
