#if UNITY_EDITOR

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class BoxColliderAdjustmentTool
{
    [MenuItem("Helper/Collider/BoxCollider To Child")]
    [MenuItem("Tools/Copy And Align BoxCollider From First Child")]
    private static void CopyAndAlignBoxColliderFromFirstChild()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject == null)
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject to copy BoxCollider to.");
            return;
        }

        if (selectedGameObject.transform.childCount == 0)
        {
            Debug.LogWarning("Selected GameObject has no children.");
            return;
        }

        GameObject childGameObject = selectedGameObject.transform.GetChild(0).gameObject;
        if (childGameObject == null)
        {
            Debug.LogWarning("No child GameObject found.");
            return;
        }

        MeshFilter childMeshFilter = childGameObject.GetComponent<MeshFilter>();
        if (childMeshFilter == null || childMeshFilter.sharedMesh == null)
        {
            Debug.LogWarning("Child GameObject has no mesh.");
            return;
        }

        BoxCollider parentBoxCollider = selectedGameObject.GetComponent<BoxCollider>();
        if (parentBoxCollider == null)
        {
            parentBoxCollider = selectedGameObject.AddComponent<BoxCollider>();
        }

        // Calculate the child mesh bounds in world space
        Mesh childMesh = childMeshFilter.sharedMesh;
        Bounds childMeshBounds = childMesh.bounds;

        // Get the vertices of the child's mesh in world space
        Vector3[] vertices = childMesh.vertices;
        Vector3 min = childGameObject.transform.TransformPoint(vertices[0]);
        Vector3 max = min;

        foreach (Vector3 vertex in vertices)
        {
            Vector3 worldPoint = childGameObject.transform.TransformPoint(vertex);
            min = Vector3.Min(min, worldPoint);
            max = Vector3.Max(max, worldPoint);
        }

        // Calculate the center and size in world space
        Vector3 worldCenter = (min + max) / 2f;
        Vector3 worldSize = max - min;

        // Set the parent BoxCollider size and center to match the child mesh bounds
        parentBoxCollider.size = worldSize;
        parentBoxCollider.center = selectedGameObject.transform.InverseTransformPoint(worldCenter);

        Debug.Log("BoxCollider copied and aligned from first child to selected GameObject.");
    }

    [MenuItem("Helper/Collider/BoxCollider To Children")]
    private static void AdjustBoxColliderToChildrenMenuOption()
    {
        // Ensure a GameObject is selected
        if (Selection.activeGameObject == null)
        {
            Debug.LogWarning("BoxColliderAdjustmentTool: No GameObject selected.");
            return;
        }

        // Get or add a BoxCollider component
        BoxCollider boxCollider = Selection.activeGameObject.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = Selection.activeGameObject.AddComponent<BoxCollider>();
        }

        // Perform the adjustment
        AdjustBoxColliderToFitChildren(Selection.activeGameObject, boxCollider);
        Debug.Log("BoxCollider adjusted to fit all child meshes.");
    }


    private static void AdjustBoxColliderToFirstChild(GameObject parent, BoxCollider boxCollider)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;

        Renderer renderer = parent.GetComponentInChildren<Renderer>();
     
            if (hasBounds)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            else
            {
                bounds = renderer.bounds;
                hasBounds = true;
            }
     
        if (!hasBounds)
        {
            Debug.LogWarning("BoxColliderAdjustmentTool: No renderers found in children.");
            return;
        }

        // Convert the bounds to local space
        Bounds localBounds = new Bounds(parent.transform.InverseTransformPoint(bounds.center), Vector3.zero);
        //foreach (Vector3 point in bounds.GetCornerPoints())
        //{
        //    localBounds.Encapsulate(parent.transform.InverseTransformPoint(point));
        //}

        // Here's the corrected center adjustment
        boxCollider.center = localBounds.center;
        boxCollider.size = localBounds.size;
    }
    private static void AdjustBoxColliderToFitChildren(GameObject parent, BoxCollider boxCollider)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;

        Renderer []renderers = parent.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
              {
            if (hasBounds)
            {
                bounds.Encapsulate(renderer.bounds);
            }
            else
            {
                bounds = renderer.bounds;
                hasBounds = true;
            }
        }

        if (!hasBounds)
        {
            Debug.LogWarning("BoxColliderAdjustmentTool: No renderers found in children.");
            return;
        }

        // Convert the bounds to local space
        Bounds localBounds = new Bounds(parent.transform.InverseTransformPoint(bounds.center), Vector3.zero);
        foreach (Vector3 point in bounds.GetCornerPoints())
        {
            localBounds.Encapsulate(parent.transform.InverseTransformPoint(point));
        }

        // Here's the corrected center adjustment
        boxCollider.center = localBounds.center;
        boxCollider.size = localBounds.size;
    }

}
#endif

public static class BoundsExtensions
{
    public static Vector3[] GetCornerPoints(this Bounds bounds)
    {
        return new Vector3[] {
            new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
            new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
            new Vector3(bounds.max.x, bounds.max.y, bounds.max.z)
        };
    }
}
#endif