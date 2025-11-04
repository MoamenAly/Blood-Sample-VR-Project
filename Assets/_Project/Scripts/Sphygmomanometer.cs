using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events; // <-- Needed for UnityEvent
using BNG;
using Sirenix.OdinInspector;

public class Sphygmomanometer : MonoBehaviour
{
    [Header("References")]
    public CustomGrabbable pumpGrabbable;
    public Transform pump;
    public Transform pointer;
    public SkinnedMeshRenderer cuff;
    public AudioClip pumpSound;
    public AudioClip releaseSound;
    public AudioSource SfxSound;

    [Header("Heartbeat Settings")]
    public AudioSource heartbeatAudio;
    public bool isStethoscopePlaced = false;

    [Header("Settings")]
    public float scaleDelta = 0.2f;
    public float scaleSpeed = 5f;
    public float pointerStep = 8f;
    public float maxRotation = 120f;
    private float releaseSpeed = 60f;
    public float passiveDecaySpeed = 15f;

    [Tooltip("Events that will trigger one by one when the cuff is fully released")]
    public List<UnityEvent2> onFullReleaseEvents = new List<UnityEvent2>();

    [Tooltip("Delay between each event firing (seconds)")]
    public float delayBetweenReleaseEvents = 0.3f;

    private Vector3 originalScale;
    private float currentRotation = 0f;
    private bool isReleasing = false;
    private bool canClick = true;
    private bool isPumping = false;
    public bool isWantSoundRelease;

    private int cuffBlendShapeIndex = 0;
    private bool hasTriggeredReleaseSequence = false;

    public float ReleaseSpeed { get => releaseSpeed; set => releaseSpeed = value; }

    void Start()
    {
        if (pump != null)
            originalScale = pump.localScale;

    }

    void Update()
    {
        isPumping = false;

        // --- Handle pumping input ---
        if (pumpGrabbable.BeingHeld)
        {
            var primaryGrabber = pumpGrabbable.GetPrimaryGrabber();

            if (primaryGrabber != null)
            {
                bool triggerPressed =
                    (primaryGrabber.HandSide == ControllerHand.Right && InputBridge.Instance.RightTrigger > 0) ||
                    (primaryGrabber.HandSide == ControllerHand.Left && InputBridge.Instance.LeftTrigger > 0);

                if (triggerPressed && canClick)
                {
                    isPumping = true;
                    StartCoroutine(DoPumpAction());
                }
            }
        }

        // --- 🎧 Handle heartbeat sound ---
        HandleHeartbeatSound();

        // --- Handle decay and release ---
        if (!isPumping && !isReleasing && currentRotation > 0)
        {
            currentRotation = Mathf.MoveTowards(currentRotation, 0, passiveDecaySpeed * Time.deltaTime);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);
        }

        if (isReleasing)
        {
            float step = ReleaseSpeed * Time.deltaTime;
            currentRotation = Mathf.MoveTowards(currentRotation, 0, step);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);

            if (Mathf.Approximately(currentRotation, 0))
            {
                isReleasing = false;
                canClick = true;

                if (!hasTriggeredReleaseSequence)
                {
                    hasTriggeredReleaseSequence = true;
                    StartCoroutine(PlayReleaseEventSequence());
                }
            }
        }

        // --- Update cuff blend shape based on pointer rotation ---
        UpdateCuffBlendShape();
    }

    private void HandleHeartbeatSound()
    {
        if (isStethoscopePlaced && pumpGrabbable.BeingHeld)
        {
            if (heartbeatAudio != null && !heartbeatAudio.isPlaying)
            {
                Debug.LogError("isPumpingandPlaced");
                heartbeatAudio.Play();
            }
        }
        else
        {
            if (heartbeatAudio != null && heartbeatAudio.isPlaying)
            {
                Debug.LogError("isNotPumpingandNotPlaced");
                heartbeatAudio.Stop();
            }
        }
    }

    private void UpdateCuffBlendShape()
    {
        if (cuff != null)
        {
            float blendValue = Mathf.InverseLerp(0, maxRotation, currentRotation) * 100f;
            cuff.SetBlendShapeWeight(cuffBlendShapeIndex, blendValue);
        }
    }

    private IEnumerator DoPumpAction()
    {
        canClick = false;

        if (pumpSound != null)
            SfxSound.PlayOneShot(pumpSound);

        Vector3 targetScale = originalScale * (1f - scaleDelta);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            pump.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            pump.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        if (pointer != null)
        {
            currentRotation += pointerStep;
            currentRotation = Mathf.Clamp(currentRotation, 0, maxRotation);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);
        }

        if (Mathf.Approximately(currentRotation, maxRotation))
        {
            yield return new WaitForSeconds(0.3f);

            if (releaseSound != null && isWantSoundRelease)
                SfxSound.PlayOneShot(releaseSound);

            isReleasing = true;
            hasTriggeredReleaseSequence = false;
        }
        else
        {
            canClick = true;
        }
    }

    // 🔹 Fires each event in the list one by one with delay
    private IEnumerator PlayReleaseEventSequence()
    {
        for (int i = 0; i < onFullReleaseEvents.Count; i++)
        {
            if (onFullReleaseEvents[i] != null)
            {
                onFullReleaseEvents[i].Invoke();
            }

            if (delayBetweenReleaseEvents > 0)
                yield return new WaitForSeconds(delayBetweenReleaseEvents);
        }
    }

    public void SetStethoscopePlaced(bool placed)
    {
        isStethoscopePlaced = placed;
    }
}
