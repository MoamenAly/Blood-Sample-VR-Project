using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NarrationController : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] UnityEvent2 OnNarrationFinished;

    private bool hasPlayed = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (hasPlayed && !audioSource.isPlaying)
        {
            Debug.Log("Audio has finished playing.");
            OnNarrationFinished?.Invoke();
            hasPlayed = false;
        }
    }

    public void PlayNarration()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
}
