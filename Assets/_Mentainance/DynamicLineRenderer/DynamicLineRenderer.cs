using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class DynamicLineRenderer : MonoBehaviour
{
    [SerializeField]
    private List<Transform> points = new List<Transform>();

    [SerializeField] LineRenderer lineRenderer;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        UpdateLineRenderer();
    }

//#if UNITY_EDITOR
    private void Update()
    {
        UpdateLineRenderer();
    }
//#endif 

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var point in points)
        {
            if (point != null)
            {
                Gizmos.DrawSphere(point.position, 0.01f);
            }
        }
    }

    public void UpdateLineRenderer()
    {
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = points.Count;
            for (int i = 0; i < points.Count; i++)
            {
                lineRenderer.SetPosition(i, points[i].position);
            }
        }
    }

    public void AddPoint()
    {
        Vector3 newPosition;

        if (points.Count == 0)
        {
            newPosition = transform.position;
        }
        else
        {
            newPosition = points[points.Count - 1].position; // Same position as the last point
        }

        GameObject newPoint = new GameObject("Point " + points.Count);
        newPoint.transform.position = newPosition;
        newPoint.transform.parent = transform;
        points.Add(newPoint.transform);
        UpdateLineRenderer();
    }

    public void RemovePoint(int index)
    {
        if (index >= 0 && index < points.Count)
        {
            DestroyImmediate(points[index].gameObject);
            points.RemoveAt(index);
            UpdateLineRenderer();
        }
    }
}


