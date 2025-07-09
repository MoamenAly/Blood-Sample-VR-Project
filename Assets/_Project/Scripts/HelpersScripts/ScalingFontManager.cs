using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScalingFontManager : MonoBehaviour
{
    [SerializeField] private float minFontsize;
    [SerializeField] private float maxFontsize;
    [SerializeField] private float scalevalue;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private Button ScaleUpButton;
    [SerializeField] private Button ScaleDownButton;

    public void Start()
    {
        ScaleUpButton.onClick.AddListener(ScaleUpFontText);
        ScaleDownButton.onClick.AddListener(ScaleDownFontText);
    }

    public void ScaleUpFontText()
    {
        if (text.fontSize < maxFontsize)
            text.fontSize += scalevalue;
    }

    public void ScaleDownFontText()
    {
        if (text.fontSize > minFontsize)
            text.fontSize -= scalevalue;
    }
}
