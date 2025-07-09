using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;
using System.Linq;
using UnityEngine.Events;
using BNG;
public class QuestionSetup : MonoBehaviour
{
    [SerializeField]
    private QuestionData questions;

    public Question currentQuestion;

    [SerializeField] TextMeshProUGUI QuestionTitle;

    public GameObject horizontalLayout;
    [SerializeField] AnswerButton[] answerButtonsHorizontal;

    public GameObject verticalLayout;
    [SerializeField] AnswerButton[] answerButtonsVertical;

    public GameObject noneLayout;
    [SerializeField] PointerEvents[] answerButtonsNone;

    [SerializeField] UnityEngine.UI.Button Submit;

    [Tooltip("Question Menu")]
    [SerializeField] TextMeshProUGUI Timer;
    [SerializeField] TextMeshProUGUI questionNumber;
    [SerializeField] TextMeshProUGUI Score;

    private AnswerButton[] answerButtons;

    float elpasedTime;

    private int CorrectAnserChoice;
    public int currentQuestionIndex = 0;
    int ALlQuestionNumber;
    //   [SerializeField]  SoundManager _soundmanagr;
       AudioSource _audioSource;

    
    [SerializeField] AudioClip CorrectAnswer, wrongAnswer;
    [SerializeField] AudioClip tryagain, execelent;
    public UnityEvent OnQuizFinishedEvent;

    private void Awake()
    {
           _audioSource = gameObject.AddComponent<AudioSource>();
    }


 

    private void Start()
    {
        ScoreManager.ResetAllDatat();

        ALlQuestionNumber = questions.Questions.Count;

        Shuffle(questions.Questions);

        UpdateQuestion(true);
        Submit.onClick.AddListener(OnsubmitButtonClicked);
    }

    private void Update()
    {
        SetupTimer();
    }

#region setupQuestion


    int QuestionIndex = 0;
    void SelectNewQuestion()
    {
        SoundManager.Instance.StopNarration();

        if (QuestionIndex == questions.Questions.Count) return;
        currentQuestion = questions.Questions[QuestionIndex];
        QuestionIndex++;

        SoundManager.Instance.PlayNarration(currentQuestion.QuestionAudio, 0);
    }

    void SetQuestionValues()
    {
        QuestionTitle.text = currentQuestion.question;
    }

    void SetAnswerValues()
    {
        List<Answer> answers = RandomizeAnswers(new List<Answer>(currentQuestion.answers));

        // setupAnswerButtons
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].SetIsCorrrect(false);
            answerButtons[i].SetAnswerText(answers[i].answerText);
            if (answers[i]._answerSprite)
            {
                answerButtons[i].SetAnswerSprite(answers[i]._answerSprite);
            }
            if (answers[i].IsCorrect)
            {
                answerButtons[i].SetIsCorrrect(true);
            }
        }

    }
    List<Answer> RandomizeAnswers(List<Answer> originalList)
    {
       
        List<Answer> newList = new List<Answer>();
        if (answerButtons != null)
        {
            for (int i = 0; i < answerButtons.Length; i++)
            {
                int random = UnityEngine.Random.Range(0, originalList.Count);
                newList.Add(originalList[random]);
                originalList.RemoveAt(random);
            }
        }
        return newList;

    }

    int tryCounts = 3,tries  = 0;

    void UpdateQuestion(bool showNewQuestion)
    {

        if (questions.Questions.Count < 0) return;

        // Update Questions
        if (showNewQuestion)
        {
            bool isLastQuestion = UpdateQuestionNumber();

            if (isLastQuestion)
            {
                return;
            }



            SelectNewQuestion();
        }
        if (currentQuestion.Layout == Layout.Horizontal)
        {
            answerButtons = answerButtonsHorizontal;
            horizontalLayout.SetActive(true);
            verticalLayout.SetActive(false);
            noneLayout.SetActive(false);
        }
        else if (currentQuestion.Layout == Layout.Vertical)
        {
            answerButtons = answerButtonsVertical;
            horizontalLayout.SetActive(false);
            verticalLayout.SetActive(true);
            noneLayout.SetActive(false);
        }
        else if(currentQuestion.Layout == Layout.None)
        {
            horizontalLayout.SetActive(false);
            verticalLayout.SetActive(false);
            noneLayout.SetActive(true);
        }

        SetQuestionValues();

        if (currentQuestion.Layout != Layout.None)
        {
            SetAnswerValues();

            ResetAnswerVisual();
        }

        Togglebuttons(true, currentQuestion.Layout);
    }

    void onQuizFinished()
    {
        OnQuizFinishedEvent?.Invoke();
    }

    public void OnsubmitButtonClicked()
    {
        StartCoroutine(OnsubmitButtonClickedCoroutine());
    }

    IEnumerator OnsubmitButtonClickedCoroutine()
    {
        Togglebuttons(false, currentQuestion.Layout);

        if (currentQuestion.Layout == Layout.None) yield break;

        bool allAnswersCorrect = answerButtons.Any(button => button.IsRightAnswer());

        // Update Score 
        if (allAnswersCorrect)
        {
            ScoreManager.Score += 1 - tries/2f;

            Score.text = "Score: " + ScoreManager.Score;
            SoundManager.Instance.PlayNarration(CorrectAnswer, 0);
               _audioSource.clip = execelent;
               _audioSource.Play();

            CheckAnswerVisual();

            tries = 0;
        }
        else //wrong answer
        {
            tries++;

            SoundManager.Instance.PlayNarration(wrongAnswer, 0);
                
            if (tries == tryCounts)
            {
                CheckAnswerVisual();                
            }
            else
            {
                _audioSource.clip = tryagain;
                _audioSource.Play();
                CheckAnswerVisual(_selectedbutton);
            }
            print("Wrong");
        }

        
        currentQuestion.Answered = true;
        yield return new WaitForSeconds(2);

        if (allAnswersCorrect)
        {
            UpdateQuestion(true);
        }
        else {
            if (tries == tryCounts )
            {
              UpdateQuestion(true);
                tries = 0;
            }
            else
            {
                UpdateQuestion(false);
            }    
        }

        yield return new WaitForSeconds(1);
    }

    //public async void OnsubmitButtonClicked()
    //{
    //    Togglebuttons(false, currentQuestion.Layout);

    //    if (currentQuestion.Layout == Layout.None) return;

    //    bool allAnswersCorrect = answerButtons.Any(button => button.IsRightAnswer());

    //    // Update Score 
    //    if (allAnswersCorrect)
    //    {
    //        ScoreManager.Score++;
    //        Score.text = "Score: " + ScoreManager.Score;
    //        SoundManager.Instance.PlayNarration(CorrectAnswer, 0);
    //        //    _audioSource.clip = CorrectAnswer;
    //        //    _audioSource.Play();
    //    }
    //    else
    //    {
    //        SoundManager.Instance.PlayNarration(wrongAnswer, 0);
    //        //    _audioSource.clip = wrongAnswer;
    //        //    _audioSource.Play();
    //        print("Wrong");
    //    }

    //    CheckAnswerVisual();
    //    currentQuestion.Answered = true;
    //    await Task.Delay(2000);
    //    UpdateQuestion();
    //    await Task.Delay(1000);

    //}

    public void ChooseCorrect()
    {
        StartCoroutine(ChooseCorrectCoroutine());
    }

    IEnumerator ChooseCorrectCoroutine()
    {
        Togglebuttons(false, currentQuestion.Layout);
        ScoreManager.Score++;
        Score.text = "Score: " + ScoreManager.Score;
        SoundManager.Instance.PlayNarration(CorrectAnswer, 0);
        currentQuestion.Answered = true;
        yield return new WaitForSeconds(2.5f);
        UpdateQuestion(true);
        yield return new WaitForSeconds(1);
    }

    //public async void ChooseCorrect()
    //{
    //    Togglebuttons(false, currentQuestion.Layout);
    //    ScoreManager.Score++;
    //    Score.text = "Score: " + ScoreManager.Score;
    //    SoundManager.Instance.PlayNarration(CorrectAnswer, 0);
    //    currentQuestion.Answered = true;
    //    await Task.Delay(2000);
    //    UpdateQuestion();
    //    await Task.Delay(1000);
    //}

    public void ChooseWrong()
    {
        StartCoroutine(ChooseWrongCoroutine());
    }

    IEnumerator ChooseWrongCoroutine()
    {
        Togglebuttons(false, currentQuestion.Layout);
        SoundManager.Instance.PlayNarration(wrongAnswer, 0);
        currentQuestion.Answered = true;
        yield return new WaitForSeconds(2.5f);
        UpdateQuestion(true);
        yield return new WaitForSeconds(1);
    }

    //public async void ChooseWrong()
    //{
    //    Togglebuttons(false, currentQuestion.Layout);
    //    SoundManager.Instance.PlayNarration(wrongAnswer, 0);
    //    currentQuestion.Answered = true;
    //    await Task.Delay(2000);
    //    UpdateQuestion();
    //    await Task.Delay(1000);
    //}


    void SetupTimer() {

        elpasedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elpasedTime / 60);
        int seconds = Mathf.FloorToInt(elpasedTime % 60);
        Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }


    bool UpdateQuestionNumber() {

        print("UpdateQuestionNumber");
        currentQuestionIndex++;
        if (currentQuestionIndex > ALlQuestionNumber) {
            onQuizFinished();
            print("onQuizFinished");
            return true;
        }  

        print("Update Question Number");
        questionNumber.text =    currentQuestionIndex + "/" + ALlQuestionNumber;
        return false;
    }

    void CheckAnswerVisual(AnswerButton answerButton=null)
    {
        if (answerButton == null)
        {
            foreach (var btn in answerButtons)
            {
                btn.OnVisualupdate?.Invoke();
            }
        }
        else{
            answerButton.OnVisualupdate?.Invoke();
        }
    }

    void ResetAnswerVisual()
    {
        foreach (var btn in answerButtons)
        {
            btn.OnResetVisual?.Invoke();
            btn.Deselect();
            
        }

    }


    public  void Shuffle<T>(IList<T> list)
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

    private void Togglebuttons(bool state, Layout layout)
    {
        if (layout == Layout.Horizontal)
        {
            foreach (var item in answerButtonsHorizontal)
            {
                item.GetComponent<UnityEngine.UI.Button>().interactable = state;
            }
        }
        else if (layout == Layout.Vertical)
        {
            foreach (var item in answerButtonsVertical)
            {
                item.GetComponent<UnityEngine.UI.Button>().interactable = state;
            }
        }
        else if (layout == Layout.None)
        {
            foreach (var item in answerButtonsNone)
            {
                item.Enabled = state;
            }
        }
    }


    #endregion


    #region Public Function

    AnswerButton _selectedbutton;

    public void SelectButton(AnswerButton _selectedbutton) {
        if (_selectedbutton != null)
            Array.ForEach(answerButtons, button => button.Deselect());

        _selectedbutton.Select();

        this._selectedbutton = _selectedbutton;
    }


#endregion
}
