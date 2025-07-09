using BNG;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace QuizSystem
{
    public class SnapZoneQuestion : Question
    {
        //SNAPZONE_3D Questions
        public QuestionChoice[] SnapZone3Dchoices;
        public QuestionAnswer[] SnapZone3Danswers;
        protected override void Start()
        {
            AssignEventsToList(SnapZone3Danswers);
#if UNITY_WEBGL || UNITY_STANDALONE_WIN
            StopChoicePhysics(SnapZone3Dchoices);
#endif
            base.Start();
        }
        protected override IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx)
        {
            yield return new WaitForSeconds(0.2f);
            CachAnswer = null; 
            bool allAnswered = true;
            foreach (QuestionAnswer answer in SnapZone3Danswers)
            {
                if (!answer.HasAnswer()) allAnswered = false;
            }
            if (!allAnswered) yield break;
            trials--; // we will always decrement trials
            bool allCorrect = true;
            foreach (QuestionAnswer answer in SnapZone3Danswers)
            {
                if (answer.grabbableInZone.GetComponent<QuestionChoice>().answer != answer.answer) allCorrect = false;
            }
            if (allCorrect) AnswerIsCorrect(indx);
            else AnswerIsFalse(indx);
        }
        protected override IEnumerator CheckOnFalseAnswer(int indx)
        {
           yield return base.CheckOnFalseAnswer(indx);

            if (trials > 0)
            {
                EnableSnapingOut(false);
                //ColorAllUIChoices(Color.red);// Show it is a wrong answer
                SetGlowForAll(Color.red);
                quizManager.WrongAnswerDonotContinue(); //Score is not calculated
                yield return new WaitForSeconds(2f);


                AssignEventsToList(SnapZone3Danswers); //Assign Events again
                                                       //RESET
                                                       //ColorAllUIChoices(Color.white);
                SetGlowForAll(initialColor);
                EnableSnapingOut(true);
                ResetZonesAndChoices(SnapZone3Danswers, SnapZone3Dchoices);

            }
            else if (trials == 0)
            {
                //foreach (QuestionAnswer answer in SnapZone3Danswers) QuestionCanvas.transform.GetChild((int)answer.answer).GetComponent<UnityEngine.UI.Image>().color = Color.green;
                EnableSnapingOut(false);
                SetGlowForAll(Color.red);
                quizManager.PlayWrongAudio();
                yield return new WaitForSeconds(2f);
                ResetZonesAndChoices(SnapZone3Danswers, SnapZone3Dchoices);
                SetGlowForAll(Color.green);

                foreach (QuestionAnswer answer in SnapZone3Danswers)
                {
                    QuestionChoice choice = SnapZone3Dchoices.FirstOrDefault(choice => choice.answer == answer.answer);
                    Destroy(choice.transform.GetComponent<RestOnUnGrab>());
                    choice.transform.position = answer.transform.position;
                    choice.transform.parent = transform;
                }
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
        protected override void ResetVisualData()
        {
            ColorAllUIChoices(Color.white); //reset at start of question
        }
        protected override void UnAssignAllEvents()
        {
            UnAssignEventsToList(SnapZone3Danswers);
        }
        protected override void AnswerIsCorrect(int indx)
        {
            base.AnswerIsCorrect(indx);
            // ColorAllUIChoices(Color.green);
            SetGlowForAll(Color.green);
            EnableSnapingOut(false);
        }

        private void EnableSnapingOut(bool state)
        {
#if UNITY_WEBGL || UNITY_STANDALONE_WIN
            foreach (QuestionAnswer answer in SnapZone3Danswers)
            {
                answer.gameObject.GetComponent<Collider>().enabled = state;
            }
#endif
#if UNITY_ANDROID
            foreach (QuestionAnswer answer in SnapZone3Danswers)
            {
                answer.gameObject.GetComponent<SnapZone>().CanRemoveItem = state;
            }
#endif
        }


    }

}
