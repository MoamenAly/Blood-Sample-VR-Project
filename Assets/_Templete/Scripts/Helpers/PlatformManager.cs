using BNG;
using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-10000)]
public class PlatformManager : MonoSinglton<PlatformManager>
{
    // UnityEvents to be set in the Inspector
    public UnityEvent2 OnAndroidPlatform;
    public UnityEvent2 OnWebPlatform;

    [ReadOnly]
    public Platform platform;

    public Platform test;


    protected override void Awake()
    {
        base.Awake();
        CheckPlatform();
    }

  

    void CheckPlatform()
    {
#if UNITY_EDITOR
        FireTestEvent();
#elif UNITY_ANDROID
        FireAndroidEvent();
#elif UNITY_WEBGL || UNITY_STANDALONE_WIN
        FireWebEvent();
#else
        Debug.Log("Unsupported platform.");
#endif
    }

    private void FireTestEvent()
    {
        platform = test;
        if (platform == Platform.Android)
        {
            FireAndroidEvent();
        }
        else
        {
            FireWebEvent();
        }
    }

    void FireAndroidEvent()
    {
        platform = Platform.Android;
        if (OnAndroidPlatform != null)
        {
            OnAndroidPlatform.Invoke();
        }
        Debug.Log("Android platform detected.");
    }

    void FireWebEvent()
    {
        platform = Platform.Webgl;

        if (OnWebPlatform != null)
        {
            OnWebPlatform.Invoke();
        }

       StartCoroutine(disableVRUI());
        Debug.Log("Web platform detected.");
    }
    IEnumerator disableVRUI()
    {
        yield return new WaitForSeconds(1.5f);
        VRUISystem.Instance.enabled = false;
    }
    public void HandleCanvas(GameObject canvas)
    {
        if (platform == Platform.Android)
        {
            if(GetComponent<VRCanvas>()) canvas.GetComponent<VRCanvas>().enabled = true;
        }
        else
        { 
            canvas.GetComponent<Canvas>().worldCamera = Camera.main;
            if (GetComponent<VRCanvas>()) 
                canvas.GetComponent<VRCanvas>().enabled = false;
            DOVirtual.DelayedCall(0.15f, () =>
            {
               canvas.GetComponent<Canvas>().worldCamera = Camera.main;
            });
        }
    }
}


public enum Platform { 
 Android,Webgl
}