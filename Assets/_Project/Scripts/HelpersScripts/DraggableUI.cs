using BNG;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour
{
    public GameObject dragged;
    protected PointerEvents _events;
    protected CustomUIPointer _customUIPointer;

    private void Awake()
    {
        _events = GetComponent<PointerEvents>();
        _customUIPointer = FindAnyObjectByType<CustomUIPointer>();
    }

    private void OnEnable()
    {
        _events.OnPointerEnterEvent.AddListener(onPointerEnter);
        _events.OnPointerExitEvent.AddListener(onPointerExit);
    }

    private void onPointerExit(PointerEventData data)
    {
        _customUIPointer.ExitElement();
    }

    private void onPointerEnter(PointerEventData data)
    {
        _customUIPointer.EnterElement(dragged);
        dragged.GetComponent<DragAndDropFeature>().OnSelectObject();
    }
}
