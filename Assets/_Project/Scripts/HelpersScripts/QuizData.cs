using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quiz", menuName = "Quiz/CreatQuizData", order = 1)]
public class QuizData : ScriptableObject
{
    [TextArea(3, 3)] public string questionText;
    [TextArea(2, 2)] public string[] answerTexts;

    public GameObject QuizTools;
    public Narrations NarrationQuizId;
}
