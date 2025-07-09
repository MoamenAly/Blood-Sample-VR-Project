using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UIDropZone : MonoBehaviour
{
    public UnityEvent<GameObject> OnSnapEvent;
    public UnityEvent<GameObject> OnDetachEvent;

    [ReadOnly] [SerializeField] private GameObject CollidedWith;

    public bool HasElement = false;
    private void OnTriggerEnter(Collider other)
    {
        if (HasElement) return;
        if(other.gameObject.GetComponent<DragDropUI>() != null)
        {
            CollidedWith = other.gameObject;

            CollidedWith.GetComponent<DragDropUI>().inZone = true;
            // CollidedWith.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            CollidedWith.transform.position = transform.position;
            OnSnapEvent?.Invoke(CollidedWith);
            HasElement = true;
            ValidateAnswer();
        }
    }

    private void ValidateAnswer()
    {
        StartCoroutine(CheckOnDoubleCollision());
    }
    private IEnumerator CheckOnDoubleCollision()
    {
        yield return new WaitForSeconds(1);
        if (HasElement)
        {
            Debug.Log("CollidedWith.transform.position:  " + CollidedWith.transform.position + "    transform.position:  " + transform.position + " IN " + gameObject.name);
            if (CollidedWith.transform.position != transform.position)
            {

                OnDetachEvent?.Invoke(CollidedWith);
                CollidedWith = null;
                HasElement = false;
            }
        }
    }

    public void ReleaseAll()
    {
        OnDetachEvent?.Invoke(CollidedWith);
        CollidedWith.GetComponent<DragDropUI>().ReturnToOriginalPosition();
        CollidedWith = null;
        HasElement = false;
    }
}
