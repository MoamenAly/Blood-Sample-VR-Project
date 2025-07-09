using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    public string SceneName = "New chemistry";

    [SerializeField]
    List<Step> steps = new List<Step>();
    [SerializeField] int stepIndex = 0;

    //may use it later for undo
    private Step prevuiosStep;

    private void OnValidate()
    {
        if (transform.parent != null)
        {
            transform.parent.SetAsFirstSibling();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Skip();
        }
    }

    private Step CreateNewItem()
    {
        return new Step();
    }

    private void FixedUpdate()
    {
        if (stepIndex < steps.Count)
        {
            var step = steps[stepIndex];
            if (step.IsReady())
            {
                prevuiosStep = step;
                step.Execute();
                stepIndex++;
            }
        }
    }

    [Button]
    public void Skip()
    {
        if (stepIndex < steps.Count)
        {
            var step = steps[stepIndex];

            prevuiosStep = step;
            step.Execute();
            stepIndex++;
        }
    }



}


