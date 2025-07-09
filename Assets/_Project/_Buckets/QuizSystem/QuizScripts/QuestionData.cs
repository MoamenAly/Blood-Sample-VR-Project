using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="Question" , menuName = "Scriptableobjects/question", order = 1)]
public class QuestionData : ScriptableObject
{

    //private void OnValidate()
    //{
    //    foreach (var question in Questions)
    //    {
    //        question.text = LanguageManager.Instance.GetText(question.language);
    //        //question.text = LanguageManager.Instance.GetText(question.language);
    //        //question.text = LanguageManager.Instance.GetText(question.language);
    //        //question.text = LanguageManager.Instance.GetText(question.language);
    //        //question.text = LanguageManager.Instance.GetText(question.language);
    //    }
    //}

    public List<Question> Questions;

    private void OnEnable()
    {
        ResetData();
    }


    void ResetData() {
        Questions.ForEach(question => question.Answered = false);
    }
}


public enum Layout {
    None,
    Horizontal,
    Vertical
}


[System.Serializable]
public class Question
{
    [TextArea]
    public string question;
  
    //public Language language;
    //public string text;
    [Tooltip("The corect answer should always be listed first , they are randmoized later")]
    public Answer[] answers;
    public AudioClip QuestionAudio;
    public bool _answerd = false;
    public Layout Layout;
    public GameObject questionPrefab;
    public bool Answered { get { return _answerd; } set { _answerd = value; } }

   

}


[System.Serializable]
public class Answer
{
    public string answerText;
    public Sprite _answerSprite;
    public bool IsCorrect;
}