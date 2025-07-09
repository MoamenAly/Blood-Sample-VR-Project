#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class CopyPasteTransformTool 
{
    private static Transform copiedTransform;
    private static RectTransform copiedRectTransform;

    [MenuItem("Edit/Copy Transform or RectTransform #c", false, 0)]
    static void CopyTransformOrRectTransform(MenuCommand command)
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject != null)
        {
            copiedTransform = selectedGameObject.transform;
            copiedRectTransform = selectedGameObject.GetComponent<RectTransform>();
            if (copiedTransform != null)
                Debug.Log("Transform copied!");
            else if (copiedRectTransform != null)
                Debug.Log("RectTransform copied!");
        }
        else
        {
            Debug.Log("No GameObject selected!");
        }
    }

    [MenuItem("Edit/Paste Transform or RectTransform #v", false, 0)]
    static void PasteTransformOrRectTransform(MenuCommand command)
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (selectedGameObject != null)
        {
            if (copiedTransform != null)
            {
                CopyTransform(selectedGameObject.transform, copiedTransform);
                Debug.Log("Transform pasted!");
            }
            else if (copiedRectTransform != null)
            {
                CopyRectTransform(selectedGameObject.GetComponent<RectTransform>(),copiedRectTransform);
                Debug.Log("RectTransform pasted!");
            }
            else
            {
                Debug.Log("No transform copied!");
            }
        }
        else
        {
            Debug.Log("No GameObject selected to paste transform to!");
        }
    }

    static void CopyRectTransform(RectTransform selectedGameObject,RectTransform copiedRectTransform) {
        Undo.RecordObject(selectedGameObject, "Paste RectTransform");
        selectedGameObject.anchoredPosition = copiedRectTransform.anchoredPosition;
        selectedGameObject.sizeDelta = copiedRectTransform.sizeDelta;
        selectedGameObject.pivot = copiedRectTransform.pivot;
        selectedGameObject.localRotation = copiedRectTransform.localRotation;
        selectedGameObject.localScale = copiedRectTransform.localScale;
        Debug.Log("RectTransform pasted!");
    }

    static void CopyTransform(Transform selectedGameObject, Transform copiedTransform)
    {
        Undo.RecordObject(selectedGameObject.transform, "Paste Transform");
        selectedGameObject.transform.position = copiedTransform.position;
        selectedGameObject.transform.rotation = copiedTransform.rotation;
        selectedGameObject.transform.localScale = copiedTransform.localScale;
        Debug.Log("RectTransform pasted!");
    }


}
#endif
