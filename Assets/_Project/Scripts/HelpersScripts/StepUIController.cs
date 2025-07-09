using RTLTMPro;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class StepUIController : MonoBehaviour
{
    [SerializeField] public StapData[] _stepDataEn;
    [SerializeField] RTLTextMeshPro _stepTitle;
    [SerializeField] RTLTextMeshPro _stepDescription;
    [SerializeField] AudioManager _narration;

    [SerializeField] int _delay;

    [SerializeField] UnityEvent OnFinishedSteps;
    [SerializeField] int currentIndexOfStep;

    [UnityEngine.ContextMenu("OnShowFirstStep")]
    public void ShowFirstStep()
    {
        _stepTitle.text = _stepDataEn[currentIndexOfStep].Title;
        _stepDescription.text = _stepDataEn[currentIndexOfStep].Description;
        _narration.Play(_stepDataEn[currentIndexOfStep].NarrationId);
    }

   // [UnityEngine.("OnNextStep")]
    [Button("OnNextStep")]
    public void NextStep()
    {
        currentIndexOfStep++;
        GenerateStep();
    }

    private async void GenerateStep()
    {
        await Task.Delay(_delay * 1000);
        if (currentIndexOfStep <= _stepDataEn.Length)
        {
            // Exist Step
            _stepTitle.text = _stepDataEn[currentIndexOfStep].Title;
            _stepDescription.text = _stepDataEn[currentIndexOfStep].Description;
            _narration.Play(_stepDataEn[currentIndexOfStep].NarrationId);
        }
        else
        {
            // Not exist Step
            OnFinishedSteps.Invoke();
        }
    }
}

