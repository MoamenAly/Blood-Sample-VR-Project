using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DragAndDropFeature : MonoBehaviour
{
    public GameObject HolderDragged;
    protected CustomUIPointer _customUIPointer;

    public Transform controller;
    public float followSpeed = 10f; // Speed at which the object follows the controller
    public float smoothTime = 0.1f; // Smoothing time for the SmoothDamp function

    public bool isGrabbed = false;
    public bool isSelected = false;

    private Vector3 velocity = Vector3.zero; // Velocity of the dragged object
    private Vector3 offset;
    public LayerMask _layerMask;


    public UnityEvent SuccessAction;
    public UnityEvent wrongAction;


    //public float Threshhold = 1f;
    private void Awake()
    {
        if (PlatformManager.Instance.platform == Platform.Android)
        {
            _customUIPointer = FindAnyObjectByType<CustomUIPointer>();
            _customUIPointer.Grabbed += Grab;
            _customUIPointer.Released += Release;
            _customUIPointer.Released += DraggedToSlot;
            _customUIPointer.Exited += PointerExited;
        }
    }

    void Update()
    {
        if (isGrabbed && isSelected)
        {
            // Calculate the desired position from the controller's forward vector and fixed distance
            Vector3 targetPosition = controller.position + controller.rotation * offset;

            RaycastHit hit;

            // Perform the raycast from current position to the target position
            if (Physics.Raycast(transform.position, targetPosition - transform.position, out hit, Vector3.Distance(transform.position, targetPosition), _layerMask))
            {
                // If there is a hit, stop the object at the hit point, slightly in front of the collision point
                targetPosition = hit.point - (targetPosition - transform.position).normalized * 0.5f; // Adjust this value as necessary to avoid embedding in the wall
            }

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime, followSpeed, Time.deltaTime);

        }
    }

    public void Grab()
    {
        isGrabbed = true;
        offset = controller.InverseTransformPoint(transform.position);
    }

    public void Release()
    {
        isGrabbed = false;
        velocity = Vector3.zero;
        OnUnSelectObject();// Reset velocity when the object is released
    }

    public void OnSelectObject()
    {
        isSelected = true;
    }

    public void OnUnSelectObject()
    {
        isSelected = false;
    }

    public void DraggedToSlot()
    {

        if ((Vector3.Distance(transform.position, HolderDragged.transform.position) < 0.5f) && HolderDragged.gameObject.activeSelf) // Check the proximity
        {
            transform.position = HolderDragged.transform.position;        // Snap to slot
            transform.rotation = HolderDragged.transform.rotation;
            SuccessAction.Invoke();
        }
        else
        {
            Debug.Log("Wrong");
            wrongAction.Invoke();
        }
        _customUIPointer.CURSOR_IMAGE.GetComponent<Image>().sprite = _customUIPointer.originalPointerSprite;
        _customUIPointer.CURSOR_IMAGE.transform.localScale = _customUIPointer.originalCursorShapeScale;
    }

    public void PointerExited()
    {
        if (!isGrabbed)
        {
            _customUIPointer.draggedElement = null;
        }
    }

}
