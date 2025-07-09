using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectImageColor : MonoBehaviour
{
    [SerializeField] Color correctAnswerColor;
    [SerializeField] Color wrongAnswerColor;
    private UnityEngine.UI.Outline _outlineImage;

    private void Start()
    {
        _outlineImage = GetComponent<UnityEngine.UI.Outline>();
    }

    public void ChangeColorCorrectImage()
    {
        _outlineImage.enabled = true;
        _outlineImage.effectColor = correctAnswerColor;
    }


    public void ChangeColorWrongImage()
    {
        _outlineImage.enabled = true;
        _outlineImage.effectColor = wrongAnswerColor;
    }

    public void ChangeColorDefaultImage()
    {
        _outlineImage.effectColor = Color.white;
        _outlineImage.enabled = false;
    }
}
