using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float timeRemaining = 0;
    private bool timeisRunning = false;
    public TMP_Text timeText;

    // Start is called before the first frame update
    void Start()
    {
        //timeisRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeisRunning)
        {
            if(timeRemaining >= 0)
            {
                timeRemaining += Time.deltaTime;
                DisplayTime(timeRemaining);
            }
        }
        
    }
    public void ResetTimer()
    {
        timeRemaining = 0;
    }
    public void StartTime()
    {
        timeisRunning = true;
    }
    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}: {1:00}",minutes,seconds);

    }
    public string getCurrentTime()
    {
        return timeText.text;
    }
}
