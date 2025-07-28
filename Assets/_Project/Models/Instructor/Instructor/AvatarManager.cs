using System;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

public class AvatarManager : MonoBehaviour
{

    public bool runOnStart = false;
   
    [SerializeField] private AudioSource audioSource;
    [Space]
    public Animator avatarAnimator;

    [Space]
    public List<Step> steps;  // List of audio clips to play


    [Space]
    public float timeOnFinishStep = 1.0f;



    int currentIndex = 0;

    public void Start()
    {

        if (runOnStart)
        {
            PlayStep(steps[currentIndex].stepName);

        }
    }

    [Button]
    public void PlayStep(string _Name)
    {
        CancelInvoke();

        for (int i = 0; i < steps.Count; i++)
        {
            if (steps[i].stepName == _Name)
            {
                currentIndex = i;
            }
        }
        steps[currentIndex].localizedClip.LoadAssetAsync().Completed += handle =>
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
                Debug.LogError($"Failed to load localized audio for step: {steps[currentIndex].stepName}");
            }
        };
        steps[currentIndex].OnStart?.Invoke();

    }



    [Button]
    public void PlayAnimation()
    {
        print("Play animation");
       avatarAnimator.SetBool("Talking", audioSource.isPlaying);
        print("correct " + audioSource.isPlaying);
    }



    void OnClipFinished()
    { 
        steps[currentIndex].OnEnd.Invoke();
        PlayAnimation();
    }




    public void StopStep()
    {
        // Cancel any pending method calls
        CancelInvoke();

        // Stop audio playback
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Stop animation (set Talking to false)
        if (avatarAnimator != null)
        {
            avatarAnimator.SetBool("Talking", false);
        }

        // Optionally: invoke OnEnd manually if needed
        steps[currentIndex].OnEnd?.Invoke();
    }



    [System.Serializable]
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
