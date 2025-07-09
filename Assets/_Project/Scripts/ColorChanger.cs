using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using Unity.XR.CoreUtils.Datums; // Only needed if you're using UI Image

public class ColorChanger : MonoBehaviour
{
    [Header("For Renderer (3D Object)")]
    public Renderer targetRenderer;

    [Header("Color Settings")]
    public Color targetColor = Color.red;
    public float duration = 1f;

    [Header("Options")]
    public bool loop = false;
    public Ease easeType = Ease.Linear;
    public float delayBeforeStart = 0f;

    public UnityEvent2 OnChangedColder;
    private Material matInstance;

    void Start()
    {
        if (targetRenderer != null)
        {
            matInstance = targetRenderer.material;
        }
    }

    [Button]
    public void ChangeRendererColor()
    {
        DOVirtual.DelayedCall(delayBeforeStart, () =>
        {
            matInstance.DOColor(targetColor, duration)
                   .SetEase(easeType)
                   .SetLoops(loop ? -1 : 0, LoopType.Yoyo)
                   .OnComplete(() => OnChangedColder?.Invoke());

        });
    }
}
