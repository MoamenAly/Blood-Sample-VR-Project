using BNG;
using UnityEngine;

public class SnapZoneDependency : MonoBehaviour, IDepndency
{
    [SerializeField] SnapZone snapZone;
    [SerializeField] Grabbable target;
    Grabbable grabed;

    private void OnValidate()
    {
        SnapZone snapZone;
        TryGetComponent<SnapZone>(out snapZone);
        if (snapZone != null) this.snapZone = snapZone;

        Grabbable target;
        TryGetComponent<Grabbable>(out target);
        if(target != null) this.target = target;
    }

    private void OnEnable()
    {
        snapZone.OnSnapEvent.AddListener(OnSnapped);
        snapZone.OnDetachEvent.AddListener(OnDetached);
    }

    private void OnDisable()
    {
        snapZone.OnSnapEvent.RemoveListener(OnSnapped);
        snapZone.OnDetachEvent.RemoveListener(OnDetached);
    }

    public bool IsCompleted()
    {
        if(grabed != null&&grabed==target) {
         return true;
        }
        return false; 
    }   

    private void OnSnapped(Grabbable arg0)
    {
        grabed = arg0; 
    }

    private void OnDetached(Grabbable arg0)
    {
        grabed = null;
    }

}
