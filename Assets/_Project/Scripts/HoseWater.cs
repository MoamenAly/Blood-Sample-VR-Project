using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dreamteck.Splines;
using DG.Tweening;

using UnityEngine.Events;
public class HoseWater : MonoBehaviour
{

    [SerializeField] private float speed;
    public TubeGenerator tube;
    // Start is called before the first frame update
    double fillamount;

    public UnityEvent2 OnFillDone;
    void Start()
    {
       
    }


    public void Fill()
    {

        DOTween.To(() => fillamount, x => fillamount = x, 1f, speed).From(0)
            .OnUpdate(() => { tube.clipTo = fillamount; })
            .OnComplete(() => OnFillDone?.Invoke());
    }

}
