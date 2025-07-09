using System;
using System.Collections;
using UnityEngine;

public class MotionController : MonoBehaviour
{
    public MotionData motionData; // Assign in the inspector
    private GameObject targetObject; // Assign the object to move
    public float duration = 5f; // Duration from start to end frame
    bool isPlay = false;
    public bool returnToStartPosition = false; // Flag to indicate whether to return to start position

    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 startScale;


    private void Start()
    {
        targetObject = gameObject;
    }

    public void SetMotion(MotionData motionData)
    {
        this.motionData = motionData;
    }

    public void StartMotion()
    {
        int currentCycle = 0;
        int cycleCount = 0;



        if (!isPlay && motionData != null && motionData.motionFrames.Count > 0 && targetObject != null)
        {
            isPlay = true;

            if (currentCycle == 0)
            {
                startPosition = targetObject.transform.position;
                startRotation = targetObject.transform.rotation;
                startScale = targetObject.transform.localScale;
            }

            StartCoroutine(ApplyMotion(() => { }, cycleCount, currentCycle));
        }
    }

    public void StartMotion(Action OnComplete, int cycleCount = 0, int currentCycle = 0)
    {
        if (!isPlay && motionData != null && motionData.motionFrames.Count > 0 && targetObject != null)
        {
            isPlay = true;

            if (currentCycle == 0)
            {
                startPosition = targetObject.transform.position;
                startRotation = targetObject.transform.rotation;
                startScale = targetObject.transform.localScale;
            }

            StartCoroutine(ApplyMotion(OnComplete, cycleCount, currentCycle));
        }
    }

    private IEnumerator ApplyMotion(Action onComplete, int totolaCycle, int currentCycle)
    {
        float time = 0;
        int frameIndex = 0;
        MotionFrame currentFrame = motionData.motionFrames[frameIndex];
        MotionFrame nextFrame = motionData.motionFrames[frameIndex + 1];

        while (frameIndex < motionData.motionFrames.Count - 1)
        {
            // Interpolate position, rotation, and scale
            targetObject.transform.position = Vector3.Lerp(currentFrame.position, nextFrame.position, time / duration);
            targetObject.transform.rotation = Quaternion.Lerp(Quaternion.Euler(currentFrame.eulerAngles), Quaternion.Euler(nextFrame.eulerAngles), time / duration);
            targetObject.transform.localScale = Vector3.Lerp(currentFrame.scale, nextFrame.scale, time / duration);

            time += Time.deltaTime;
            if (time >= duration)
            {
                // Move to the next frame
                frameIndex++;
                if (frameIndex < motionData.motionFrames.Count - 1)
                {
                    currentFrame = motionData.motionFrames[frameIndex];
                    nextFrame = motionData.motionFrames[frameIndex + 1];
                    time = 0;
                }
            }

            yield return null;
        }

        // Ensure the final frame is applied
        MotionFrame finalFrame = motionData.motionFrames[motionData.motionFrames.Count - 1];
        targetObject.transform.position = finalFrame.position;
        targetObject.transform.rotation = Quaternion.Euler(finalFrame.eulerAngles);
        targetObject.transform.localScale = finalFrame.scale;
        isPlay = false;

        Debug.Log("cycle " + currentCycle);
        // Return to start position, rotation, and scale if required
        if (returnToStartPosition && currentCycle == totolaCycle - 1)
        {
            targetObject.transform.position = startPosition;
            targetObject.transform.rotation = startRotation;
            targetObject.transform.localScale = startScale;
        }

        onComplete?.Invoke();

    }
}
