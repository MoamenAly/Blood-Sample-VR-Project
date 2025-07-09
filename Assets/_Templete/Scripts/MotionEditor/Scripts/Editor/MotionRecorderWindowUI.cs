#if UNITY_EDITOR 
using System;
using UnityEditor;
using UnityEngine;

public class MotionRecorderWindowUI {
    //constant
    private const int ButtonWidth = 100;
    public  bool IsRecording = false;

    IMotionRecorderWindow motionRecorderWindow;

    public MotionRecorderWindowUI(IMotionRecorderWindow motionRecorderWindow) { 
       this.motionRecorderWindow = motionRecorderWindow;
    }
    public  void UpdateWindowUI(GameObject selectedObject)
    {
        GUILayout.Label("Motion Recorder", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        //selected object 
        EditorGUILayout.ObjectField("Selected Object", selectedObject, typeof(GameObject), true);


        if(IsRecording)
        ShowLoadUI();

        // Record 
        GUILayout.BeginHorizontal();
        {
            if (!IsRecording)
            {
                //Satrt button ui 
                GUI.backgroundColor = GUI.contentColor; 
                if (GUILayout.Button("Start", GUILayout.Width(ButtonWidth)))
                {
                    IsRecording = true;
                    motionRecorderWindow.StartRecord();
                }
            }
            else
            {
                //Stop button ui 
                //load save
                ShowStopUI(); 
                //show recording
                ShowRecordingUI();
                ShowSavingUI();
                ShowRestUI();
            }

        }
        GUILayout.EndHorizontal();

    }

    private void ShowStopUI()
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Stop", GUILayout.Width(ButtonWidth)))
        {
            IsRecording = false;
            motionRecorderWindow.StopRecord();
        }
    }

    private void ShowLoadUI()
    {
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Load", GUILayout.Width(ButtonWidth)))
            {
                motionRecorderWindow.LoadMotion();
            }
        }
        GUILayout.EndHorizontal();
    }

    private void ShowRestUI()
    {
        if (GUILayout.Button("Rest", GUILayout.Width(ButtonWidth)))
        {
            motionRecorderWindow.RestMotion();
        }
    }

    private void ShowSavingUI()
    {
        if (GUILayout.Button("Save", GUILayout.Width(ButtonWidth)))
        {
            motionRecorderWindow.SaveMotion();
        }
    }

    private  void ShowRecordingUI()
    {
        if (GUILayout.Button("Add Frame", GUILayout.Width(ButtonWidth)))
        {
            motionRecorderWindow.AddMotionFrame();
        }
    }

 

    public  void RestWindowUI() {
        IsRecording = false;
    }



}

#endif