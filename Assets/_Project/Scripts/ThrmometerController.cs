using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

using DG.Tweening;
using Sirenix.OdinInspector;

public class ThrmometerController : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _Temp;
    public int Currentindex;

    float temperature = 0;


    public List<TemperatureValues> temperatureValues;

    [Button]
    public void ChangeTemperature()
    {


        temperatureValues[Currentindex].OnStart?.Invoke();

        DOTween.To(() => temperature, x => temperature = x, temperatureValues[Currentindex].TargetTemperature, temperatureValues[Currentindex].TargetTime)
                   .OnUpdate(() => UpdateTemperatureDisplay())
                   .SetEase(Ease.Linear);
    }

    void UpdateTemperatureDisplay()
    {
        // Update the UI text with the current temperature value, formatted to 1 decimal place
        _Temp.text = temperature.ToString("F1") + " °C";

        if(temperatureValues[Currentindex].TargetTemperature == temperature)
        {
            temperatureValues[Currentindex].OnTargetTempAchived?.Invoke();
            Currentindex++;
        }
    }


}



[System.Serializable]
public class TemperatureValues

{
    public UnityEvent2 OnStart;

    public float TargetTemperature;
    public float TargetTime;

    public UnityEvent2 OnTargetTempAchived;
}
