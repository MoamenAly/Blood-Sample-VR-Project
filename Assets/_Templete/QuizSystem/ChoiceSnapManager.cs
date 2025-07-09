using DG.Tweening;
using QuizSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizSystem
{
    //Used for handling WebGL
    public class ChoiceSnapManager : MonoBehaviour
    {
        public List<QuestionChoice> choices;
        public QuestionChoice CurrentChoice;

        private QuizManager _quizManager;

        public static ChoiceSnapManager Instance;

        private void Awake()
        {
            Instance = this;
            _quizManager = GetComponent<QuizManager>();
            _quizManager.UpdateSnapManager += updateChoiceList;
        }


        private void updateChoiceList()
        {
     
            if (_quizManager.questions[_quizManager.currQuestion] is SnapZoneQuestion)
            {
                SnapZoneQuestion snapZoneQuestion = (SnapZoneQuestion)_quizManager.questions[_quizManager.currQuestion];
                choices.Clear();
                foreach (QuestionChoice choice in snapZoneQuestion.SnapZone3Dchoices)
                {
                    choices.Add(choice);
                }
            }
        }
        public void SetCurrentChoice(QuestionChoice curr)
        {
            if (CurrentChoice == curr) return;
            if (_quizManager.questions[_quizManager.currQuestion] is SnapZoneQuestion)
            {
                SnapZoneQuestion snapZoneQuestion = (SnapZoneQuestion)_quizManager.questions[_quizManager.currQuestion];
                if (snapZoneQuestion.SnapZone3Danswers[0].gameObject.GetComponent<Collider>().enabled == false) return;
            }
            //to stop movement during showing the right answer
            if (CurrentChoice != null)
            {
                CurrentChoice.transform.DOMoveY(CurrentChoice.transform.position.y - 0.1f, 1f);

            }
            CurrentChoice = curr;
            CurrentChoice.transform.DOMoveY(CurrentChoice.transform.position.y + 0.1f, 1f);
        }


    }
}

