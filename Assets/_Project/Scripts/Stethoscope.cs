using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stethoscope : MonoBehaviour
{
    [SerializeField] Sphygmomanometer sphygmomanometer;
    [SerializeField] GameObject Arm_CollisionForStethoscope;
    [SerializeField] UnityEvent2 OnPutStethoscope;
    private bool isPutStethoscope;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arm"))
        {
            Arm_CollisionForStethoscope.SetActive(false);
            sphygmomanometer.SetStethoscopePlaced(true);
            if(!isPutStethoscope)
            {
                OnPutStethoscope?.Invoke();
                isPutStethoscope = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Arm"))
        {
            sphygmomanometer.SetStethoscopePlaced(false);
        }
    }

}
