using BNG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ToolsSnapZoneManager : MonoBehaviour
{

    public UnityEvent2 OnprepareAllTools;

    public List<SnapZone> snapZones ;

    public int itemsCount = 0;
    [SerializeField] UiTimer uiTimer;
    [SerializeField] private  ScoreManager scoreManager;



    private void Start()
    {
        uiTimer.OnTimeFinishedAction += SetScore;

    }
    public void OnPrepareTool()
    {
        itemsCount++;
        if(itemsCount == snapZones.Count)
        {

            SetScore();
        }
    }


    public void SetScore()
    {
        if(itemsCount== snapZones.Count)
        {
            scoreManager.IncreaseScore(60);

        }
        if(itemsCount< snapZones.Count)
        {
            int counttodecrease = 60 - ((snapZones.Count - itemsCount)*5);
            scoreManager.IncreaseScore(counttodecrease);
        }
        uiTimer.StopTimer();
        OnprepareAllTools?.Invoke();

    }







}
