using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using BrunoMikoski.AnimationSequencer;

public class TimerController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] int timer = 0;
    [SerializeField] float speed = 1f;
    [SerializeField] List<TimerEvents> _timerEvents;
    [SerializeField] public int currentIndex;

    [SerializeField] bool isRunning = false;
    [SerializeField] AudioSource timerSource;
    [SerializeField] AudioClip timerStartEffect, timerStopEffect;
    [SerializeField] public UnityEvent2 OnPausedTimer;

    private Coroutine countdownRoutine;


    private void OnValidate()
    {
        for (int i = 0; i < _timerEvents.Count; i++)
        {
            if (_timerEvents[i] != null)
                _timerEvents[i].index = i;
        }
    }

    [Button]
    public void PlayTimer(int timerEventsIndex)
    {
        currentIndex = timerEventsIndex;
        timer = _timerEvents[currentIndex].targetSecond;

        isRunning = true;

        // Start audio
        timerSource.clip = timerStartEffect;
        timerSource.Play();


        // Start countdown
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        countdownRoutine = StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (isRunning && timer > 0)
        {
            UpdateTimerDisplay();
            UpdateTextColor();
            yield return new WaitForSeconds(1f / speed);
            timer--;
        }

        timer = 0;
        UpdateTimerDisplay();
        UpdateTextColor();

        PauseTimer();
        _timerEvents[currentIndex].OnReachSeconds?.Invoke();
    }

    void UpdateTimerDisplay()
    {
        timerText.text = timer.ToString() + "s";
    }

    void UpdateTextColor()
    {
        if (timer <= _timerEvents[currentIndex].targetSecond / 3)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.green;
        }

        if (timer == 0)
        {
            timerText.color = Color.white;
        }
    }

    [Button]
    public void PauseTimer()
    {
        isRunning = false;

        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        // Pause audio
        if (timerSource.isPlaying)
            timerSource.Stop();

        timerSource.clip = timerStopEffect;
        timerSource.Play();

        DOVirtual.DelayedCall(2f, () => OnPausedTimer?.Invoke());
    }

    [Button]
    public void StopTimer()
    {
        isRunning = false;

        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);

        if (timerSource.isPlaying)
            timerSource.Stop();

        timerSource.clip = timerStopEffect;
        timerSource.Play();


        timer = 0;
        UpdateTimerDisplay();
        timerText.color = Color.white;
    }

    [Button]
    public void ResetTimer()
    {
        timer = 0;
        UpdateTimerDisplay();
        timerText.color = Color.white;
    }
}

[Serializable]
public class TimerEvents
{
    [HideInInspector] public int index;

    [FoldoutGroup("$GetGroupTitle")]
    public int targetSecond; // Changed from float to int

    [FoldoutGroup("$GetGroupTitle")]
    public UnityEvent2 OnReachSeconds;

    private string GetGroupTitle => $"Timer Event {index}";
}
