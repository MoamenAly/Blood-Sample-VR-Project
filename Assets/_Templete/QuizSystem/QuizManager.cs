using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System.Collections.Generic;
namespace QuizSystem
{
    public class QuizManager : MonoBehaviour
    {
        [Header("Make Quiz Manager and questions GAMEOBJECTS inactive before running")]
        public Theme_Text questionText;
        //public GameObject QuestionCanvas;

        public Question[] questions;
        [SerializeField] private Timer timer;
        [SerializeField] private TMP_Text NumberQuestionText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text FinalScore;
        [SerializeField] private TMP_Text FinalTime;

        public int currQuestion = 0;

       // public AudioSource AudioSourceManager;
        public Narrations Correct;
        public Narrations Wrong;
        public Narrations TryAgain;
        public Narrations GreatJob;

        private float Score = 0;

        //--------------- Events--------------------//
        public UnityEvent2 OnQuizStart;
        public UnityEvent2 OnQuizEnd;

        //graphical data
        [Header("UI Question data")]
        //For UI questions
        public Sprite correct;
        public Sprite wrong;
        public Sprite empty;


        //For Snap Manage For Web
        public UnityAction UpdateSnapManager;
        // Start is called before the first frame update
        void Start()
        {
            //--- Quiz Stuff ---//
           
            scoreText.text = Score.ToString();
            //totalQuestionsText.text = questions.Length.ToString();
            //shuffle
           // Shuffle(questions);
            MoveToNextQuestion();
        }
        [Button]
        public void MoveToNextQuestion()
        {
            StartCoroutine(MovetoNext());
        }
        public void Shuffle<T>(IList<T> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        IEnumerator MovetoNext()
        {
            yield return new WaitForSeconds(2.5f);

            if (currQuestion == 0) 
            {
                timer.StartTime();
                OnQuizStart?.Invoke(); 
            }
            if (questions.Length <= 0) yield break;
            if (currQuestion > 0) questions[currQuestion - 1].gameObject.SetActive(false); //close current question

            if (currQuestion == questions.Length)
            {
                Debug.Log("quiz should end");
                AudioManager.Instance.Play(GreatJob);
                FinalScore.text = Score.ToString();
                FinalTime.text = timer.getCurrentTime();
                OnQuizEnd?.Invoke();

                yield break;
            }
            UpdateSnapManager?.Invoke();

            questions[currQuestion].gameObject.SetActive(true);
            currQuestion++;

            NumberQuestionText.text = currQuestion + "/" + questions.Length.ToString(); // update current question count
        }

        
        public void CorrectAnswer(int trial)
        {
            AudioManager.Instance.Play(Correct);

            if (trial == 1) Score++;
            else if (trial == 0) Score += 0.5f;

            scoreText.text = Score.ToString();
            Debug.Log("HEEEREEEE");
            MoveToNextQuestion();

        }
        public void WrongAnswer()
        {
            AudioManager.Instance.Play(Wrong);

            MoveToNextQuestion();
        }
        public void PlayWrongAudio()
        {
            AudioManager.Instance.Play(Wrong);
        }
        public void WrongAnswerDonotContinue()
        {
            AudioManager.Instance.Play(TryAgain);
        }

        
    }
}

