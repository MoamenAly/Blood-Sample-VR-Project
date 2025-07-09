using BNG;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace QuizSystem
{
    public class SnapUIQuestion : Question
    {

        //2d SNAP ZONE
        public QuestionChoice[] SnapZone2Dchoices;
        public QuestionAnswer[] SnapZone2Danswers;
        protected override void Start()
        {
            AssignEventsToList(SnapZone2Danswers);
            PlatformManager.Instance.HandleCanvas(gameObject);
            base.Start();

        }
        protected override IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx)
        {
            yield return new WaitForSeconds(0.2f);
            CachAnswer = null;
            //First check if All SnapZones Have Answers
            bool allAnswered = true;
            foreach (QuestionAnswer answer in SnapZone2Danswers)
            {
                if (!answer.HasAnswer()) allAnswered = false;
            }
            if (!allAnswered) yield break;
            trials--; // we will always decrement trials
            bool allCorrect = true;
            foreach (QuestionAnswer answer in SnapZone2Danswers)
            {

                if ( (answer.grabbableInZone.gameObject.GetComponent<QuestionChoice>().answer & answer.answer) == 0 ) allCorrect = false;
            }
            if (allCorrect) AnswerIsCorrect(indx);
            else AnswerIsFalse(indx);

          

        }
        protected override void ResetVisualData()
        {
            return;
        }

        protected override void UnAssignAllEvents()
        {
            UnAssignEventsToList(SnapZone2Danswers);
        }
        protected override IEnumerator CheckOnFalseAnswer(int indx)
        {
           yield return base.CheckOnFalseAnswer(indx);

            if (trials > 0)
            {
                foreach (QuestionChoice choice in SnapZone2Dchoices) choice.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.red;
                quizManager.WrongAnswerDonotContinue(); //Score is not calculated

                yield return new WaitForSeconds(2f);
                AssignEventsToList(SnapZone2Danswers);
                foreach (QuestionChoice choice in SnapZone2Dchoices) choice.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.white;
                foreach (QuestionAnswer answer in SnapZone2Danswers)
                {
                    answer.transform.GetComponent<UIDropZone>().ReleaseAll();
                    answer.AssignUISnapZoneEvent();

                    //Extra check For Web
                    answer.grabbableInZone = null;

                }
            }
            else if (trials == 0)
            {
                foreach (QuestionChoice choice in SnapZone2Dchoices) choice.gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.red;

                yield return new WaitForSeconds(2f);

                foreach (QuestionAnswer zone in SnapZone2Danswers)
                {
                    QuestionChoice choice = SnapZone2Dchoices.FirstOrDefault(choice => choice.answer == zone.answer);
                    choice.gameObject.GetComponent<UnityEngine.UI.Image>().color = Color.green;
                    choice.gameObject.GetComponent<DragDropUI>().MoveToDropZone(zone.gameObject.transform);
                }
                quizManager.WrongAnswer();//Score is Zero
            }

        }
        protected override void InitializeStrings()
        {
            base.InitializeStrings();

            int i = 0;
            foreach (QuestionChoice choice in SnapZone2Dchoices)
            {
                Theme_Text choicetext = choice.gameObject.transform.GetChild(0).GetComponent<Theme_Text>();
                choicetext._SetTextID(textData.ChoiceDescription[i]);
                i++;
            }
        }
        protected override void AnswerIsCorrect(int indx)
        {
            base.AnswerIsCorrect(indx);
            foreach (QuestionChoice choice in SnapZone2Dchoices)
            {
                choice.gameObject.GetComponent<UnityEngine.UI.Image>().color = UnityEngine.Color.green;
            }
        }
    }
}

