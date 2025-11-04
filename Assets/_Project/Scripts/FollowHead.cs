using UnityEngine;

public class FollowHead : MonoBehaviour
{
    public Transform playerHead; // Assign the player head transform in Inspector
    public Vector3 offset = Vector3.zero; // Optional offset from head position
    public float smoothSpeed = 10f; // How smoothly the object follows

    void LateUpdate()
    {
        if (playerHead == null) return;

        // Target position based on head + offset
        Vector3 targetPosition = playerHead.position + offset;

        // Smoothly move to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // Optional: make object match head rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, playerHead.rotation, smoothSpeed * Time.deltaTime);
    }
}
