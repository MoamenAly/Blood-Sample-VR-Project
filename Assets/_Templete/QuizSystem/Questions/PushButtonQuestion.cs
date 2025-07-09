using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;

namespace QuizSystem
{
    public class PushButtonQuestion : Question
    {
        public QuestionAnswer[] PushButtonAnswers;
        public AnswerEnum IDCorrectAnswer;
        // Start is called before the first frame update
        protected override void Start()
        {
            AssignEventsToList(PushButtonAnswers);
            base.Start();
            
        }

        protected override IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx)
        {
            yield return new WaitForSeconds(0.2f);
            CachAnswer = null;
            trials--; // we will always decrement trials
                      //Debug.Log("PushButtonAnswers[indx].answer:  " + indx);
            if ((PushButtonAnswers[indx].answer & IDCorrectAnswer) != 0) // answer exists in enum
                AnswerIsCorrect(indx);
            else
                AnswerIsFalse(indx);

           
        }
        protected override IEnumerator CheckOnFalseAnswer(int indx)
        {
         // yield return base.CheckOnFalseAnswer(indx);
           yield return base.CheckOnFalseAnswer(indx);

            if (trials > 0)
            {
                //Show Wrong Answer
                //QuestionCanvas.transform.GetChild(indx).GetComponent<UnityEngine.UI.Image>().color = Color.red;
                SetGlowColor(indx, Color.red);
                quizManager.WrongAnswerDonotContinue(); //Score is not calculated

                yield return new WaitForSeconds(2f);
                SetGlowColor(indx, initialColor);
                //Reset
                AssignEventsToList(PushButtonAnswers);
                QuestionCanvas.transform.GetChild(indx).GetComponent<UnityEngine.UI.Image>().color = Color.white;

            }
            else if (trials == 0)
            {
                //Show Wrong Answer
                //QuestionCanvas.transform.GetChild(indx).GetComponent<UnityEngine.UI.Image>().color = Color.red;
                SetGlowColor(indx, Color.red);

                //Show Correct Answer
                //QuestionCanvas.transform.GetChild((int)IDCorrectAnswer).GetComponent<UnityEngine.UI.Image>().color = Color.green;
                SetGlowColorForCorrectAnswer((int)IDCorrectAnswer, Color.green);
                yield return new WaitForSeconds(2f);
                quizManager.WrongAnswer();//Score is Zero
            }
        }
        protected override void InitializeStrings()
        {
            base.InitializeStrings();
            int i = 0;
            foreach (Transform child in QuestionCanvas.transform)
            {
                Theme_Text choicetext = child.GetChild(0).GetComponent<Theme_Text>();
                choicetext._SetTextID(textData.ChoiceDescription[i]);
                i++;
            }
           
        }
        protected override void ResetVisualData()
        {
            ColorAllUIChoices(Color.white); //reset at start of question
        }
        protected override void UnAssignAllEvents()
        {
       //     Debug.Log("Call Un ASSign ALL ");
            UnAssignEventsToList(PushButtonAnswers);
        }

        protected override void AnswerIsCorrect(int indx)
        {
            base.AnswerIsCorrect(indx);
            //QuestionCanvas.transform.GetChild(indx).GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.green;
            SetGlowColor(indx, Color.green);
        }
    }
}

