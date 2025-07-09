using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ZinCPowderSimulation : MonoBehaviour
{


    [SerializeField] GameObject powdereffect;

    [SerializeField] GameObject powder1, powder2;
    
    public async void PutPowdereFFECT()
    {

        print("Hello fromPutPowdereFFECT ");

        await Task.Delay(1000);

        powder1.SetActive(true);
        powder2.SetActive(true);
    }
}
