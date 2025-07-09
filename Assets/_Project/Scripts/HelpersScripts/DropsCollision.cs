using LiquidVolumeFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DropsCollision : MonoBehaviour
{
    //public Elements _targetElementId;
    //public int currentDropsNo;
    //public int totalDropsNo;
    //public LiquidVolume _liquidTube;
    //public float amountValue;
    //[Space] public UnityEvent2 OnDone;

    public List<DropsSequnace> Sequnance = new List<DropsSequnace>();
    public LiquidVolume _liquidTube;
    public int CurrentIndex;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Item item))
        {
            if (item.id != Sequnance[CurrentIndex]._targetElementId) return;

            if (Sequnance[CurrentIndex].currentDropsNo >= Sequnance[CurrentIndex].totalDropsNo) return;

            Sequnance[CurrentIndex].currentDropsNo++;
            _liquidTube.level += Sequnance[CurrentIndex].amountValue;
            //_liquidTube.UpdateLayers();

            if (Sequnance[CurrentIndex].currentDropsNo == Sequnance[CurrentIndex]. totalDropsNo) StartCoroutine(OnDone_Coroutine());
        }
    }

    IEnumerator OnDone_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);

        CurrentIndex++;
        Sequnance[CurrentIndex].OnDone?.Invoke();
        
    }


    [System.Serializable]
    public class DropsSequnace
    {

        public Elements _targetElementId;
        public int currentDropsNo;
        public int totalDropsNo;
  
        public float amountValue;
        [Space] public UnityEvent2 OnDone;
    }
}
