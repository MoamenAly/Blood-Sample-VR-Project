using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
public class MotionRecorderVisualizer : MonoBehaviour 
{
    public List<GameObject> frames = new List<GameObject>();
    public static MotionRecorderVisualizer Instance;

    public void RestVisualObjects(List<MotionFrame> motionFrames) {      
            for (int i = 0; i < frames.Count; i++)
            {
                //if (motionFrames != null && motionFrames.Count > i)
                //{
                //    motionFrames[i].CopyTransform(frames[i].transform);
                //}
                if(frames[i].gameObject)
                DestroyImmediate(frames[i].gameObject);
            }   
            frames = new List<GameObject>();
    }

    public void CopyVisualsObjectsToFrames(ref List<MotionFrame> motionFrames)
    {
        if (frames.Count != transform.childCount) {
            frames = new List<GameObject>(transform.childCount);
            motionFrames = new List<MotionFrame>(transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (frames.Count>i)
                frames[i] = transform.GetChild(i).gameObject;
                else
                frames.Add(transform.GetChild(i).gameObject);
            } 
        }

        for (int i = 0; i < frames.Count; i++)
        {
            if (motionFrames != null && motionFrames.Count > i)
            {
                motionFrames[i].CopyTransform(frames[i].transform);
            }
            else {
                motionFrames.Add(new MotionFrame().CopyTransform(frames[i].transform));
            }
        }
    }

    public void DrawMotion(GameObject selectedObject,ref List<MotionFrame> framesData,ref bool ovvrideFrameByData)
    {
        if (!ovvrideFrameByData)
        {
            //copy edits in game objects
            CopyVisualsObjectsToFrames(ref framesData);
        }
        else
        {
            //UpdateFrames with Motions
            for (int i = 0; i < framesData.Count; i++)
            {
                var frame = GetFrame(selectedObject, i);
                frame.transform.position = framesData[i].position;
                frame.transform.rotation = Quaternion.Euler(framesData[i].eulerAngles);
                frame.transform.localScale = framesData[i].scale;
            }
        }

        ovvrideFrameByData = false;
    }

    internal void HideMotion()
    {
        if (frames == null) return;
        for (int i = 0; i < frames.Count; i++)
        {
            frames[i].SetActive(false);
        }
    }

    private GameObject GetFrame(GameObject selectedObject, int i)
    {
        GameObject visualIndicator;
        if (frames.Count > i)
        {
            visualIndicator =  frames[i];
        }
        else {
            visualIndicator = GameObject.Instantiate(selectedObject,transform);
            visualIndicator.transform.parent = transform;
            if (selectedObject.transform.parent != null)
                transform.localScale = selectedObject.transform.parent.lossyScale;
            else transform.localScale = new Vector3(1, 1, 1);
            frames.Add(visualIndicator);
        }
        visualIndicator.gameObject.SetActive(true);
        return visualIndicator;
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        for(int i = 0;i<transform.childCount;i++)
        {
            Handles.Label(transform.GetChild(i).position+Vector3.up*0.1f, "" + i);
        }
    }

#endif
}





