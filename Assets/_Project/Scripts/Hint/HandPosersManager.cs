using BNG;
using BrunoMikoski.AnimationSequencer;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosersManager : MonoBehaviour
{
    [SerializeField] HandPoser handPoserHint;
    [SerializeField] HandPoser handPoserWebgl;

    private void Start()
    {
        handPoserWebgl.gameObject.SetActive(false);
        handPoserHint.gameObject.SetActive(false);
    }


    public void AssignHandChildSource(Transform Source, Transform HintMesh ,bool isHint)
    {
        var grabPoints = Source.GetComponentsInChildren<GrabPoint>();

        if(PlatformManager.Instance.platform == Platform.Webgl)
        {
            CheckHandWebGl();
        }

        for (int i = 0; i < grabPoints.Length; i++)
        {
            if (grabPoints[i].RightHandIsValid)
            {
                if (isHint)
                {
                    handPoserHint.gameObject.SetActive(true);

                    handPoserHint.CurrentPose = grabPoints[i].SelectedHandPose;
                    handPoserHint.DoPoseUpdate();
                    handPoserHint.transform.parent = HintMesh.transform;

                    handPoserHint.transform.localPosition = grabPoints[i].transform.localPosition;
                    handPoserHint.transform.localRotation = grabPoints[i].transform.localRotation;
                }
                else
                {
                    handPoserWebgl.gameObject.SetActive(true);

                    handPoserWebgl.CurrentPose = grabPoints[i].SelectedHandPose;
                    handPoserWebgl.DoPoseUpdate();
                    handPoserWebgl.transform.parent = Source.transform;

                    handPoserWebgl.transform.localPosition = grabPoints[i].transform.localPosition;
                    handPoserWebgl.transform.localRotation = grabPoints[i].transform.localRotation;
                }

            }
        }
    }



    void CheckHandWebGl()
    {


        // Check if handPoserWebgl is assigned
        if (handPoserWebgl != null)
        {
            // Check if the local scale is not equal to Vector3.one (scale of 1 on all axes)
            if (handPoserWebgl.transform.localScale != Vector3.one)
            {
                Debug.Log("Local scale is not equal to one. Fixing it now.");

                // Set the local scale to Vector3.one (1, 1, 1)
                handPoserWebgl.transform.localScale = Vector3.one;
            }
  
        }

    }
}

