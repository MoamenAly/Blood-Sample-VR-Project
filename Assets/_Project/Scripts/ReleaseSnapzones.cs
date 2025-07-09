using BNG;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ReleaseSnapzones : MonoBehaviour
{
    [SerializeField] public List<SnapZone> snapZones = new List<SnapZone>();

    public void ReleaseAllSnapzones()
    {
        foreach (var slot in snapZones)
        {
            slot.ReleaseAll();
        }

        DOVirtual.DelayedCall(1f, () =>
        {
            foreach (var slot in snapZones)
            {
                slot.GetComponent<Collider>().enabled = false;
            }
        });
    }
}
