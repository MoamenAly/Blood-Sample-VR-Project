using System;
using UnityEngine;

namespace VRUIP.Drawing
{
    /// <summary>
    /// This class is for changing the color of a drawing pen.
    /// </summary>
    public class PenColorChanger : MonoBehaviour
    {
        [Tooltip("Color to change pen to.")]
        public Color color;

        private void Awake()
        {
            //TODO: UPDATE FOR URP
            //var newMat = new Material(Shader.Find("Standard"));
            var newMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            newMat.color = color;
            GetComponent<MeshRenderer>().material = newMat;
        }
    }
}
