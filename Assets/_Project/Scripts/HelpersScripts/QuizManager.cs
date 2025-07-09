using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTLTMPro;
using UnityEngine.Events;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using System;

public class QuizManager : MonoBehaviour
{
    public QuizData[] quizes;
    public GameObject parentQuizTools;
    public RTLTextMeshPro _questionText;
    public RTLTextMeshPro[] _answerTexts;
    public RTLTextMeshPro _scoreText;
    public RTLTextMeshPro _quizNumText;
    public RTLTextMeshPro _timerText;
    public AudioSource successEffect;
    public AudioSource wrongEffect;

    private float elpasedTime_Timer;
    private bool playTimer = false;

    private float scoreCounter = 0;
    private int quizNumCounter = 1;
    private int currentQuizIndex = 0;

    public UnityEvent2 OnFinishedQuizes;

    public static QuizManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public int GetCurrentQuizIndex
    {
        get { return currentQuizIndex; }
    }


    [Button("StartQuiz")]
    public void StartQuiz()
    {
        if (quizes.Length > 0)
        {
            DisplayQuizAndTools();
            DisplayScore();
            PlayingTimer();
        }
    }

    private void Update()
    {
        if (playTimer == true)
            SetupTimerAndDisplay();
    }

    #region Question

    QuizData quizdata;
    GameObject instantiatedQuizTool;

    void DisplayQuizAndTools()
    {
        if (currentQuizIndex < quizes.Length)
        {
            Debug.Log("Exist Quiz");
            quizdata = quizes[currentQuizIndex];

            InstantiateQuizTools();

            GameObject parentQuestion = instantiatedQuizTool.transform.GetChild(0).gameObject;
            GameObject parentAnswers = instantiatedQuizTool.transform.GetChild(1).gameObject;

            _questionText = GetTextOfQuestion(parentQuestion);
            _questionText.text = quizdata.questionText;

            for (int i = 0; i < _answerTexts.Length; i++)
            {
                if (i < quizdata.answerTexts.Length)
                {
                    _answerTexts[i] = GetTextOfAnswers(parentAnswers)[i];
                    _answerTexts[i].text = quizdata.answerTexts[i];
                }
            }

            AudioManager.Instance.Play(quizdata.NarrationQuizId);

            _quizNumText.text = quizNumCounter.ToString() + "/" + quizes.Length.ToString();
        }
        else
        {
            Debug.Log("No Exist Quiz");
            OnFinishedQuizes.Invoke();
        }
    }

    private void InstantiateQuizTools()
    {
        DestoryQuizTools();

        instantiatedQuizTool = Instantiate(quizdata.QuizTools);
        instantiatedQuizTool.transform.SetParent(parentQuizTools.transform);
        instantiatedQuizTool.transform.localPosition = Vector3.zero;
        instantiatedQuizTool.transform.localRotation = Quaternion.identity;
   //     instantiatedQuizTool.transform.localScale = Vector3.one;
    }

    public void DestoryQuizTools()
    {
        if (parentQuizTools.transform.childCount > 0)
        {
            foreach (Transform child in parentQuizTools.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public RTLTextMeshPro GetTextOfQuestion(GameObject parent)
    {
        RTLTextMeshPro texts = parent.GetComponentInChildren<RTLTextMeshPro>();

        return texts;
    }

    public RTLTextMeshPro[] GetTextOfAnswers(GameObject parent)
    {
        RTLTextMeshPro[] texts = parent.GetComponentsInChildren<RTLTextMeshPro>();

        return texts;
    }

    void DisplayScore()
    {
        _scoreText.text = scoreCounter.ToString();
    }

    void PlayingTimer()
    {
        playTimer = true;
    }


    void SetupTimerAndDisplay()
    {
        elpasedTime_Timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elpasedTime_Timer / 60);
        int seconds = Mathf.FloorToInt(elpasedTime_Timer % 60);
        _timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    #endregion

    #region Answer

    public void CorrectAnswer(float IncreaseScoreValue)
    {
        Debug.Log("Correct Answer!");
        scoreCounter += IncreaseScoreValue;
        DisplayScore();
        successEffect.Play();
        ShowNextQuiz();
    }

    public void WrongAnswer(bool isFinalTry)
    {
        Debug.Log("Wrong Answer. Try again!");
        if (isFinalTry)
            ShowNextQuiz();
        else
            wrongEffect.Play();
    }

    private async void ShowNextQuiz()
    {
        await Task.Delay(4000);
        currentQuizIndex++;
        quizNumCounter++;
        DisplayQuizAndTools();
    }

  


    #endregion
}
