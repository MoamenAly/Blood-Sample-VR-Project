using BNG;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomUIPointer : UIPointer
{
    public GameObject draggedElement;
    protected GameObject cursorImage;
    public Vector3 originalCursorShapeScale;
    public Vector3 moveCursorShapeScale;

    public Sprite originalPointerSprite;
    public Sprite movePointerSprite;

    public bool PointerVisabality;
    public UnityAction Grabbed;
    public UnityAction Released;
    public UnityAction Exited;
    public GameObject CURSOR_IMAGE
    {
        get { return cursorImage; }
    }

    protected override void Awake()
    {
        if (cursor)
        {
            _cursor = GameObject.Instantiate(cursor);
            _cursor.transform.SetParent(transform);
            _cursorInitialLocalScale = transform.localScale;
            cursorImage = _cursor.transform.GetChild(0).gameObject;
            originalCursorShapeScale = cursorImage.transform.localScale;
        }

        // If no Line Renderer was specified in the editor, check this Transform
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

#if XRIT_INTEGRATION
            // Setup XRIT info
            if (VRUISystem.Instance.UseXRInteractionToolkitUISystem) {

                // Add the XRaycaster if it's not currently attached
                SetupXRITRaycaster();
            }
            else {
                uiSystem = VRUISystem.Instance;
            }
#else
        uiSystem = VRUISystem.Instance;
#endif
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.G) || InputBridge.Instance.RightTriggerDown) // Placeholder for VR controller button press
        {
            if (draggedElement != null)
            {
                Grabbed?.Invoke();
                //  draggedElement.GetComponent<DragAndDropFeature>().Grab();
                //draggedElement.GetComponent<DragDrop>().Grab();
            }

        }

        if (Input.GetKeyUp(KeyCode.G) || InputBridge.Instance.RightTriggerUp) // Placeholder for VR controller button release
        {
            if (draggedElement != null)
            {
                Released?.Invoke();
                // draggedElement.GetComponent<DragAndDropFeature>().Release();
                // draggedElement.GetComponent<DragAndDropFeature>().DraggedToSlot();

                //draggedElement.GetComponent<DragDrop>().Release();
                //draggedElement.GetComponent<DragDrop>().DraggedToSlot();
            }

            draggedElement = null;
        }
    }
    public override void UpdatePointer()
    {
        data = uiSystem.EventData;

        // Can bail early if not looking at anything
        if ((data == null || data.pointerCurrentRaycast.gameObject == null) && !PointerVisabality)
        {

            HidePointer();

            return;
        }


        // Set position of the cursor
        if (_cursor != null)
        {

            bool lookingAtUI = data.pointerCurrentRaycast.module.GetType() == typeof(GraphicRaycaster);
            selectedPointerEvents = data.pointerCurrentRaycast.gameObject.GetComponent<PointerEvents>();
            bool lookingAtPhysicalObject = selectedPointerEvents != null;

            // Are we too far away from the Physics object now?
            if (lookingAtPhysicalObject && !PointerVisabality)
            {
                if (data.pointerCurrentRaycast.distance > selectedPointerEvents.MaxDistance)
                {
                    HidePointer();
                    return;
                }
            }

            // Can bail immediately if not looking at a UI object or an Object with PointerEvents on it
            if (!lookingAtUI && !lookingAtPhysicalObject && !PointerVisabality)
            {
                HidePointer();
                return;
            }

            // Set as local position
            float distance = Vector3.Distance(transform.position, data.pointerCurrentRaycast.worldPosition);
            _cursor.transform.localPosition = new Vector3(0, 0, distance - 0.0001f);
            _cursor.transform.rotation = Quaternion.FromToRotation(Vector3.forward, data.pointerCurrentRaycast.worldNormal);

            // Scale cursor based on distance from main camera
            float cameraDist = Vector3.Distance(Camera.main.transform.position, _cursor.transform.position);
            _cursor.transform.localScale = _cursorInitialLocalScale * Mathf.Clamp(cameraDist, CursorMinScale, CursorMaxScale);

            _cursor.SetActive(data.pointerCurrentRaycast.distance > 0);
            //print("cursor position"+ _cursor.transform.position);
        }

        // Update linerenderer
        if (lineRenderer)
        {
            lineRenderer.useWorldSpace = false;
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, new Vector3(0, 0, Vector3.Distance(transform.position, data.pointerCurrentRaycast.worldPosition) * LineDistanceModifier));
            lineRenderer.enabled = data.pointerCurrentRaycast.distance > 0;
        }
    }

    public override void HidePointer()
    {
        // Hide the line and cursor
        if (HidePointerIfNoObjectsFound)
        {
            _cursor.SetActive(false);
            lineRenderer.enabled = false;
        }
        // Show a fixed length line
        else
        {
            if (_cursor)
            {
                _cursor.SetActive(false);
            }

            // Set length to fixed amount
            if (lineRenderer)
            {
                lineRenderer.useWorldSpace = false;
                lineRenderer.SetPosition(0, Vector3.zero);
                lineRenderer.SetPosition(1, new Vector3(0, 0, FixedPointerLength * LineDistanceModifier));
                lineRenderer.enabled = true;
            }
        }
    }

    public void EnterElement(GameObject draggedItem)
    {
        originalCursorShapeScale = cursorImage.transform.localScale;
        cursorImage.transform.localScale = moveCursorShapeScale;
        cursorImage.GetComponent<Image>().sprite = movePointerSprite;


        draggedElement = draggedItem;
    }


    public void ExitElement()
    {
        cursorImage.transform.localScale = originalCursorShapeScale;
        cursorImage.GetComponent<Image>().sprite = originalPointerSprite;

        if (draggedElement == null) return;

        Exited?.Invoke();

    }
}