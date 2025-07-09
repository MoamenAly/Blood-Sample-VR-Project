using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
public class AnswerButton : MonoBehaviour
{
    public bool isCorrect , Selected;

    [SerializeField]
    private TextMeshProUGUI answertext;
    [SerializeField] Image AnswerSprite;
    [SerializeField] GameObject trueImage, falseimage , selected;


    public UnityAction OnVisualupdate;
    public UnityAction OnResetVisual;


    public QuestionSetup _questionsetup;
    private void OnEnable()
    {
        OnVisualupdate += UpdateAnswerVisual;
        OnResetVisual += ResetAnswerVisual;
    }
    public void SetAnswerText(string newText)
    {
        answertext.text = newText;
    } 



    public void SetAnswerSprite(Sprite _sprite) {
        AnswerSprite.sprite = _sprite;
    }




    public void SetIsCorrrect(bool newBool)
    {
        isCorrect = newBool;
    }


    public void OnClick()
    {
        _questionsetup.SelectButton(this);
        _questionsetup.OnsubmitButtonClicked();
    }

    void ResetAnswerVisual() {

        trueImage.SetActive(false);
        falseimage.SetActive(false);
    }


    void UpdateAnswerVisual()
    {
        if (isCorrect)
        {
            trueImage.SetActive(true);

        }
        else
        {
            falseimage.SetActive(true);
        }
    }


    public void Select()

    {
        Selected = true;
        selected.SetActive(true);
    }

    public void Deselect()
    {

        Selected = false;
        selected.SetActive(false);
    }

    public bool IsRightAnswer () =>  isCorrect && Selected; 

     
}
