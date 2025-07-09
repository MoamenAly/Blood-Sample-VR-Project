using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BNG;
using Sirenix.OdinInspector;

public class SnapZoneAvability : MonoBehaviour
{


    SnapZone _snapzone;
        // Start is called before the first frame update
    void Start()
    {

        _snapzone = GetComponent<SnapZone>();
        
    }





    [Button]
    public void SetSnapZoneRemoveItem(bool _state)
    {
        if (_snapzone != null)
            _snapzone.CanRemoveItem = _state;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
