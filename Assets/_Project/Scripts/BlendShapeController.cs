using UnityEngine;
using DG.Tweening;

public class BlendShapeController : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private int blendShapeIndex = 0;
    [SerializeField] private float duration = 1f;

    private void Start()
    {
        skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
    }

    // Tween from 0 to 1 (which is 0% to 100%)
    public void TweenBlendShape01()
    {
        DOTween.To(() => 0f,
                   value => skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, value * 100f),
                   1f,
                   duration);
    }

    // Tween from 1 to 0 (which is 100% to 0%)
    public void InverseTweenBlendShape01()
    {
        DOTween.To(() => 1f,
                   value => skinnedMeshRenderer.SetBlendShapeWeight(blendShapeIndex, value * 100f),
                   0f,
                   duration);
    }
}
