using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using BNG;
using TMPro;
using Slider = UnityEngine.UI.Slider;

public class TubeRotationTracker : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField] private int totalRotationTarget = 3;
    [SerializeField] private float rotationThreshold = 150f;
    [SerializeField] private float returnThreshold = 30f;
    [SerializeField] private float maxRotationSpeed = 10f; // degrees per second

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI rotationIndexText;
    [SerializeField] private Slider progressSlider;

    [Header("Events")]
    public UnityEvent2 OnRotationFinished;

    private Grabbable grabbable;
    private bool isGrabbed;
    private bool reached180 = false;

    private float initialY;
    private float previousY;
    private int rotationIndex = 0;
    private bool finished = false;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        initialY = transform.eulerAngles.y;
        previousY = initialY;

        UpdateUI();

        if (progressSlider != null)
        {
            progressSlider.maxValue = totalRotationTarget;
        }
    }

    void Update()
    {
        if (grabbable != null)
        {
            isGrabbed = grabbable.BeingHeld;
        }

        if (!isGrabbed || finished) return;

        // Calculate rotation speed first
        float currentY = transform.eulerAngles.y;
        float delta = Mathf.DeltaAngle(previousY, currentY);
        float rotationSpeed = Mathf.Abs(delta) / Time.deltaTime;
        previousY = currentY;

        // Ignore tracking if rotating too fast
        if (rotationSpeed > maxRotationSpeed)
        {
            return;
        }

        TrackRotation();
    }

    void TrackRotation()
    {
        float currentY = Mathf.DeltaAngle(initialY, transform.eulerAngles.y);
        float absY = Mathf.Abs(currentY);

        if (!reached180 && absY >= rotationThreshold)
        {
            reached180 = true;
            Debug.Log("Reached 180 degrees");
        }

        if (reached180 && absY <= returnThreshold)
        {
            rotationIndex++;
            reached180 = false;

            if (rotationIndex >= totalRotationTarget)
            {
                rotationIndex = totalRotationTarget;
                finished = true;
                Debug.Log("Rotation finished.");
                OnRotationFinished?.Invoke();
            }

            UpdateUI();
        }
    }

    void UpdateUI()
    {
        if (rotationIndexText != null)
        {
            rotationIndexText.text = rotationIndex + "/" + totalRotationTarget;
        }

        if (progressSlider != null)
        {
            progressSlider.value = rotationIndex;
        }
    }
}
