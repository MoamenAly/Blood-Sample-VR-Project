using System;
using UnityEngine;

[System.Serializable]
public class MotionFrame
{
    public Vector3 position;
    public Vector3 eulerAngles; // Euler angles
    public Vector3 scale;


    public MotionFrame CopyTransform(Transform other) {
        this.position = other.position;
        this.eulerAngles = other.eulerAngles;
        this.scale    = other.localScale;
        return this;
    }

    public MotionFrame ApplayTransformTo(Transform other)
    {
       other.position    = this.position  ;
       other.eulerAngles = this.eulerAngles  ;
       other.localScale  = this.scale     ;
       return this;
    }
}
