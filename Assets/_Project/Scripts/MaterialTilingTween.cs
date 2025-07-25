using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

public class MaterialTilingTweenX : MonoBehaviour
{
    [Header("Target Material Settings")]
    [SerializeField] private Material targetRendererMaterial;
    [SerializeField] private float targetTilingX = 2f;
    [SerializeField] private float duration = 1f;

    private Material materialInstance;

    private void Awake()
    {
        materialInstance = targetRendererMaterial;

        // Fix: assign the full Vector2
        Vector2 tiling = materialInstance.mainTextureScale;
        tiling.x = 0f;
        materialInstance.mainTextureScale = tiling;
    }

    [Button]
    public void TweenXTiling()
    {
        Vector2 currentTiling = materialInstance.mainTextureScale;

        DOTween.To(() => currentTiling.x, x =>
            {
                currentTiling.x = x;
                materialInstance.mainTextureScale = currentTiling;
            },
            targetTilingX, duration);
    }
}