//#if UNITY_EDITOR
//using UnityEngine;
//using UnityEditor;

//[CustomPropertyDrawer(typeof(DropdownListAttribute), true)]
//public class StringListDropdownDrawer : PropertyDrawer
//{
//    private DropdownListManager manager;

//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        DropdownListAttribute myAttribute = (DropdownListAttribute)attribute;

//        // Ensure manager is loaded
//        if (manager == null)
//        {
//            manager = Resources.Load<DropdownListManager>("DropdownListManager");
//            if (manager == null)
//            {
//                EditorGUI.PropertyField(position, property, label);
//                Debug.LogError("DropdownListManager resource not found.");
//                return;
//            }
//        }

//        // Retrieve the list from the DropdownListManager using the ListName
//        string[] list = manager.GetList(myAttribute.ListName);

//        if (list != null && list.Length > 0)
//        {
//            int index = Mathf.Max(0, System.Array.IndexOf(list, property.stringValue));
//            index = EditorGUI.Popup(position, label.text, index, list);
//            property.stringValue = list[index];
//        }
//        else
//        {
//            EditorGUI.PropertyField(position, property, label);
//            if (list == null || list.Length == 0)
//                Debug.LogWarning("List not found or empty: " + myAttribute.ListName);
//        }
//    }
//}
//#endif
