using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BrunoMikoski.AnimationSequencer;

public class MonitorAnimationHelper : MonoBehaviour
{


    public AnimationSequencerController _gettoplayer , restMointor;


    public void GetMonitorToPlayer()
    {

        if(PlatformManager.Instance.platform == Platform.Webgl)
        {
            _gettoplayer.Play();
        }
  
    }

    public void RestMonitorToPlayer()
    {

        if (PlatformManager.Instance.platform == Platform.Webgl)
        {
            restMointor.Play();
        }

    }

}
