using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;
using BNG;

public class Sphygmomanometer : MonoBehaviour
{
    [Header("References")]
    public CustomGrabbable pumpGrabbable;               // Pump object to grap
    public Transform pump;               // Pump object to scale
    public Transform pointer;            // Pointer needle
    public AudioClip pumpSound;        // Sound when pumping
    public AudioClip releaseSound;     // Sound when releasing
    public AudioSource SfxSound;

    [Header("Settings")]
    public float scaleDelta = 0.2f;      // How much the pump shrinks
    public float scaleSpeed = 5f;        // How fast it scales
    public float pointerStep = 8f;       // Rotation step per click
    public float maxRotation = 120f;     // Maximum rotation before stop
    public float releaseSpeed = 60f;     // Rotation speed when releasing

    private Vector3 originalScale;
    private float currentRotation = 0f;
    private bool isReleasing = false;
    private bool canClick = true;

    void Start()
    {
        if (pump != null)
            originalScale = pump.localScale;
    }

    void Update()
    {
        if(pumpGrabbable.BeingHeld)
        {
            var primaryGrabber = pumpGrabbable.GetPrimaryGrabber();
            if (primaryGrabber.HandSide == ControllerHand.Right && InputBridge.Instance.RightTrigger > 0 && canClick)
            {
                StartCoroutine(DoPumpAction());
            }

            else if (primaryGrabber.HandSide == ControllerHand.Left && InputBridge.Instance.LeftTrigger > 0 && canClick)
            {
                StartCoroutine(DoPumpAction());
            }
        }



        if (isReleasing)
        {
            float step = releaseSpeed * Time.deltaTime;
            currentRotation = Mathf.MoveTowards(currentRotation, 0, step);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);

            // Stop releasing when back to zero
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

        // Play pump sound
        if (pumpSound != null)
            SfxSound.PlayOneShot(pumpSound);

        // Scale down
        Vector3 targetScale = originalScale * (1f - scaleDelta);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            pump.localScale = Vector3.Lerp(originalScale, targetScale, t);
            yield return null;
        }

        // Scale up
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * scaleSpeed;
            pump.localScale = Vector3.Lerp(targetScale, originalScale, t);
            yield return null;
        }

        // Rotate pointer
        if (pointer != null)
        {
            currentRotation += pointerStep;
            currentRotation = Mathf.Clamp(currentRotation, 0, maxRotation);
            pointer.localRotation = Quaternion.Euler(0, currentRotation, 0);
        }

        // If reached maximum, start release
        if (Mathf.Approximately(currentRotation, maxRotation))
        {
            yield return new WaitForSeconds(0.3f);
            if (releaseSound != null)
                SfxSound.PlayOneShot(releaseSound);

            isReleasing = true;
        }
        else
        {
            canClick = true;
        }
    }
}
