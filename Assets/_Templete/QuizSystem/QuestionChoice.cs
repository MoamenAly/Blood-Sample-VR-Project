using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuizSystem
{
    public class QuestionChoice : MonoBehaviour
    {
        public AnswerEnum answer;
        private ChoiceSnapManager snipManager;

        private void Start()
        {
            snipManager = FindObjectOfType<ChoiceSnapManager>(true);
        }

#if UNITY_WEBGL || UNITY_STANDALONE_WIN
        private void OnMouseDown()
        {  
                CustomGrabbable Canbegrabbed = GetComponent<CustomGrabbable>();
                if (Canbegrabbed != null)
                {
                    snipManager.SetCurrentChoice(this);
                }
        }
#endif
    }

}

