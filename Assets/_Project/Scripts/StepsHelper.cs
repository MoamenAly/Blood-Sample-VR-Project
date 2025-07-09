using BNG;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StepHolder
{
    public int id;
    public Narrations narrationId;
    public Language titelTextId;
    public Language descriptionTextId;
    //public List<ComponentsNames> components;
    //public List<SnapZonesNames> snapZones;
    //[Space]
    public UnityEvent2 OnStepStart;
    public UnityEvent2 OnStepEnd;
}

public class StepsHelper : MonoBehaviour
{
    public AudioManager audioManager;

    public Theme_Text titleTheme_Text;
    public Theme_Text descriptionTheme_Text;

    public List<StepHolder> steps;

    StepHolder currentStepHolder;

    //PropsHolder props;

    private void OnValidate()
    {
        for (int i = 0; i < steps.Count; i++)
        {
            steps[i].id = i + 1;
            if (Enum.TryParse<Narrations>("Step" + steps[i].id, out Narrations narrationId))
            {
                steps[i].narrationId = narrationId;
            }
            if (Enum.TryParse<Language>("Step" + steps[i].id, out Language titelTextId))
            {
                steps[i].titelTextId = titelTextId;
            }
            if (Enum.TryParse<Language>("Step" + steps[i].id + "_Txt", out Language descriptionTextId))
            {
                steps[i].descriptionTextId = descriptionTextId;
            }

            if (Enum.TryParse<Language>("Step" + steps[i].id + "Txt", out Language descriptionTextId1))
            {
                steps[i].descriptionTextId = descriptionTextId1;
            }

        }
    }

    private void Start()
    {
        //props = GetComponent<PropsHolder>();

        SetAllComponentsState();
    }

    private void SetAllComponentsState()
    {
        //props.components.ForEach(component =>
        //{
        //    if (component.componentOutline != null) component.componentOutline.enabled = false;
        //    if (component.componentGrab != null) component.componentGrab.enabled = false;
        //    //if (component.componentCol != null) component.componentCol.enabled = false;
        //    //if (component.componentRb != null) component.componentRb.isKinematic = true;
        //});
        //props.snapZones.ForEach(snapZone =>
        //{
        //    //if (snapZone.grab != null) snapZone.grab.enabled = false;
        //    if (snapZone.col != null) snapZone.col.enabled = false;
        //});
    }

    public void SelectId(int id)
    {
        if (id > 1)
        {
            SetPropsState(false);

            currentStepHolder.OnStepEnd?.Invoke();
        }

        currentStepHolder = steps.Find(s => s.id == id);

        if (currentStepHolder != null)
        {
            audioManager.Play(currentStepHolder.narrationId);
            titleTheme_Text._SetTextID(currentStepHolder.titelTextId);
            descriptionTheme_Text._SetTextID(currentStepHolder.descriptionTextId);

            SetPropsState(true);

            currentStepHolder.OnStepStart?.Invoke();
        }
        else
        {
            Debug.LogWarning("Step with the specified ID not found.");
        }
    }

    void SetPropsState(bool state)
    {
        //if (currentStepHolder.components != null && currentStepHolder.components.Count > 0)
        //{
        //    foreach (var componentName in currentStepHolder.components)
        //    {
        //        var component = props.components.FirstOrDefault(c => c.name == componentName.ToString());
        //        if (component != null)
        //        {
        //            if (component.componentOutline != null) component.componentOutline.enabled = state;
        //            if (component.componentGrab != null) component.componentGrab.enabled = state;
        //            //if (component.componentCol != null) component.componentCol.enabled = state;
        //            //if (component.componentRb != null) component.componentRb.isKinematic = !state;
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"Component with name {componentName} not found.");
        //        }
        //    }
        }

        //if (currentStepHolder.snapZones != null && currentStepHolder.snapZones.Count > 0)
        //{
        //    foreach (var snapZoneName in currentStepHolder.snapZones)
        //    {
        //        var snapZone = props.snapZones.FirstOrDefault(c => c.name == snapZoneName.ToString());
        //        if (snapZone != null)
        //        {
        //            //if (snapZone.grab != null) snapZone.grab.enabled = state;
        //            if (snapZone.col != null) snapZone.col.enabled = state;
        //        }
        //        else
        //        {
        //            Debug.LogWarning($"SnapZone with name {snapZoneName} not found.");
        //        }
        //    }
        //}
   // }
}
