#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class MeshColliderCopyTool
{
    [MenuItem("Scivr/Collider/Copy MeshCollider")]
    private static void CopyMeshColliderFromFirstChild()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject == null)
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject to copy MeshCollider to.");
            return;
        }

        var child = selectedGameObject.transform.GetChild(0);
        MeshCollider childMeshCollider = child.GetComponent<MeshCollider>();
        bool childAdded = false;
        if (childMeshCollider == null)
        {
            childAdded= true;
            childMeshCollider = child.AddComponent<MeshCollider>();            
            Debug.LogWarning("No MeshCollider found in the selected GameObject's children.");
        }

        if (childMeshCollider.sharedMesh == null)
        {
            Debug.LogWarning("The MeshCollider in the child does not have a mesh set.");
            return;
        }

        MeshCollider parentMeshCollider = selectedGameObject.GetComponent<MeshCollider>();
        if (parentMeshCollider == null)
        {
            parentMeshCollider = selectedGameObject.AddComponent<MeshCollider>();
        }

        // Create a new mesh that is a copy of the child's mesh but transformed
        Mesh transformedMesh = TransformMesh(childMeshCollider.sharedMesh, childMeshCollider.transform, selectedGameObject.transform);

        parentMeshCollider.sharedMesh = transformedMesh;
        parentMeshCollider.convex = childMeshCollider.convex;
        parentMeshCollider.isTrigger = childMeshCollider.isTrigger;
        parentMeshCollider.sharedMaterial = childMeshCollider.sharedMaterial;
        if (childAdded)
        {
            GameObject.DestroyImmediate(childMeshCollider);
        }
        Debug.Log("MeshCollider with transformed mesh copied from first child to selected GameObject.");
    }

    private static Mesh TransformMesh(Mesh originalMesh, Transform childTransform, Transform parentTransform)
    {
        Mesh transformedMesh = new Mesh();
        Vector3[] originalVertices = originalMesh.vertices;
        Vector3[] transformedVertices = new Vector3[originalVertices.Length];

        // Calculate the transformation matrix from child to parent
        Matrix4x4 transformMatrix = parentTransform.worldToLocalMatrix * childTransform.localToWorldMatrix;

        for (int i = 0; i < originalVertices.Length; i++)
        {
            transformedVertices[i] = transformMatrix.MultiplyPoint3x4(originalVertices[i]);
        }

        transformedMesh.vertices = transformedVertices;
        transformedMesh.triangles = originalMesh.triangles;
        transformedMesh.normals = originalMesh.normals; // Normals might need to be recalculated for correct lighting
        transformedMesh.uv = originalMesh.uv;

        transformedMesh.RecalculateBounds();
        transformedMesh.RecalculateNormals(); // Optional: Recalculate normals if the visual appearance is important

        return transformedMesh;
    }

    [MenuItem("Scivr/Collider/Copy MeshColliders")]
    private static void CopyMeshCollidersFromFirstChild()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject == null)
        {
            Debug.LogWarning("No GameObject selected. Please select a GameObject to copy MeshCollider to.");
            return;
        }

        var child = selectedGameObject.transform.GetChild(0);
        MeshCollider[] childMeshColliders = child.GetComponents<MeshCollider>();
        bool[] childAdded = new bool[childMeshColliders.Length];

        if (childMeshColliders.Length == 0)
        {
            Debug.LogWarning("No MeshColliders found in the selected GameObject's children.");
            return;
        }

        MeshCollider[] parentMeshColliders = new MeshCollider[childMeshColliders.Length];
        for (int i = 0; i < childMeshColliders.Length; i++)
        {
            if (childMeshColliders[i].sharedMesh == null)
            {
                Debug.LogWarning("The MeshCollider in the child does not have a mesh set.");
                return;
            }

            parentMeshColliders[i] = selectedGameObject.AddComponent<MeshCollider>();

            // Create a new mesh that is a copy of the child's mesh but transformed
            Mesh transformedMesh = TransformMesh(childMeshColliders[i].sharedMesh, childMeshColliders[i].transform, selectedGameObject.transform);

            parentMeshColliders[i].sharedMesh = transformedMesh;
            parentMeshColliders[i].convex = childMeshColliders[i].convex;
            parentMeshColliders[i].isTrigger = childMeshColliders[i].isTrigger;
            parentMeshColliders[i].sharedMaterial = childMeshColliders[i].sharedMaterial;
        }

        Debug.Log("MeshColliders with transformed meshes copied from first child to selected GameObject.");
    }


}








#endif