using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Scivr.QuestionSystem
{
    public class QuizManager : MonoSinglton<QuizManager>
    {
        public List<BaseQuestion> questions;

        private int currentQuestionIndex;
        private int score;
        private int attempts;
        private int answersGiven;
        private float timer;
        private bool isQuizActive;
        private bool canAnswer;

        public int maxAttempts = 3;
        public int maxAnswersPerQuestion = 2;

        public TMP_Text questionText;
        public TMP_Text timerText;
        public TMP_Text scoreText;
        public TMP_Text attemptsText;
        public AnswerComponent[] answerComponents;

        public UnityEvent onCorrectAnswer;
        public UnityEvent onIncorrectAnswer;
        public UnityEvent onQuizEnd;

        void Update()
        {
            if (isQuizActive)
            {
                timer += Time.deltaTime;
                timerText.text = "Time: " + Mathf.Round(timer).ToString();
            }
        }

        public void StartQuiz()
        {
            currentQuestionIndex = 0;
            score = 0;
            attempts = 0;
            answersGiven = 0;
            isQuizActive = true;
            LoadQuestion();
        }

        public void LoadQuestion()
        {
            if (currentQuestionIndex < questions.Count)
            {
                BaseQuestion question = questions[currentQuestionIndex];
                questionText.text = question.questionText;
                List<Answer> answers = question.GetAnswers();

                for (int i = 0; i < answerComponents.Length; i++)
                {
                    if (i < answers.Count)
                    {
                        answerComponents[i].BindAnswer(answers[i]);
                        answerComponents[i].gameObject.SetActive(true);
                    }
                    else
                    {
                        answerComponents[i].gameObject.SetActive(false);
                    }
                }

                question.ShowQuestion(); // Call the method to invoke the Unity event
                canAnswer = true;
                timer = 0;
                answersGiven = 0; // Reset answers given for the new question
            }
            else
            {
                EndQuiz();
            }
        }

        public void HandleAnswer(Answer answer)
        {
            if (!isQuizActive || !canAnswer) return;

            canAnswer = false;
            answersGiven++;

            if (answer.isCorrectAnswer)
            {
                score += 1;
                onCorrectAnswer?.Invoke();
                StartCoroutine(NextQuestionWithDelay(2f)); // Wait for 2 seconds before loading the next question
            }
            else
            {
                onIncorrectAnswer?.Invoke();

                if (answersGiven >= maxAnswersPerQuestion)
                {
                    attempts++;
                    ShowCorrectAnswer();
                    StartCoroutine(NextQuestionWithDelay(2f)); // Wait for 2 seconds before loading the next question
                }
                else
                {
                    canAnswer = true; // Allow another attempt for the same question
                }
            }

            scoreText.text = "Score: " + score;
            attemptsText.text = "Attempts: " + attempts;
        }

        private void ShowCorrectAnswer()
        {
            BaseQuestion question = questions[currentQuestionIndex];
            List<Answer> answers = question.answers;

            for (int i = 0; i < answerComponents.Length; i++)
            {
                if (i < answers.Count)
                {
                    answerComponents[i].ShowCorrectAnswer(answers[i].isCorrectAnswer);
                }
            }
        }

        private IEnumerator NextQuestionWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            currentQuestionIndex++;
            LoadQuestion();
        }

        private void EndQuiz()
        {
            isQuizActive = false;
            questionText.text = "Quiz Over!";
            onQuizEnd?.Invoke();
        }
    }
}
