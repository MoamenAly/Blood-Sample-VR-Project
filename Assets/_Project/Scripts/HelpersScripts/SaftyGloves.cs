using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaftyGloves : MonoBehaviour
{
    [SerializeField] private HandModelSelector handModelSelector;


    public void WearGloves()
    {
        handModelSelector.ChangeHandsModel(2);
    }
}
