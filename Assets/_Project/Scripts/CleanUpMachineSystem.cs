using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BNG;
using Sirenix.OdinInspector;

public class CleanUpMachineSystem : MonoBehaviour
{


    public List<SnapZone> systemsnapzone;

    public List<RestOnUnGrab> restonungrabObjects;


    public Transform Parent;



    [Button]
    public void RestMachine()
    {



        foreach (var snap in systemsnapzone)
        {

            if (snap != null)
            {

                snap.ReleaseAll();
                //snap.transform.SetParent(Parent);
            }
        }

        foreach (var rest in restonungrabObjects)
        {
            if (rest != null)
            {
           //     rest.transform.SetParent(Parent);
          //      rest.OnForceDefaultPosition();

            }
        }

    }
}
