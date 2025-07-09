using UnityEngine;
using UnityEngine.Events;

public class MouseMotionInteraction : WebGL_MouseIteraction
{
    private int completedMotions = 0;
    
    [SerializeField] int motionCount = 1;
    [SerializeField] UnityEvent onComplete;

    public override void OnMouseInteract()
    {
        if (grabbable == null || !grabbable.enabled) return;

        completedMotions = 0;

        FireGrabEvent();

        StartNextMotion();
    }

    private void StartNextMotion()
    {
        // Only start the next motion if we haven't reached the motionCount yet
        if (completedMotions < motionCount)
        {
            MotionController motionController = grabbable.GetComponent<MotionController>();
            motionController.StartMotion(OnMotionDone,motionCount,completedMotions);
        }
    }

    private void OnMotionDone()
    {
        completedMotions++;

        if (completedMotions >= motionCount)
        {
            onComplete.Invoke();
            grabbable.enabled = false;
            FireRelaseEvent();
        }
        else
        {
            StartNextMotion();
        }
    }

    public override void OnRestInteract()
    {
        throw new System.NotImplementedException();
    }
}
