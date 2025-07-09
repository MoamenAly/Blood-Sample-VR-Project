using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeColorSmooth : MonoBehaviour
{
    [SerializeField] private Material render;
    [SerializeField] Color defaultColor;
    [SerializeField] Color[] targetColor;
    [SerializeField] int currentIndex = 0;
    [SerializeField] float time;

    private void Start()
    {
        render.color = defaultColor;
    }

    [ContextMenu("OnChangeColor")]
    public void ChangeColor()
    {
        render.DOColor(targetColor[currentIndex], time);
        currentIndex++;
    }
}
