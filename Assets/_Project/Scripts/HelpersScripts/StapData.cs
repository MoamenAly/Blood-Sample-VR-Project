using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Step", menuName = "Step/CreatStepData", order = 1)]
public class StapData : ScriptableObject
{
    public string Title;
    [TextArea(6, 6)] public string Description;
    public Narrations NarrationId;
}
