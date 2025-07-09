using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BNG;
public class EnableMultiplyeSnapZone : MonoBehaviour
{


    public List<SnapZone> allsnape;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void EnableAlSnapZone()
    {


        foreach(var snap in allsnape)
        {

            SnapZoneAvability snapAV = snap.GetComponent<SnapZoneAvability>();
            if(snapAV != null)
            {
                snapAV.SetSnapZoneRemoveItem(true);
            }
        }
    }
}
