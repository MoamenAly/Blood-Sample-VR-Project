using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvertToDefault : MonoBehaviour
{
    private void OnValidate()
    {

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {

            renderer.sharedMaterial.shader = Shader.Find("Sprites/Default");

          //  Invoke(nameof(UpdateRenderQueue), 0.15f);

        }
    }
}
