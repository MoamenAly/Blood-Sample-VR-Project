using UnityEngine;
using System.Collections;
using BNG;

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

    [Header("Settings")]
    public float scaleDelta = 0.2f;
    public float scaleSpeed = 5f;
    public float pointerStep = 8f;
    public float maxRotation = 120f;
    public float releaseSpeed = 60f; 
    public float passiveDecaySpeed = 15f; 

    private Vector3 originalScale;
    private float currentRotation = 0f;
    private bool isReleasing = false;
    private bool canClick = true;
    private bool isPumping = false;
    public bool isWantSoundRelease;

    void Start()
    {
        if (pump != null)
            originalScale = pump.localScale;
    }

    void Update()
    {
        isPumping = false;

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

        if (!isPumping && !isReleasing && currentRotation > 0)
        {
            currentRotation = Mathf.MoveTowards(currentRotation, 0, passiveDecaySpeed * Time.deltaTime);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);
        }

        if (isReleasing)
        {
            float step = releaseSpeed * Time.deltaTime;
            currentRotation = Mathf.MoveTowards(currentRotation, 0, step);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);

            if (Mathf.Approximately(currentRotation, 0))
            {
                isReleasing = false;
                canClick = true;
            }
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
        }
        else
        {
            canClick = true;
        }
    }
}
