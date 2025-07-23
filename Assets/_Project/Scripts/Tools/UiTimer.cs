using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class UiTimer : MonoBehaviour
{
    public Image fillImage; // Assign in inspector
    private Coroutine fillCoroutine;
    [SerializeField]float duration;

    [SerializeField] UnityEvent2 OnFillFinished;
    /// <summary>
    /// Call this function to start the fill for a duration in seconds
    /// </summary>
    /// 
    [Button]
    public void StartFill()
    {
        if (fillCoroutine != null)
            StopCoroutine(fillCoroutine);

        fillCoroutine = StartCoroutine(FillImageOverTime(duration));
    }

    private IEnumerator FillImageOverTime(float duration)
    {
        fillImage.fillAmount = 0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        OnFillFinished?.Invoke();
        fillImage.fillAmount = 1f;
        fillCoroutine = null;
    }
}
