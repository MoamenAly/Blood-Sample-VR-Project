using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "NewMotionData", menuName = "Motion Data", order = 0)]
public class MotionData : ScriptableObject
{
    public List<MotionFrame> motionFrames = new List<MotionFrame>();    
}
