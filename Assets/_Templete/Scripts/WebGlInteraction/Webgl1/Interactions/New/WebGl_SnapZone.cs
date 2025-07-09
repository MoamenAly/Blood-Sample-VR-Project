using BNG;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WebGl_SnapZone : MouseClicked
{
    [SerializeField] private  float updistance = 0.1f;
    SnapZone snapZone;

    [SerializeField] float snapTime = 1f;

    [SerializeField] LayerMask mask;

    private void Start()
    {


        snapZone = GetComponent<SnapZone>();

        bool web = PlatformManager.Instance.platform == Platform.Webgl;
        if (web)
        {

            GetComponent<Collider>().enabled = false;

            gameObject.layer = LayerMaskToLayer(mask);
        }



    }

    public override void OnWebMouseDown()
    {       
        OnMouseInteract();
    }

    public override void OnMouseInteract()
    {
        Debug.Log("OnMouseInteract" + gameObject.name);

        var selectedGrabbable = SelectionManager.SelectedGameObject;
        if (!snapZone.enabled || selectedGrabbable == null) return;
        Debug.Log("D0" + gameObject.name + selectedGrabbable.name);

        if (snapZone.OnlyAllowNames.Contains(selectedGrabbable.name))
        {
            var grabbaleTransform = selectedGrabbable.transform;
           
                Vector3 []points = GetPath(grabbaleTransform.position);
             
                    grabbaleTransform.DOPath(points, snapTime*2.5f).OnComplete(() =>
                    {
                        selectedGrabbable.transform.parent = transform;
                        selectedGrabbable.transform.localRotation = Quaternion.identity;
                        selectedGrabbable.FireRelaseEvent();
                        snapZone.OnSnapEvent?.Invoke(selectedGrabbable.grabbable);
                        SelectionManager.SelectedGameObject = null;
                    });
                   SelectionManager.SelectedGameObject = null;                
            }
    }

    private Vector3[] GetPath(Vector3 objectPoistion)
    {
        float higgerY = transform.position.y < objectPoistion.y ? objectPoistion.y : transform.position.y;
    
        Vector3[] points = new Vector3[3];
        Vector3 point1 = objectPoistion;
        point1.y = higgerY;

        //0
        points[0] = point1 + Vector3.up* updistance;

        Vector3 point2 = transform.position;
        point2.y = higgerY;

        //1
        points[1] = point2 + Vector3.up*(updistance);

        //2
        points[2] = transform.position;

        return points;
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //Gizmos.DrawWireSphere(GetPath(SelectionManager.SelectedGameObject.transform.position),0.01f);
    }

    private int LayerMaskToLayer(LayerMask layerMask)
    {
        int layer = 0;
        int layerMaskValue = layerMask.value;
        while (layerMaskValue > 0)
        {
            layerMaskValue >>= 1;
            layer++;
        }
        return layer - 1;
    }


    [SerializeField] LayerMask obstacleLayer;
    public float avoidStrength = 5f;
    public float avoidDistance = 2f;

    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition,Action OnComplete)
    {
        var selectedGrabbable = SelectionManager.SelectedGameObject;
        var grabbableTransform = selectedGrabbable.transform;
        float obstacleCheckDistance = 0.1f;

        Vector3 direction = (targetPosition - grabbableTransform.position).normalized;
        float distance = Vector3.Distance(grabbableTransform.position, targetPosition);
        Vector3 currentPosition = grabbableTransform.position;

        while (distance > 0.1f)
        {
            // Avoid obstacles
            RaycastHit hit;
            Vector3 avoidance = Vector3.zero;
            if (Physics.SphereCast(currentPosition, 0.5f, direction, out hit, obstacleCheckDistance, obstacleLayer))
            {
                Vector3 hitNormal = hit.normal;
                avoidance = hitNormal * avoidStrength;
                avoidance = Vector3.ClampMagnitude(avoidance, avoidDistance);
            }

            Vector3 newDirection = (direction + avoidance).normalized;
            currentPosition = Vector3.MoveTowards(currentPosition, currentPosition + newDirection, snapTime * Time.deltaTime);

            grabbableTransform.position = currentPosition;
            distance = Vector3.Distance(currentPosition, targetPosition);

            yield return null;
        }

        // Ensure the object reaches the exact target position
        grabbableTransform.DOMove(targetPosition, snapTime).OnComplete(()=>{
            OnComplete?.Invoke();
        });
    }

}
