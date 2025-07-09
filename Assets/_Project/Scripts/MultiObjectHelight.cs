using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiObjectHelight : MonoBehaviour
{
    public Outline[] higlightobjects;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    public void EnableHiglight()
    {

        foreach (var obj in higlightobjects)
        {

            if (obj != null)
            {

                obj.enabled = true;
            }
        }
    }

    public void DisableHiglight()
    {

        foreach (var obj in higlightobjects)
        {

            if (obj != null)
            {

                obj.enabled = false;
            }
        }
    }
}
