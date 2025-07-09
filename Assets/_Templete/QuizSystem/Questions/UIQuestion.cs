using BNG;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace QuizSystem
{
    public class UIQuestion : Question
    {
        //UI Questions
        public QuestionAnswer[] UIAnswers;
        public AnswerEnum UICorrectAnswer;

        protected override void Start()
        {
            AssignEventsToList(UIAnswers);
            PlatformManager.Instance.HandleCanvas(gameObject);
            base.Start();

        }

        protected override IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx)
        {
            yield return new WaitForSeconds(0.2f);
            CachAnswer = null;
            trials--; // we will always decrement trials
            //Debug.Log("PushButtonAnswers[indx].answer:  " + indx);
            if ((UIAnswers[indx].answer & UICorrectAnswer) != 0)
            {
                AnswerIsCorrect(indx);
            }
            else
            {
                AnswerIsFalse(indx);
            }

        }

        protected override IEnumerator CheckOnFalseAnswer(int indx)
        {
           yield return  base.CheckOnFalseAnswer(indx);

            if (trials > 0)
            {
                QuestionCanvas.transform.Cast<Transform>().ElementAt(indx).GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = quizManager.wrong;

                quizManager.WrongAnswerDonotContinue(); //Score is not calculated

                yield return new WaitForSeconds(1f);
                AssignEventsToList(UIAnswers);
                ChangeAllUISprites(quizManager.empty);

            }
            else if (trials == 0)
            {

                int index = 0;
                foreach (Transform child in QuestionCanvas.transform)
                {
                    if (index == (int)UICorrectAnswer) child.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = quizManager.correct;
                    else child.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = quizManager.wrong;
                    index++;
                }
                quizManager.WrongAnswer();//Score is Zero
            }
        }
        protected override void ResetVisualData()
        {
            ChangeAllUISprites(quizManager.empty);
        }
        protected override void InitializeStrings()
        {
            base.InitializeStrings();
            int i = 0;
            foreach (Transform child in QuestionCanvas.transform)
            {
                Theme_Text choicetext = child.GetChild(0).GetChild(0).GetComponent<Theme_Text>();
                choicetext._SetTextID(textData.ChoiceDescription[i]);
                i++;
            }

        }
        protected override void UnAssignAllEvents()
        {
            UnAssignEventsToList(UIAnswers);
        }

        protected override void AnswerIsCorrect(int indx)
        {
            base.AnswerIsCorrect(indx);

            //TODO: USE LINQ
            int index = 0;
            foreach (Transform child in QuestionCanvas.transform)
            {
                if (index == (int)UICorrectAnswer) child.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = quizManager.correct;
                //else child.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = quizManager.wrong;
                index++;
            }

        }

        private void ChangeAllUISprites(Sprite sprite)
        {
            foreach (Transform child in QuestionCanvas.transform)
            {
                child.GetChild(1).GetComponent<UnityEngine.UI.Image>().sprite = sprite;
            }
        }
        
    }

}
