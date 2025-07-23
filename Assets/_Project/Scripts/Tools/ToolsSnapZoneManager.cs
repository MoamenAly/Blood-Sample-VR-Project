using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolsSnapZoneManager : MonoBehaviour
{

    public UnityEvent2 ONprepareAllTools;

    public List<SnapZone> snapZones ;

    public int itemsCount = 0;



    public void OnPrepareTool()
    {
        itemsCount++;
        if(itemsCount == snapZones.Count)
        {
            print("Finished");
            ONprepareAllTools?.Invoke();    
        }
    }






}
