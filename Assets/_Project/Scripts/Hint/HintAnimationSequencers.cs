using BrunoMikoski.AnimationSequencer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintAnimationSequencers : MonoBehaviour
{
    [SerializeField] AnimationSequencerController[] controller;
    [SerializeField] int currentIndex = 1;

    public void SetIndex(int _index)
    {
        currentIndex = _index;
    }

    public void HintButton()
    {
        controller[currentIndex - 1].Rewind();
        controller[currentIndex - 1].Play();
    }
}
