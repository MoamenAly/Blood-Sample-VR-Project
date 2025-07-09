using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace Scivr.QuestionSystem
{
    public class BaseQuestion 
    {
        public string questionText;

        public bool hasImage = false;
        [ShowIf(nameof(hasImage))]
        public Sprite questionImage;

        public List<Answer> answers;       

        public UnityEvent OnQuestionShown;

        public List<Answer> GetAnswers()
        {
            return answers;
        }     
       
        public void ShowQuestion()
        {
            OnQuestionShown?.Invoke();
        }



    }


    public class Answer { 
       public string text;
       public bool   isCorrectAnswer;
       public bool hasImage = false;
       [ShowIf(nameof(hasImage))]
       public Sprite answerImage;
    }

}
