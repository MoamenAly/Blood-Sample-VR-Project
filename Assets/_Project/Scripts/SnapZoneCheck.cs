using BNG;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SnapZoneCheck : MonoBehaviour
{
    [ReadOnly] public List<string> allowNames = new List<string>();
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip wrongClip;

    private SnapZone snapZone;

    void Start()
    {
        snapZone = GetComponent<SnapZone>();
        if (snapZone != null && snapZone.OnlyAllowNames != null)
        {
            foreach (var item in snapZone.OnlyAllowNames)
            {
                allowNames.Add(item);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!allowNames.Contains(other.name) && other.gameObject.layer == LayerMask.NameToLayer("Grabbale"))
        {
            if (!sfx.isPlaying)
            {
                sfx.PlayOneShot(wrongClip);
            }
        }
    }

}
