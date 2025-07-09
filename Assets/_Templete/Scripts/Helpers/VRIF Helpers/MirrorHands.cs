#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class MirrorHands
{
    [MenuItem("Scivr/Mirror Hands/Along X Axis &x")]
    static void MirrorHandsAlongX()
    {
        MirrorHandsAlongAxis(new Vector3(-1, 1, 1));
    }

    [MenuItem("Scivr/Mirror Hands/Along Y Axis &y")]
    static void MirrorHandsAlongY()
    {
        MirrorHandsAlongAxis(new Vector3(1, -1, 1));
    }

    [MenuItem("Scivr/Mirror Hands/Along Z Axis &z")]
    static void MirrorHandsAlongZ()
    {
        MirrorHandsAlongAxis(new Vector3(1, 1, -1));
    }

    static void MirrorHandsAlongAxis(Vector3 mirrorAxes)
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogError("No object selected. Please select a hand object.");
            return;
        }

        GameObject parentObject = Selection.activeTransform.parent?.gameObject;
        if (parentObject == null)
        {
            Debug.LogError("Please select a hand object with a parent containing both hands.");
            return;
        }

        Transform source = Selection.activeTransform;
        Transform target = null;

        // Use tags to determine the target based on the selected source hand
        string targetTag = source.tag == "rightHandGrabPoint" ? "leftHandGrabPoint" : "rightHandGrabPoint";

        foreach (var child in parentObject.GetComponentsInChildren<Transform>())
        {
            if (child.tag == targetTag)
            {
                target = child;
                break;
            }
        }

        if (target == null)
        {
            Debug.LogError($"Could not find another hand under the selected parent object with the tag {targetTag}.");
            return;
        }

        // Record the target object before making changes for undo functionality
        Undo.RecordObject(target, "Set Hand Position and Rotation");

        // Dynamic offset based on the source's rotation
        Vector3 offset = (source.tag == "rightHandGrabPoint") ?
                         (source.right * 0.02f + source.up * 0.05f + source.forward * 0.035f) :
                         (-source.right * 0.02f - source.up * 0.05f - source.forward * 0.035f);
        target.localPosition = source.localPosition + offset;

        // Mirror the rotation
        Quaternion mirrorRotation = Quaternion.LookRotation(
            Matrix4x4.Scale(mirrorAxes).MultiplyVector(source.forward),
            Matrix4x4.Scale(mirrorAxes).MultiplyVector(source.up));

        target.localRotation = mirrorRotation;

        // Register complete undo action
        Undo.FlushUndoRecordObjects();

        Debug.Log($"Set the target hand's position and rotation based on the source hand with dynamically applied offset.");
    }
}
#endif