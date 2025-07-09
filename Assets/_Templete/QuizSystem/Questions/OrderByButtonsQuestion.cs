using BNG;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 namespace QuizSystem
{
    public class OrderByButtonsQuestion : Question
    {
        //ORDER by buttons Questions
        public QuestionAnswer[] OrderButtonsAnswers;
        public AnswerEnum[] CorrectButtonsOrder;
        public Button ResetButton;
        [ReadOnly]public List<QuestionAnswer> RecorededSequence;
        // Start is called before the first frame update
        protected override void Start()
        {
            AssignEventsToList(OrderButtonsAnswers);
            ResetButton.onButtonDown.AddListener(ResetRecordedOrder);
            base.Start();

        }
        protected override void ResetVisualData()
        {
            ColorAllUIChoices(Color.white); //reset at start of question
        }

        protected override void UnAssignAllEvents()
        {
            UnAssignEventsToList(OrderButtonsAnswers);
        }

        protected override IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx)
        {
            yield return new WaitForSeconds(0.2f);
            CachAnswer = null;
            bool allAnswered = true;
            // If button is already pressed do not do anything when it is pressed again
            if (RecorededSequence.Contains(OrderButtonsAnswers[indx])) yield break;

            RecorededSequence.Add(OrderButtonsAnswers[indx]);
            OrderButtonsAnswers[indx].order.text = RecorededSequence.Count.ToString();
            foreach (QuestionAnswer answer in OrderButtonsAnswers)
            {
                if (!answer.HasAnswer()) allAnswered = false;
            }
            if (!allAnswered) yield break;

            trials--; // we will always decrement trials
            bool allCorrect = true;

            for (int i = 0; i < OrderButtonsAnswers.Length; i++)
            {
                if ((RecorededSequence[i].answer & CorrectButtonsOrder[i]) == 0) allCorrect = false;
            }
            if (allCorrect) AnswerIsCorrect(indx);
            else AnswerIsFalse(indx);
        }
        protected override IEnumerator CheckOnFalseAnswer(int indx)
        {
          yield return base.CheckOnFalseAnswer(indx);
            if (trials > 0)
            {

                //foreach (Transform child in QuestionCanvas.transform) child.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.red;
                SetGlowForAll(Color.red);
                quizManager.WrongAnswerDonotContinue(); //Score is not calculated

                yield return new WaitForSeconds(0.8f);
                AssignEventsToList(OrderButtonsAnswers);
                SetGlowForAll(initialColor);
                //reset
                //foreach (Transform child in QuestionCanvas.transform) child.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.white;
                foreach (QuestionAnswer answer in OrderButtonsAnswers)
                {
                    answer.order.text = "";
                    answer.ButtonClicked = false;
                    answer.AssignClickEventToButton();
                }

                RecorededSequence.Clear();

            }
            else if (trials == 0)
            {
                //foreach (Transform child in QuestionCanvas.transform) child.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.red;
                quizManager.PlayWrongAudio();
                SetGlowForAll(Color.red);
                yield return new WaitForSeconds(1f);
                SetGlowForAll(Color.green);
                foreach (QuestionAnswer answer in OrderButtonsAnswers)
                {
                    answer.order.color = Color.green;
                    answer.order.text = SequenceEnumOrder(answer.answer).ToString();

                }
                yield return new WaitForSeconds(2f);
                quizManager.MoveToNextQuestion();//Score is Zero
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

        protected override void AnswerIsCorrect(int indx)
        {
            base.AnswerIsCorrect(indx);
            SetGlowForAll(Color.green);
        }

        //--------------- Utilities -------------------------//
        private int SequenceEnumOrder(AnswerEnum answer)
        {
            //foreach (QuestionAnswer answer in OrderAnswers) //Correct Answer
            //{
            //    answer.order.color = Color.green;
            //    answer.order.text = SequenceEnumOrder(answer.answer).ToString();
            //    //------------------------------------------------------------//
            //}
            for (int i = 0; i < CorrectButtonsOrder.Length; i++)
            {
                if (answer == CorrectButtonsOrder[i]) return i + 1;
            }
            return 0;
        }
        //Used for order by buttons 
        protected void ResetRecordedOrder()
        {
            foreach (QuestionAnswer answer in OrderButtonsAnswers)
            {
                answer.order.text = "";
                answer.ButtonClicked = false;
                //if (IsButtons) answer.AssignClickEventToButton();
                //else answer.AssignGrabbableEvent();
            }

            RecorededSequence.Clear();
        }
    }
}

