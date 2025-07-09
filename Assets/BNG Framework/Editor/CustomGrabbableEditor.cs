using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace BNG
{ 
    [CustomEditor(typeof(CustomGrabbable), true)]
     [CanEditMultipleObjects] 
     public class CustomGrabbableEditor : GrabbableEditor
     {
        //protected SerializedProperty handOffLayer;
        //protected SerializedProperty handOnLayer;
        //protected SerializedProperty _handOffLayer;
        //protected SerializedProperty _handOnLayer;

        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //    handOffLayer = serializedObject.FindProperty("_handOffLayer");
        //    handOnLayer = serializedObject.FindProperty("_handOnLayer");
        //    _handOffLayer = serializedObject.FindProperty("handOffLayer");
        //    _handOnLayer = serializedObject.FindProperty("handOnLayer");
        //}


        //public override void OnInspectorGUI()
        //{
        //    base.OnInspectorGUI();
        //        EditorGUILayout.PropertyField(handOnLayer);
        //       EditorGUILayout.PropertyField(handOffLayer);
        //    EditorGUILayout.PropertyField(_handOnLayer);
        //    EditorGUILayout.PropertyField(_handOffLayer);
        //    serializedObject.ApplyModifiedProperties();

        //}

    }
    
}