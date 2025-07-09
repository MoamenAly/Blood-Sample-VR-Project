using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using BNG;

public class CleanerSnapZone : MonoBehaviour
{



    [SerializeField] private SnapZone _snapzone;
    public List<objectProperties> ObjectProperties;



    private void Awake()
    {
        _snapzone = GetComponent<SnapZone>();
    }
    private void Start()
    {
       
    }


    public void OnSetPositoin()
    {
        if (_snapzone == null)
        {
            Debug.LogError("SnapZone is not assigned.");
            return;
        }

        if (_snapzone.HeldItem == null)
        {
            Debug.LogWarning("No item is currently held in the SnapZone.");
            return;
        }

        if (ObjectProperties == null || ObjectProperties.Count == 0)
        {
            Debug.LogWarning("ObjectProperties list is empty or not assigned.");
            return;
        }

        foreach (var item in ObjectProperties)
        {
            if (item == null)
            {
                Debug.LogWarning("An objectProperties item is null.");
                continue;
            }

            if (item.o_bject == null)
            {
                Debug.LogWarning("An objectProperties item has o_bject set to null.");
                continue;
            }

            if (_snapzone.HeldItem.gameObject == null)
            {
                Debug.LogWarning("HeldItem's gameObject is null.");
                continue;
            }

            if (item.o_bject.gameObject == _snapzone.HeldItem.gameObject)
            {
                _snapzone.HeldItem.transform.localPosition = item.Offset;
                _snapzone.HeldItem.transform.localRotation = Quaternion.Euler(item.RotationOffset);
            }
        }
    }


}



[System.Serializable]
public class objectProperties
{
  
   
  
    public GameObject o_bject; // GameObject preview with no label

    [FoldoutGroup("$GetFoldoutName")]
    [BoxGroup("$GetFoldoutName/Offsets")]
    [LabelText("Position Offset")]
    public Vector3 Offset; // Position offset inside a foldable section

    [FoldoutGroup("$GetFoldoutName")]
    [BoxGroup("$GetFoldoutName/Offsets")]
    [LabelText("Rotation Offset")]
    public Vector3 RotationOffset; // Rotation offset inside a foldable section

    // This method will return the GameObject's name for the FoldoutGroup title
    private string GetFoldoutName()
    {
        return o_bject != null ? o_bject.name : "Unnamed Object"; // Default name if no object is assigned
    }

}
