using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectClickInteraction : MonoBehaviour
{
    public List<UnityEvent2> ClickActions; // List of actions to perform in sequence
    public int currentActionIndex = 0; // Index to keep track of the current action

    public bool _isUseIncreaseIndex = false;
    public bool _canClick = true;

    public bool CanClick
    {
        get { return _canClick; }
        set { _canClick = value; }
    }

    private void Start()
    {
        // Initialize the list if it's not already initialized in the inspector
        if (ClickActions == null)
        {
            ClickActions = new List<UnityEvent2>();
        }
    }

    private void OnMouseDown()
    {
        if (!CanClick) return;

        print("OnMouseDown");
        PerformNextAction();
    }

    private void PerformNextAction()
    {
        if (currentActionIndex < ClickActions.Count)
        {
            var action = ClickActions[currentActionIndex];
            if (action != null)
            {
                action.Invoke();
            }

            if (_isUseIncreaseIndex)
            {
                currentActionIndex++;
            }

            CanClick = false;
        }
    }
}
