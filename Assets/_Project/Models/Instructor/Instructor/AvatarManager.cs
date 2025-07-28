using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class AvatarManager : MonoBehaviour
{
    public bool runOnStart = false;

    [SerializeField] private AudioSource audioSource;
    [Space]
    public Animator avatarAnimator;
    [Space]
    public List<Step> steps; // List of steps with localized audio
    [Space]
    public float timeOnFinishStep = 1.0f;

    private int currentIndex = 0;

    private void Start()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        if (runOnStart && steps.Count > 0)
        {
            PlayStep(steps[currentIndex].stepName);
        }
    }

    private void OnDestroy()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale locale)
    {
        Debug.Log("Locale changed to: " + locale.Identifier.Code);

        if (audioSource.isPlaying)
        {
            StopStep(); // Stop old clip before playing the new one
            PlayStep(steps[currentIndex].stepName);
        }
    }

    [Button]
    public void PlayStep(string stepName)
    {
        CancelInvoke();

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].stepName == stepName)
            {
                currentIndex = i;
                break;
            }
        }

        var step = steps[currentIndex];

        step.localizedClip.LoadAssetAsync().Completed += handle =>
        {
            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                AudioClip clip = handle.Result;
                audioSource.clip = clip;
                audioSource.Play();

                PlayAnimation();

                Invoke(nameof(OnClipFinished), clip.length + timeOnFinishStep);
            }
            else
            {
                Debug.LogError($"Failed to load localized audio for step: {step.stepName}");
            }
        };

        step.OnStart?.Invoke();
    }

    [Button]
    public void PlayAnimation()
    {
        bool isPlaying = audioSource.isPlaying;
        avatarAnimator.SetBool("Talking", isPlaying);
    }

    private void OnClipFinished()
    {
        steps[currentIndex].OnEnd?.Invoke();
        PlayAnimation(); // Update animation status
    }

    public void StopStep()
    {
        CancelInvoke();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (avatarAnimator != null)
        {
            avatarAnimator.SetBool("Talking", false);
        }

        steps[currentIndex].OnEnd?.Invoke();
    }

    [Serializable]
    public class Step
    {
        [FoldoutGroup("Step")]
        public string stepName;

        [FoldoutGroup("Step")]
        [Space]
        public LocalizedAudioClip localizedClip;

        [FoldoutGroup("Step")]
        [Space]
        public UnityEvent2 OnStart;

        [FoldoutGroup("Step")]
        [Space]
        public UnityEvent2 OnEnd;
    }
}
