using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class MaterialTransparencyTween : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] [Range(0, 255)] private int targetAlphaByte = 160;
    [SerializeField] private float duration = 1.5f;

    private Material material;

    private void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        material = targetRenderer.material;
        
    }

    [Button]
    public void FadeToTargetAlpha()
    {
        float alpha = targetAlphaByte / 255f;
        Color currentColor = material.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

        material.DOColor(targetColor, duration);
    }
    
}