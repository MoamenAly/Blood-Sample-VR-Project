using UnityEngine;
using BNG;

public class TubeRotationTracker : MonoBehaviour
{
    private Grabbable grabbable;
    private bool isGrabbed;

    private float rotationThreshold = 150f; // Degree threshold to detect 180
    private float returnThreshold = 30f;    // Degree threshold to detect return to 0

    private bool reached180 = false;
    private int fullRotations = 0;

    private float initialY;

    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        initialY = transform.eulerAngles.y;
    }

    void Update()
    {
        if (grabbable != null)
        {
            isGrabbed = grabbable.BeingHeld;
        }

        if (isGrabbed)
        {
            TrackRotation();
        }
    }

    void TrackRotation()
    {
        float currentY = Mathf.DeltaAngle(initialY, transform.eulerAngles.y);
        float absY = Mathf.Abs(currentY);

        // Went past 180 (or close enough)
        if (!reached180 && absY >= rotationThreshold)
        {
            reached180 = true;
            Debug.Log("Reached 180 degrees");
        }

        // Came back to ~0
        if (reached180 && absY <= returnThreshold)
        {
            fullRotations++;
            reached180 = false;
            Debug.Log("Returned to start, full rotations = " + fullRotations);
        }
    }
}
