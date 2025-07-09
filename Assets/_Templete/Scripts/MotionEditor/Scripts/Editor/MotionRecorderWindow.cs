#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MotionRecorderWindow : EditorWindow, IMotionRecorderWindow
{
    //data
    private GameObject selectedObject;
    MotionFrame defultTransformForSelectedObject = new MotionFrame();

    List<MotionFrame> motionFrames = new List<MotionFrame>();   

    MotionRecorderWindowUI motionUI;
    MotionRecorderVisualizer motionRecorderVisualizer;
    private static int currentFrameIndex = 0;

    public GameObject SelectedObject {
        get {      
            return selectedObject;
        }
    }

    #region GUI
    [MenuItem("Tools/Motion Recorder V2")]
    public static void ShowWindow()
    {
        GetWindow<MotionRecorderWindow>("Motion Recorder").Show();
    }

    private void OnEnable()
    {     
        motionUI = new MotionRecorderWindowUI(this);
    }

    void OnGUI()
    {
        motionUI.UpdateWindowUI(SelectedObject);

        ShowPreview();

        VisualizeFrames();  
    }


    public bool previewState = false;
    public int cashedIndex = -1;
    private void ShowPreview()
    {

        if(previewState!= motionUI.IsRecording && motionUI.IsRecording || cashedIndex!=currentFrameIndex&&currentFrameIndex==-1) {
            currentFrameIndex = motionFrames.Count-1;
            if (selectedObject != null)
            {
                if (currentFrameIndex >= 0 && currentFrameIndex < motionFrames.Count)
                {
                    MotionFrame frame = motionFrames[currentFrameIndex];
                    frame.ApplayTransformTo(selectedObject.transform);
                }
            }
            cashedIndex = currentFrameIndex = -1;
        }

        previewState = motionUI.IsRecording;
        cashedIndex = currentFrameIndex;

        if (motionUI.IsRecording)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Edit Mode Motion Preview", EditorStyles.boldLabel);
            // Use the static variable to persist the slider value
            int maxFrameIndex = motionFrames.Count - 1;
            currentFrameIndex = EditorGUILayout.IntSlider("Frame Index", currentFrameIndex,-1, maxFrameIndex);
            if (selectedObject != null) {
                   if (currentFrameIndex >= 0 && currentFrameIndex < motionFrames.Count)
                   {
                       MotionFrame frame = motionFrames[currentFrameIndex]; 
                       frame.ApplayTransformTo(selectedObject.transform);                    
                   }
            }
        }
    }



    private void VisualizeFrames()
    {
        if (motionRecorderVisualizer == null) return;   

        if(selectedObject != null && motionUI.IsRecording)
            motionRecorderVisualizer.DrawMotion(SelectedObject,ref motionFrames,ref ovverideFrameByData);
        //else
         //   motionRecorderVisualizer.RestVisualObjects(motionFrames);
    }

    #endregion

    #region Actions
    public bool ovverideFrameByData = false;
    public void AddMotionFrame()
    {
        ovverideFrameByData = true;
        motionFrames.Add(new MotionFrame().CopyTransform(SelectedObject.transform));
    }

    public void RestMotion()
    {
        RestSelectedTransform();
        motionFrames = new();
        motionRecorderVisualizer.RestVisualObjects(motionFrames);
    }

    public void SaveMotion()
    {
        // Prompt the user to enter a motion name
        SaveMotionWindow.Init((motionName) =>
        {
            if (string.IsNullOrEmpty(motionName))
            {
                Debug.LogError("Motion name cannot be empty!");
                return;
            }

            // Create a new motion data asset
            var currentMotionData = Helper.CreateNewMotionDataAsset(motionName);

            // Save the recorded motion data
            currentMotionData.motionFrames = motionFrames;
            EditorUtility.SetDirty(currentMotionData);
        });
    }

    public void StartRecord()
    {
        ovverideFrameByData = true;

        currentFrameIndex = 0;
        motionRecorderVisualizer = FindObjectOfType<MotionRecorderVisualizer>();
        if (motionRecorderVisualizer == null)
        {
            GameObject g = new GameObject("motion editor");
            motionRecorderVisualizer = g.AddComponent<MotionRecorderVisualizer>();
        }

        selectedObject = Selection.activeGameObject;
        SaveDefaultTransformOfSelectedObject(selectedObject);
        Debug.Log("start");

    }

    public void StopRecord()
    {   
        Selection.activeGameObject = selectedObject;
        RestSelectedTransform();
        Debug.Log("stop");
    }

    private void RestSelectedTransform() {
        if (selectedObject != null)
        {
            defultTransformForSelectedObject.ApplayTransformTo(selectedObject.transform);
            motionUI.RestWindowUI();
            motionRecorderVisualizer.RestVisualObjects(motionFrames);
        }
    }

    private void SaveDefaultTransformOfSelectedObject(GameObject gameObject) {
        if (selectedObject == null) return;       
        defultTransformForSelectedObject = new MotionFrame().CopyTransform(gameObject.transform);
    }

    public void LoadMotion()
    {
       ovverideFrameByData = true; 
       var loadedData = Helper.LoadMotion();
       if(loadedData != null) 
       motionFrames = new List<MotionFrame>(loadedData.motionFrames);
    }

    #endregion

}

public class Helper {
    public static MotionData CreateNewMotionDataAsset(string motionName)
    {
        // Ensure the Motions directory exists
        string motionDataFolderPath = "Assets/Motions";
        if (!AssetDatabase.IsValidFolder(motionDataFolderPath))
        {
            AssetDatabase.CreateFolder("Assets", "Motions");
        }

        MotionData currentMotionData = ScriptableObject.CreateInstance<MotionData>();
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(motionDataFolderPath + "/" + motionName + ".asset");
        AssetDatabase.CreateAsset(currentMotionData, uniquePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.FocusProjectWindow();
        return currentMotionData;
    }

    public static MotionData LoadMotion()
    {
        string path = EditorUtility.OpenFilePanel("Load Motion Data", "Assets/Motions", "asset");
        if (!string.IsNullOrEmpty(path) && path.StartsWith(Application.dataPath))
        {
            string relativePath = "Assets" + path.Substring(Application.dataPath.Length);
            MotionData loadedData = AssetDatabase.LoadAssetAtPath<MotionData>(relativePath);
            return loadedData;
        }else return null;
    }



}
#endif