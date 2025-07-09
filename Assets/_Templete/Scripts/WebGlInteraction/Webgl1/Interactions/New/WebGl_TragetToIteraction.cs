using BNG;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebGl_TragetToIteraction : MouseClicked
{

    [SerializeField] float snapTime = 1f;

    [SerializeField] List<string> interactionsSequence = new List<string>();

    int index = 0;

    bool interacted = false;
    
    public override void OnMouseInteract()
    {
        Debug.Log("1");

        var selectedGrabbable = SelectionManager.SelectedGameObject;

        Debug.Log("2");

        if(selectedGrabbable == null || interactionsSequence.Count<index || interacted) return;

        if (interactionsSequence[index]==selectedGrabbable.name)
        {
            Debug.Log("3");
            interacted = true;    
            var grabbaleTransform = selectedGrabbable.transform;
            grabbaleTransform.DOMove(transform.position, snapTime).OnComplete(() => {
                selectedGrabbable.FireRelaseEvent();
                interacted = false;    
                SelectionManager.SelectedGameObject = null;
                index++;
            });
        }
    }




}
