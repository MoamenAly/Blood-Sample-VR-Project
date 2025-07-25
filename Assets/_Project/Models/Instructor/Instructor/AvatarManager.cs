using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AvatarManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [Space]
    public Animator avatarAnimator;

    [Space]
    public List<Step> steps;  // List of audio clips to play


    [Space]
    public float timeOnFinishStep = 1.0f;



    int currentIndex = 0;

    //public void PlayStep(int index)
    //{
    //    CancelInvoke();

    //    currentIndex = index;
    //    audioSource.clip = steps[currentIndex].audioClip;
    //    audioSource.Play();


    //    PlayAnimation();

    //    Invoke("OnClipFinished", steps[currentIndex].audioClip.length + timeOnFinishStep);
    //}


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
        steps[currentIndex].OnStart.Invoke();

        audioSource.clip = steps[currentIndex].audioClip;
        audioSource.Play();

        PlayAnimation();

        Invoke("OnClipFinished", steps[currentIndex].audioClip.length + timeOnFinishStep);
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


  
    


    [System.Serializable]
    public class Step
    {
        public string stepName;
        [Space]
        public AudioClip audioClip;
        [Space]
        public UnityEvent2 OnStart;
        [Space]
        public UnityEvent2 OnEnd;
    }
}
