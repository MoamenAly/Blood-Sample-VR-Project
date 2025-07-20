using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.Events;
public class SafteyItem : MonoBehaviour
{

    public int Order;

    GrabbableUnityEvents _grabbableUnityEvents;
    public UnityEvent2 OnSucces;


    private void Awake()
    {
        if (!gameObject.TryGetComponent<GrabbableUnityEvents>(out _grabbableUnityEvents)) _grabbableUnityEvents = gameObject.AddComponent<GrabbableUnityEvents>();
       
    }

    private void Start()
    {
        SafteyManager.Instance.SubscribeItem(this);
    }

    private void OnEnable()
    {
        _grabbableUnityEvents.onGrab.AddListener(OnGrabItem);
    }

    private void OnDisable()
    {
        _grabbableUnityEvents.onGrab.RemoveListener(OnGrabItem);
    }

    private void OnGrabItem(Grabber grabbe)
    {

        SafteyManager.Instance.CheckSaftey(this);
    }


    public void OnTakeitemSucces()
    {

       
        OnSucces?.Invoke();
    }

}
