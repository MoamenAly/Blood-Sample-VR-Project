using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiglightComponent : MonoBehaviour
{
    private Renderer ComponentRenderer;
    [SerializeField] Color higlightColor = Color.green;
    [SerializeField] float higlightSpeed = 5;
    [SerializeField] float higlightValue = 0.5f;

    [SerializeField] bool ovverideRenderQueue = false;
    [ShowIf(nameof(ovverideRenderQueue))]
    [SerializeField] int renderQueue = 3000;

#if UNITY_EDITOR
    private void OnValidate()
    {

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {      
         
            renderer.sharedMaterial.shader = Shader.Find("CustomShaders/HighLightShader");

            Invoke(nameof(UpdateRenderQueue), 0.15f);
            
        }
    }

    private void UpdateRenderQueue()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (ovverideRenderQueue)
            renderer.sharedMaterial.renderQueue = renderQueue;
    }




#endif

    void Start()
    {
        ComponentRenderer = GetComponent<Renderer>();
        if(ovverideRenderQueue)
            ComponentRenderer.sharedMaterial.renderQueue = renderQueue;
    }
    private void SetHiglightValue(float value)
    {
        if (!gameObject.activeSelf) return;
        foreach (Material material in ComponentRenderer.materials)
        {
            if (material != null && material.HasProperty("_FresnelScale"))
            {
                material.SetColor("_HighLightColor", higlightColor);
                material.SetFloat("_FresnelScale", value);
                material.SetFloat("_AnimationFreq", higlightSpeed);
            }
            else
            {
                Debug.LogWarning("Material or shader property not found.");
            }
        }
    }
    public void _ActiveHiglight()
    {
       

        DOVirtual.DelayedCall(0.01f,
            ()=>{
            if (!gameObject.activeSelf)
                    return;
               SetHiglightValue(higlightValue);
            }
        );
    }
    public void _DeActiveHiglight()
    {
        if (!gameObject.activeSelf) return;
        SetHiglightValue(0);
    }
    
}
