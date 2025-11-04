using UnityEngine;
using System.Collections;

public class MoveCanvasSmoothly : MonoBehaviour
{
    public RectTransform canvasTransform; // World Space Canvas RectTransform
    public Transform target;              // Target Transform
    public float duration = 1f;

    private bool isMoving = false;

    public Transform Target
    {
        get => target;
        set => target = value;
    }

    [ContextMenu("Move Canvas")]
    public void MoveCanvas()
    {
        if (!isMoving && canvasTransform != null && target != null)
            StartCoroutine(MoveToTarget(canvasTransform, target, duration));
    }

    private IEnumerator MoveToTarget(RectTransform rect, Transform target, float time)
    {
        isMoving = true;

        Vector3 startPos = rect.position;
        Quaternion startRot = rect.rotation;
        float elapsed = 0f;

        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / time);

            // Smoothly move
            rect.position = Vector3.Lerp(startPos, target.position, t);

            // Smoothly rotate
            rect.rotation = Quaternion.Slerp(startRot, target.rotation, t);

            yield return null;
        }

        // Snap exactly to target
        rect.position = target.position;
        rect.rotation = target.rotation;

        isMoving = false;
    }
}
