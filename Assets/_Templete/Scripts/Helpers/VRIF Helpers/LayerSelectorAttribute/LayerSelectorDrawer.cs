//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEngine;

//[CustomPropertyDrawer(typeof(LayerSelectorAttribute))]
//public class LayerSelectorDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        EditorGUI.BeginProperty(position, label, property);

//        if (property.propertyType != SerializedPropertyType.Integer)
//        {
//            EditorGUI.LabelField(position, label, "Use LayerSelectorAttribute with int.");
//            EditorGUI.EndProperty();
//            return;
//        }

//        EditorGUI.BeginChangeCheck();

//        SerializedProperty selectedLayerProperty = property.FindPropertyRelative("selectedLayer");

//        if (selectedLayerProperty == null)
//        {
//            EditorGUI.LabelField(position, label, "Selected layer property not found.");
//            EditorGUI.EndProperty();
//            return;
//        }

//        // Get the names of all layers
//        string[] layerNames = GetLayerNames();

//        // Convert the selected layer index to a layer name
//        string selectedLayerName = layerNames[selectedLayerProperty.intValue];

//        // Draw the layer dropdown
//        int layerIndex = EditorGUI.Popup(position, label.text, selectedLayerProperty.intValue, layerNames);

//        if (EditorGUI.EndChangeCheck())
//        {
//            selectedLayerProperty.intValue = layerIndex;
//            property.serializedObject.ApplyModifiedProperties();
//        }

//        // Display the selected layer name
//        Rect valueRect = new Rect(position.x + position.width - 30, position.y, 30, position.height);
//        EditorGUI.LabelField(valueRect, selectedLayerName, EditorStyles.miniLabel);

//        EditorGUI.EndProperty();
//    }

//    private string[] GetLayerNames()
//    {
//        return UnityEditorInternal.InternalEditorUtility.layers;
//    }
//}
//#endif

