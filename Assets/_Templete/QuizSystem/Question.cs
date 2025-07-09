using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using UnityEngine.Events;
using System;
namespace QuizSystem
{
    /// <summary>
    /// All questions are derived from this class
    /// </summary>
    public abstract class Question : MonoBehaviour
    {
        protected QuizManager quizManager;
        public QuestionTextData textData;
        public GameObject QuestionCanvas;
        public Narrations QuestionAudio;

        protected int trials = 2;
        //PUSH_BUTTONS Questions

       
        public List<GameObject> GlowCircles;
        protected Color initialColor = new Color(0f , 0.9797904f , 1f , 1f);

        [Space(30)]
        public UnityEvent2 onStartQuestion;
        public UnityEvent2 onEndQuestion;

        private void Awake()
        {
            quizManager = FindAnyObjectByType<QuizManager>();
            ResetVisualData();
        }
        private void OnEnable()
        {
            onStartQuestion?.Invoke();
        }
        private void OnDisable()
        {
            onEndQuestion?.Invoke();
        }
        protected virtual void Start()
        {
            InitializeStrings();

            AudioManager.Instance.Play(QuestionAudio);
        }

        /// <summary>
        /// Reset any changed colors
        /// </summary>
        protected abstract void ResetVisualData();

        /// <summary>
        /// get data from the scriptable object
        /// </summary>
        protected virtual void InitializeStrings()
        {
            quizManager.questionText._SetTextID(textData.QuestionId);
        }

        /// <summary>
        /// Assign event according to its type (Button, snapzone , ui button ,...)
        /// </summary>
        /// <param name="mylist"></param>
        protected void AssignEventsToList(QuestionAnswer[] mylist)
        {
            for (int i = 0; i < mylist.Length; i++)
            {
                int index = i;
                Button answerbutton = mylist[i].gameObject.GetComponent<Button>();
                SnapZone answerZone = mylist[i].gameObject.GetComponent<SnapZone>();
                UnityEngine.UI.Button button = mylist[i].gameObject.GetComponent<UnityEngine.UI.Button>();
                UIDropZone dropzone = mylist[i].gameObject.GetComponent<UIDropZone>();

                //Debug.Log("Assining index: " + i + " Name: " + mylist[i].gameObject.name + "  indx: " + index);
                Grabbable temp = null;
                if (answerbutton != null) answerbutton.onButtonDown.AddListener(() => ValidateAnswer(temp, index));
                if (answerZone != null) answerZone.OnSnapEvent.AddListener((grabbable) => ValidateAnswer(grabbable, index));
                if (button != null) button.onClick.AddListener(() => ValidateAnswer(temp, index));
                if (dropzone != null) dropzone.OnSnapEvent.AddListener((go) => ValidateAnswer(temp, index));
            }
        }
        /// <summary>
        /// UnAssign event according to its type (Button, snapzone , ui button ,...)
        /// </summary>
        /// <param name="mylist"></param>
        protected void UnAssignEventsToList(QuestionAnswer[] mylist)
        {
            for (int i = 0; i < mylist.Length; i++)
            {
                Button answerbutton = mylist[i].gameObject.GetComponent<Button>();
                SnapZone answerZone = mylist[i].gameObject.GetComponent<SnapZone>();
                UnityEngine.UI.Button button = mylist[i].gameObject.GetComponent<UnityEngine.UI.Button>();
                UIDropZone dropzone = mylist[i].gameObject.GetComponent<UIDropZone>();


                if (answerbutton != null) answerbutton.onButtonDown.RemoveAllListeners();
                if (answerZone != null) answerZone.OnSnapEvent.RemoveAllListeners();
                if (button != null) button.onClick.RemoveAllListeners();
                if (dropzone != null) dropzone.OnSnapEvent.RemoveAllListeners();
            }
        }

        /// <summary>
        /// Validate answer according to the question type
        /// </summary>
        /// <param name="grab">grabbable in case of snapzone </param>
        /// <param name="indx">index of chosen answer</param>
        /// <returns></returns>
        protected abstract IEnumerator ValidateAnswerAfterTime(Grabbable grab, int indx);

        /// <summary>
        /// Call  ValidateAnswerAfterTime function 
        /// </summary>
        /// <param name="grab"></param>
        /// <param name="indx"></param>
        /// 
        protected Coroutine CachAnswer = null;
        protected void ValidateAnswer(Grabbable grab, int indx)
        {
            if (CachAnswer == null) // make sure question is not validating already
            {
               // Debug.Log("<color=red>------------------------------ANSWERINGG------------------</color>");
                CachAnswer= StartCoroutine(ValidateAnswerAfterTime(grab, indx));
            }
            //else
            //{
            //   // Debug.Log("<color=red>---------------COROTINE DOES NOT EQUAL NULL---------------------</color>");
            //}
        }
        protected abstract void UnAssignAllEvents();
        protected virtual void AnswerIsCorrect(int indx)
        {
            Debug.Log("Answer is correct");
            UnAssignAllEvents();
            quizManager.CorrectAnswer(trials);
        }
        /// <summary>
        /// Call CheckOnFalseAnswer function
        /// </summary>
        /// <param name="indx"></param>
        protected void AnswerIsFalse(int indx)
        {
            StartCoroutine(CheckOnFalseAnswer(indx));

        }
        /// <summary>
        /// UnAssign events then override to do a different behavior according to number of trial
        /// </summary>
        /// <param name="indx"></param>
        /// <returns></returns>
        protected virtual IEnumerator CheckOnFalseAnswer(int indx)
        {
           // Debug.Log("-----------------UNASSIGN ALL THHHHINGS----------");
            UnAssignAllEvents();
            yield return new WaitForSeconds(0.1f);
        }


        //------------------------------- Utilities ------------------------------//
        protected void ResetZonesAndChoices(QuestionAnswer[] Answers , QuestionChoice[] Choices)
        {
            foreach (QuestionAnswer answer in Answers) //Reset zones
            {
                answer.AssignGrabbableEvent();
                answer.transform.GetComponent<SnapZone>().ReleaseAll();
                //Extra check For Web
                answer.grabbableInZone = null;

            }
            //For VR & Web
            foreach (QuestionChoice choice in Choices) //reset choices
            {
                choice.transform.parent = transform;
                //NOTE: ForceDefaultPosition() Does not exist in all RestOnUnGrab versions
                if (choice.transform.GetComponent<RestOnUnGrab>()) choice.transform.GetComponent<RestOnUnGrab>().ForceDefaultPosition();
                //Extra check For Web
                choice.transform.GetComponent<Collider>().enabled = true;
            };
        }
        protected void ColorAllUIChoices(Color color)
        {
            foreach (Transform child in QuestionCanvas.transform) child.GetComponent<UnityEngine.UI.Image>().color = color;
        }
        //[Button]
        //protected void ChangeParticleColor(int i , Color color)
        //{
        //    SetGlowColor(i, color);
        //}
        //Used for webGL
        protected void StopChoicePhysics(QuestionChoice[] choicelist)
        {
            foreach (QuestionChoice choice in choicelist)
            {
                choice.GetComponent<Rigidbody>().isKinematic = true;
                choice.GetComponent<Rigidbody>().useGravity = false;
            }

        }
        //used for particle system
        protected void SetGlowForAll(Color color)
        {
            for (int i = 0; i < GlowCircles.Count; i++)
            {
                SetGlowColor(i, color);
            }
        }
        protected void SetGlowColor(int i, Color color)
        {

            var main = GlowCircles[i].GetComponent<ParticleSystem>().main;
            main.startColor = color;

            var mainchild1 = GlowCircles[i].transform.GetChild(0).GetComponent<ParticleSystem>().main;
            mainchild1.startColor = color;

            var mainchild2 = GlowCircles[i].transform.GetChild(1).GetComponent<ParticleSystem>().main;
            mainchild2.startColor = color;

        }
        protected void SetGlowColorForCorrectAnswer(int i, Color color)
        {
            //Debug.Log("GLOW INDEX: " + i);
            i = (int) ( Math.Log(i) / Math.Log(2));
            //Debug.Log("NEW INDEX:  " + i);
            var main = GlowCircles[i].GetComponent<ParticleSystem>().main;
            main.startColor = color;

            var mainchild1 = GlowCircles[i].transform.GetChild(0).GetComponent<ParticleSystem>().main;
            mainchild1.startColor = color;

            var mainchild2 = GlowCircles[i].transform.GetChild(1).GetComponent<ParticleSystem>().main;
            mainchild2.startColor = color;

        }
    }
}

