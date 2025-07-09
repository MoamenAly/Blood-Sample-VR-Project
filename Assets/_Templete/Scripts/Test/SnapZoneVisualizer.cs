#if UNITY_EDITOR
using BNG;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(SnapZone))]
public class SnapZoneVisualizer : Editor
{
    GrabbableItem[] grabbleList;
    static int index = 0;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // Execute your code when the inspector is drawn
        SnapZone script = (SnapZone)target;
        if (GUILayout.Button("Test"))
        {
            var childs = script.GetComponentsInChildren<GrabbableItem>();

            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] != null)
                DestroyImmediate(childs[i].gameObject);
            }
           

            var array = GetGrabbableList();
            Debug.Log(array.Length);

            if (array.Length == 0) return;



            if (index > array.Length - 1)
            {
                index = 0;
            }           



            var grabbable = Instantiate(array[index], script.transform);
            grabbable.transform.parent = script.transform;
            grabbable.gameObject.SetActive(true);
            grabbable.transform.localPosition = Vector3.zero;
            grabbable.transform.localRotation = Quaternion.identity;

            index++;
        }


        if (GUILayout.Button("Rest"))
        {
            var childs = script.GetComponentsInChildren<GrabbableItem>();


            for (int i = 0; i < childs.Length; i++)
            {
                if (childs[i] != null)

                    DestroyImmediate(childs[i].gameObject);
            }
        }



    }

    public GrabbableItem[] GetGrabbableList() {
        //if(grabbleList == null)
        grabbleList = GameObject.FindObjectsOfType<GrabbableItem>(true);
        Debug.Log(grabbleList.Length);
        return grabbleList;
    }
}
#endif

