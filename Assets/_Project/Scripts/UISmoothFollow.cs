using UnityEngine;

public class UISmoothFollow : MonoBehaviour
{
    public Camera targetCamera;             // Camera to follow
    public Vector3 offset = new Vector3(0, 0, 2f);  // Offset from the camera
    public float smoothTime = 0.2f;         // Smooth follow time

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (targetCamera == null) return;

        // Desired position based on camera's position + offset in camera space
        Vector3 desiredPosition = targetCamera.transform.position +
                                  targetCamera.transform.rotation * offset;

        // Smoothly move UI to desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // Optional: make UI face the camera
        //transform.rotation = Quaternion.LookRotation(transform.position - targetCamera.transform.position);
    }
}
