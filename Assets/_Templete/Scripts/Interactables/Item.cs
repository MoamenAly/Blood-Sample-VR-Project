using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IItem
{
    [SerializeField] protected bool showGizmo = true;
    [SerializeField, ShowIf(nameof(showGizmo))] protected float GizmosSize = 0.5f;
    public Elements id;

    protected virtual void OnDrawGizmos()
    {
        if (!showGizmo) return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.up * GizmosSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * GizmosSize);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.right * GizmosSize);
    }

}
