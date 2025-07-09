using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

public class StopWatch : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] float speed = 10;

    private float startValue;
    public float endValue;

    public UnityEvent2 OnCompleted;

    private bool DoOnce = true;
    public bool TimerStarted = false;

    public AudioSource sfxSource;
    public AudioClip timerEffect;

    public static StopWatch instance;

    public void _DoOnce(bool val)
    {
        DoOnce = val;
    }

    private void Awake()
    {
        if (instance != null) return;

        instance = this;
    }

    void Update()
    {

        if (!TimerStarted) return;

        if (startValue < endValue)
        {
            startValue += speed * Time.deltaTime;
            _timerText.text = ((int)startValue).ToString();

        }
        else
        {
            if (DoOnce)
            {

                Invoke(nameof(SetTimerCompleted), 1);

                StopTimer();
                DoOnce = false;
            }

        }
    }
    private void SetTimerCompleted()
    {
        OnCompleted?.Invoke();
    }

    [Button("Start Timer")]
    public void StartTimer()
    {
        TimerStarted = true;
        sfxSource.clip = timerEffect;
        sfxSource.Play();
        DoOnce = true;
    }

    [Button("Stop Timer")]
    public void StopTimer()
    {
        TimerStarted = false;
        sfxSource.Stop();
    }


    [Button("Reset Timer")]
    public void ResetTimer()
    {
        DoOnce = true;
        TimerStarted = false;
        startValue = 0;
        _timerText.text = "0";
        sfxSource.Stop();
    }

    [Button("Change Timer End Value")]
    public void ChangeTimerEndValue(float _endValue)
    {
        endValue = _endValue;
    }

    [Button("Change Timer Speed")]
    public void ChangeTimerSpeed(float _speed)
    {
        speed = _speed;
    }

    public void SetTimerText(float timeTxt)
    {
        startValue = timeTxt;
    }
}

