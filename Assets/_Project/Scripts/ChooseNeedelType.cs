using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ChooseNeedleType: MonoBehaviour
{
    [SerializeField] public NeedleType needleType;

    private void Start()
    {
        needleType = NeedleType.None;
    }

    [Button]
    public void ChooseNeedle(NeedleType _needleType)
    {
        needleType = _needleType;
    }
}

public enum NeedleType
{
    Vacutainer,
    Butterfly,
    None
}
