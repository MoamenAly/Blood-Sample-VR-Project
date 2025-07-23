using Sirenix.OdinInspector;
using UnityEngine;

public class MaterialTextureChanger : MonoBehaviour
{
    [Header("Target Material Settings")]
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Texture newTexture;

[Button]
public void ChangeBaseTexture()
    {
        if (targetRenderer == null || newTexture == null)
        {
            Debug.LogWarning("Missing targetRenderer or newTexture");
            return;
        }

        Material material = targetRenderer.material;
        material.SetTexture("_BaseMap", newTexture); 
    }
}