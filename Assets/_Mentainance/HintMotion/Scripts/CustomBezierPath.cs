using UnityEngine;
using DG.Tweening;
using System;

public class CustomBezierPath : MonoBehaviour
{
    public event System.Action OnMotionStarted;
    public event System.Action OnMotionCompleted;

    public static void DOBezierPath(Transform target, Vector3[] waypoints, float duration,Action OnMotionStarted,Action OnMotionCompleted)
    {
        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogError("Waypoints array must contain at least 2 points.");
            return;
        }

        OnMotionStarted?.Invoke();

       
            // Use custom Bezier path
            DOTween.To(
                () => 0f,
                t => {
                    Vector3 newPosition;
                    if (waypoints.Length == 3)
                    {
                        // Quadratic Bezier curve
                        newPosition = Bezier.CalculateQuadraticBezierPoint(t, waypoints[0], waypoints[1], waypoints[2]);
                    }
                    else if (waypoints.Length == 4)
                    {
                        // Cubic Bezier curve
                        newPosition = Bezier.CalculateCubicBezierPoint(t, waypoints[0], waypoints[1], waypoints[2], waypoints[3]);
                    }
                    else
                    {
                        Debug.LogError("Bezier path requires exactly 3 (quadratic) or 4 (cubic) waypoints.");
                        return;
                    }

                    target.position = newPosition;
                },
                1f,
                duration
            ).OnComplete(() =>
            {
                OnMotionCompleted?.Invoke();
            });
        }
 }


public static class Bezier
{
    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * p0; // u^2 * P0
        point += 2 * u * t * p1; // 2 * u * t * P1
        point += tt * p2; // t^2 * P2

        return point;
    }

    public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float ttt = tt * t;
        float uuu = uu * u;

        Vector3 point = uuu * p0; // u^3 * P0
        point += 3 * uu * t * p1; // 3 * u^2 * t * P1
        point += 3 * u * tt * p2; // 3 * u * t^2 * P2
        point += ttt * p3; // t^3 * P3

        return point;
    }
}
