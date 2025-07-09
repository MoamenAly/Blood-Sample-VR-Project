using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundArrowMotion : MonoBehaviour
{

    public float scrollSpeedX = 0.1f;

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = GetComponent<Renderer>().material.mainTextureOffset;

        // Increase the offset
        offset.x -= scrollSpeedX * Time.deltaTime;

        // Apply the new offset to the material
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}
