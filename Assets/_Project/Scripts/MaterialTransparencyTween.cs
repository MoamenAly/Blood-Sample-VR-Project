using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class URPTransparencyTween : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField, Range(0, 255)] private int targetAlphaByte = 160;
    [SerializeField] private float duration = 1.5f;

    private Material material;

    private void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // Clone instance
        material = targetRenderer.material;
        
    }

    [Button]
    public void FadeToTargetAlpha()
    {
        float targetAlpha = targetAlphaByte / 255f;
        Color currentColor = material.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);

        material.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f); // Start fully opaque
        material.DOColor(targetColor, duration);
    }
    
    
}