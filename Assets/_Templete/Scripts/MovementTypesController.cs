using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTypesController : MonoBehaviour
{
    [SerializeField] BNGPlayerController _PlayerController;

    public void ChangeMovementStyle(LocomotionType type)
    {
        _PlayerController.GetComponent<LocomotionManager>().ChangeLocomotion(type, false);
    }
}
