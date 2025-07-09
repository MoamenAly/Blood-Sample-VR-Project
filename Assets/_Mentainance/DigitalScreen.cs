using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitalScreen : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text m_TextMeshPro;

    public void Start()
    {
        SetWeight(0);
    }

    public void SetWeight(float value)
    {
        //int intValue = (int)value; // 0    2
        //float decimalvalue = (value - intValue) * 10; //0   5
        //m_TextMeshPro.text = intValue + $".{decimalvalue}00";  //2.500
        string formattedValue = value.ToString("0.000");

        // Update the TextMeshPro text with the formatted value
        m_TextMeshPro.text = formattedValue;
    }
}
