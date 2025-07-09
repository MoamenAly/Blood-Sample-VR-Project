using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizSystem
{
    [CreateAssetMenu(fileName = "NewQuestionData", menuName = "Question Data", order = 1)]
    public class QuestionTextData : ScriptableObject
    {

         public Language QuestionId;
         public Language[] ChoiceDescription;
    }
}

