using UnityEngine;
using UnityEngine.EventSystems;
using BNG;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.UI;


#if UNITY_WEBGL || UNITY_STANDALONE_WIN

public class DragDropUI : PointerEvents , IBeginDragHandler, IEndDragHandler,IDragHandler

# else
public class DragDropUI : PointerEvents
#endif
{
    [SerializeField] private RectTransform canvasRectTransform; //used for clamping
    private GameObject dragged;
    protected CustomUIPointer _customUIPointer;


    public Transform controller;
    public float followSpeed = 10f; // Speed at which the object follows the controller
    public float smoothTime = 0.1f; // Smoothing time for the SmoothDamp function

    public bool isGrabbed = false;
    public bool isSelected = false;
    public bool inZone = false;
    private Vector3 velocity = Vector3.zero; // Velocity of the dragged object
    private Vector3 offset;
    public LayerMask _layerMask;


    public UnityEvent SuccessAction;
    public UnityEvent wrongAction;

    public Vector3 SmoothedPos;
    public float limit = 2f;
    private Vector3 InitialPos;

#if UNITY_ANDROID 
    private void Awake()
    {
             _customUIPointer = FindAnyObjectByType<CustomUIPointer>();
            controller =(GameObject.Find("RightController")).transform;
            dragged = gameObject;
            _customUIPointer.Grabbed += Grab;
            _customUIPointer.Released += Release;
            _customUIPointer.Released += DraggedToSlot;
            _customUIPointer.Exited += PointerExited;
        
   

        InitialPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
       
        // Don't call events if exceeded distance
        if (DistanceExceeded(eventData))
        {
            return;
        }
        if (_customUIPointer.draggedElement != null) // be able to drag over other answers
        {
            if (_customUIPointer.draggedElement != gameObject)
            {
                // Release();
                return;
            }
        }
        _customUIPointer.EnterElement(dragged);
        OnSelectObject(); //Select first
        DragDropManager.instance.AssignSelectedDraggable(this); // remove previously selected

        OnPointerEnterEvent?.Invoke(eventData);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
     
        Debug.Log("pointer is down ");
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        _customUIPointer.ExitElement();
     
        // Can call OnPointerExit events even if exceeded distance
        OnPointerExitEvent?.Invoke(eventData);
    }
    void Update()
    {
        //if (platform != RuntimePlatform.Android) return;

        if (inZone) return;
        
        //if (_customUIPointer.draggedElement != null)
        //{
        //    if (_customUIPointer.draggedElement != gameObject)
        //    {
        //        // Release();

        //    }
        //}
        if (isGrabbed && isSelected)
            {
                transform.SetAsLastSibling();
                // Calculate the desired position from the controller's forward vector and fixed distance
                Vector3 targetPosition = controller.position + controller.rotation * offset;
                SmoothedPos = Vector3.SmoothDamp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), ref velocity, smoothTime, followSpeed, Time.deltaTime);
                transform.position = ClampToCanvas(SmoothedPos);
                //GetComponent<RectTransform>().anchoredPosition = ClampToCanvas(smoothedPos);
            }
            else
            {
                ReturnToOriginalPosition();
            }
    }

    [Button]
    public void ChangeCanvasPosition(float x, float y, float z)
    {
        GetComponent<RectTransform>().anchoredPosition = ClampToCanvas(new Vector3(x, y, z));
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

    private void OnDisable()
    {
        _customUIPointer.ExitElement();
    }

#endif

#if UNITY_WEBGL || UNITY_STANDALONE_WIN

    private void Awake()
    {
        InitialPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!inZone) ReturnToOriginalPosition();
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        //GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvasRectTransform.GetComponent<Canvas>().scaleFactor;

        if (!inZone)
        {
            transform.SetAsLastSibling();
            Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Mathf.Abs(transform.position.z - Camera.main.transform.position.z));
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint);
            transform.position = ClampToCanvas( new Vector3(cursorPosition.x, cursorPosition.y, transform.position.z));
        }

    }
#endif

    //--------------------------------------------------  Shared For all ------------------------------------------------------------//
    private Vector3 ClampToCanvas(Vector3 position)
    {

        // Convert the world position to the canvas's local position
        Vector3 localPosition = canvasRectTransform.InverseTransformPoint(position);
        Vector3 minPosition =    canvasRectTransform.rect.min   +  (GetComponent<RectTransform>().rect.size ) / 2f    ;
        Vector3 maxPosition =    canvasRectTransform.rect.max   -  (GetComponent<RectTransform>().rect.size ) / 2f   ;

        //Debug.Log("minPosition: " + minPosition + "  canvasRectTransform.rect.min :  " + canvasRectTransform.rect.min + " GetComponent<RectTransform>().rect.size: " + GetComponent<RectTransform>().rect.size + " canvasRectTransform.localScale: "+ canvasRectTransform.localScale);
        // Clamp the position to stay within the canvas bounds
        localPosition.x = Mathf.Clamp(localPosition.x, minPosition.x, maxPosition.x);
        localPosition.y = Mathf.Clamp(localPosition.y, minPosition.y, maxPosition.y);

        // Convert back to world position
        return canvasRectTransform.TransformPoint(localPosition);
    }
    [Button]
    public void ReturnToOriginalPosition()
    {
        inZone = false;
        transform.position = InitialPos;
    }
    [Button]
    public void MoveToDropZone(Transform zonePos)
    {
        inZone = true;
        transform.position = zonePos.position;
    }

}
