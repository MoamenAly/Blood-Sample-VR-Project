using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using System;

public class RadialProgress : MonoBehaviour
{


    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Image image;
    [SerializeField] float speed = 30;

    [SerializeField] string finishedmessage; 
    float currentvalue;

    public UnityEvent2 OnProgreesBarCompleted;

    bool DoOnce = true;
    bool progressstarted = false;

    private void Start()
    {
        _timerText.text = finishedmessage;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.forward =transform.position - Camera.main.transform.position ;

        if (!progressstarted) return;

        if(currentvalue < 100)
        {
            currentvalue += speed * Time.deltaTime;
           // _timerText.text = ((int)currentvalue).ToString() + "%";
        }
        else
        {    
            if (DoOnce)
            {
                //_timerText.text = finishedmessage;

                Invoke(nameof(SetTimerCompleted),1);
                
                StopProgressbar();
                DoOnce = false;
            }

        }
        image.fillAmount = currentvalue / 100;
    }

    private void SetTimerCompleted()
    {
        OnProgreesBarCompleted?.Invoke();
    }

    [Button("Start Progress Bar")]
   public void StartProgressBar()
    {

        progressstarted = true;
    }


    void StopProgressbar()
    {

        progressstarted = false;
    }



    [Button("Reset Progress Bar")]
    void ResetProgressBar()
    {
         DoOnce = true;
         progressstarted = false;
        currentvalue = 0;
        image.fillAmount = 0;
        _timerText.text = "";
    }
}
