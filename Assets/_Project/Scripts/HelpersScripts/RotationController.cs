using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotationController : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float rotationDuration = 5f;

    private float elapsedTime = 0f;
    private bool isRotating = false;

    public UnityEvent2 OnRotated;

    void Update()
    {
        if (isRotating)
        {

            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);


            elapsedTime += Time.deltaTime;


            if (elapsedTime >= rotationDuration)
            {
                isRotating = false;
                elapsedTime = 0f;
                OnRotated?.Invoke();
            }
        }
    }

    [Button()]
    public void StartRotation()
    {
        isRotating = true;
    }

    [Button()]
    public void StopRotation()
    {
        isRotating = false;
    }
}
