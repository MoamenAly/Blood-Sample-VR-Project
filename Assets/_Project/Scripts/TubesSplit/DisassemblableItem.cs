using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.Events;
using UnityEngine.InputSystem.iOS;
using VFolders.Libs;

public class DisassemblableItem : MonoBehaviour
{
    public int Order;

    GrabbableUnityEvents _grabbableUnityEvents;
    public UnityEvent2 OnSucces, onFailure;


    private void Awake()
    {
        if (!gameObject.TryGetComponent<GrabbableUnityEvents>(out _grabbableUnityEvents)) _grabbableUnityEvents = gameObject.AddComponent<GrabbableUnityEvents>();

    }

    private void Start()
    {
       // DisassemblableIMananger.Instance.Subscribe(this);
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
        DisassemblableIMananger.Instance.CheckDisassemble(this);
    }


    public void OnTakeitemSucces()
    {


        OnSucces?.Invoke();
       // Invoke(nameof(DestoryOnScusse), 1f);
    }

    public void OnFailureItem()
    {
        onFailure?.Invoke();
    }


    void DestoryOnScusse()
    {
        gameObject.Destroy();
    }
}
