using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stethoscope : MonoBehaviour
{
    [SerializeField] Sphygmomanometer sphygmomanometer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arm"))
        {
            sphygmomanometer.SetStethoscopePlaced(true);
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
