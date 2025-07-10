//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;
//public class VRIFShortcutHelpersTool 
//{
//    static GameObject R_Grabbable_Templete;
//    static GameObject SnapZone_Templete;
//    static GameObject VRPlayer_Templete;
//    static GameObject VRCanvas_Templete;
//    static GameObject Grabbable_Templete;

//    public static string R_Grabbable_Templete_Path = "Assets/_Templete/Prefabs/VRIF templetes/RemoteGrabbable_templete.prefab"; 
//    public static string Grabbable_Templete_Path   = "Assets/_Templete/Prefabs/VRIF templetes/Grabbable_templete.prefab";
//    public static string SnapZone_Templete_Path = "Assets/_Templete/Prefabs/VRIF templetes/SnapZone_templete.prefab";
//    public static string VRPlayer_Templete_Path = "Assets/_Templete/Prefabs/VRIF templetes/VR_Player_templete.prefab";
//    public static string VRCanvas_Templete_Path = "Assets/_Templete/Prefabs/VRIF templetes/VR_Canvas_templete.prefab";


//    [MenuItem("Scivr/Set/Grabbale #G", false, 0)]
//    static void SetGrabbale(MenuCommand command)
//    {
//        GameObject selectedGameObject = Selection.activeGameObject;
//        if (selectedGameObject != null)
//        {
//            //create empty parent
//            GameObject grabbableObject = InstantiateParentGrabbablePrefab(ref Grabbable_Templete, "_Grabbable", Grabbable_Templete_Path);
//        }
//    }

//    [MenuItem("Scivr/Set/RemoteGrabbale #R", false, 0)]
//    static void SetRemoteGrabbale(MenuCommand command)
//    {
//        GameObject selectedGameObject = Selection.activeGameObject;
//        if (selectedGameObject != null)
//        {
//            //create empty parent
//            GameObject grabbableObject = InstantiateParentGrabbablePrefab(ref R_Grabbable_Templete, "_RemoteGrabbable", R_Grabbable_Templete_Path);     
//        }
//    }

//    [MenuItem("Scivr/Set/SnapZone #Z", false, 0)]
//    static void SetSnapZoneGrabbale(MenuCommand command)
//    {
//        GameObject selectedGameObject = Selection.activeGameObject;
//        if (selectedGameObject != null)
//        {
//            //create empty parent
//            GameObject grabbableObject = InstantiateParentGrabbablePrefab(ref SnapZone_Templete, "_SnapZone", SnapZone_Templete_Path);
//        }
//    }

//    [MenuItem("Scivr/Add/VR Player", false, 0)]
//    static void AddVRPlayer(MenuCommand command)
//    {       
//       //create empty parent
//       InstantiatePrefab(ref VRPlayer_Templete,  VRPlayer_Templete_Path);        
//    }

//    [MenuItem("Scivr/Add/VR Canvas", false, 0)]
//    static void AddVRCanvas(MenuCommand command)
//    {
//        //create empty parent
//        var canvasObject = InstantiatePrefab(ref VRCanvas_Templete, VRCanvas_Templete_Path);
//        canvasObject.GetComponent<Canvas>().worldCamera = Camera.main;
//    }

//    [MenuItem("Scivr/Add/Empty UI Parent", false, 0)]
//    static void AddUIParent(MenuCommand command)
//    {
//        for (int i = 0; i < Selection.count; i++)
//        {
//            CreateParentUI(Selection.gameObjects[i]);
//        }
//    }


//    static GameObject InstantiateParentGrabbablePrefab(ref GameObject prefab , string nameSussfix ,string prefabPath)
//    {
//        GameObject selectedGameObject = Selection.activeGameObject;

//        if (prefab == null)
//        {
//            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
//            if (prefab == null)
//            {
//                Debug.LogError("Prefab not found at path: " + prefabPath);
//                return null;
//            }
//        }

//        Debug.Log("Prefab instantiated from path: " + prefabPath);
//        var grabbableObject = GameObject.Instantiate(prefab, selectedGameObject.transform.parent);
//        grabbableObject.transform.position = selectedGameObject.transform.position;
//        selectedGameObject.transform.parent = grabbableObject.transform;
//        selectedGameObject.transform.SetAsFirstSibling();
//        grabbableObject.name = selectedGameObject.name + nameSussfix;
//        return grabbableObject;
//    }

//    static GameObject InstantiatePrefab(ref GameObject prefab, string prefabPath)
//    {
//        if (prefab == null)
//        {
//            prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
//            if (prefab == null)
//            {
//                Debug.LogError("Prefab not found at path: " + prefabPath);
//                return null;
//            }
//        }

//        Debug.Log("Prefab instantiated from path: " + prefabPath);
//        return GameObject.Instantiate(prefab, Vector3.zero,Quaternion.identity);
//    }

//   static void CreateParentUI(GameObject target) {

//        GameObject parentObject = new GameObject(target+"_parent", typeof(RectTransform));
//        RectTransform parentRectTransform = parentObject.GetComponent<RectTransform>();

//        var childRectTransform = target.GetComponent<RectTransform>();

//        // Set the parent size to match the child's size
//        parentRectTransform.SetParent(childRectTransform.parent, false);
//        parentRectTransform.sizeDelta = childRectTransform.sizeDelta;

//        // Set the parent to be the child of the original parent
//        parentRectTransform.SetSiblingIndex(childRectTransform.GetSiblingIndex());

//        // Set the child to be a child of the new parent
//        childRectTransform.SetParent(parentObject.transform, false);

//        // Set the child's size to match the parent's size
//        childRectTransform.anchorMin = Vector2.zero;
//        childRectTransform.anchorMax = Vector2.one;
//        childRectTransform.offsetMin = Vector2.zero;
//        childRectTransform.offsetMax = Vector2.zero;
//    }

//}
//#endif