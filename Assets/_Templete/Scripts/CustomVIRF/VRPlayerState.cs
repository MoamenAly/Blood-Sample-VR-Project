using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class VRPlayerState
{
    [FoldoutGroup("$name")] public string name;
    [Space, FoldoutGroup("$name")] public Controller Grab;
    [FoldoutGroup("$name")] public Controller RemoteGrab;
    [FoldoutGroup("$name")] public Controller UIPointer;
    [Space]
    [FoldoutGroup("$name")]
    public bool teleport;
    [FoldoutGroup("$name")]
    public bool Locomotion;
    [FoldoutGroup("$name")]
    public bool Rotation;

    [Serializable]
    [InlineProperty]
    public class Controller
    {
        [HorizontalGroup]
        public bool Right;
        [HorizontalGroup]
        public bool Left;
    }
}

