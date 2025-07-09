using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scivr.QuestionSystem
{
    public class AnswerComponent : MonoBehaviour
    {
        public TMP_Text answerText;
        public Color defaultColor;
        public Color correctAnswerColor;
        private Answer answer;

        private void Start()
        {
            defaultColor = answerText.color;
        }

        public void BindAnswer(Answer answer)
        {
            this.answer = answer;
            answerText.text = answer.text;
            answerText.color = defaultColor;
        }

        public void OnAnswerSelected()
        {
            QuizManager.Instance.HandleAnswer(answer);
        }

        public void ShowCorrectAnswer(bool isCorrect)
        {
            if (isCorrect)
            {
                answerText.color = correctAnswerColor;
            }
        }
    }
}
