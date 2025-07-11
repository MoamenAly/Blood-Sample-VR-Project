using AYellowpaper;
using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Step {

    [Space][GUIColor(0.55f, 0.95f, 0.55f)]

#if UNITY_EDITOR
    [OnValueChanged(nameof(OnValueChanged))]
#endif

    [DynamicTextArea][SerializeField] internal string stepDescription;

    [Space][SerializeField] List<InterfaceReference<IDepndency>> depndencies;

    float delayInSeconds = 0.5f;

    [Space(3)]
    [GUIColor(1.0f, 0.9373f, 0.8353f)]
    [SerializeField] UnityEvent2 Do;

#if UNITY_EDITOR

    private void OnValueChanged()
    {
        // Mark the parent object as dirty to force an inspector update
        var parents = GetParentObject();
        if (parents != null)
        {
            for (int i = 0; i < parents.Length; i++)
            {
                var parent = parents[i];
#if UNITY_EDITOR
                Debug.Log(parent.name + " - " + stepDescription);
                EditorUtility.SetDirty(parent);              
#endif
            }
        }
    }

#endif



    private StepsManager[] GetParentObject()
    {
        // Find the parent object containing this step
        return GameObject.FindObjectsOfType<StepsManager>(true); // Adjust this based on your actual parent object retrieval logic
    }

    internal bool IsReady()
    {
        for (int i = 0; i < depndencies.Count; i++)
        {
            if (!depndencies[i].Value.IsCompleted()) {
                return false;
            }
        }
        return true;
    }

    internal void Execute()
    {
        DOVirtual.DelayedCall(delayInSeconds, () => {
            Do?.Invoke();
        });
    }




}