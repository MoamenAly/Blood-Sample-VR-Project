using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RTLTMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
public class SafteyManager : MonoSinglton<SafteyManager>
{


    public List<OrderItem> Items;

    [SerializeField] private LocalizedString SafteyWaring;
    [SerializeField] private AudioSource wrongsource;
    [SerializeField] private LocalizeStringEvent warningText;




    private void Start()
    {
        warningText.gameObject.SetActive(false);
    }
    public void SubscribeItem(OrderItem item)
    {
        Items.Add(item);
    }

public    int NextItemOrderTotake = 1;

    public void CheckSaftey(OrderItem item)
    {
      
        if(NextItemOrderTotake == item.Order)
        {
            print("Takee it"); /// sunscribe to ui manager

            item.OnTakeitemSucces();
            if (NextItemOrderTotake > Items.Count + 1)  return;
        }
        else
        {
            StartCoroutine(ShowSafteyWarning());
            wrongsource.Play();
            warningText.StringReference = SafteyWaring;
         
        }
    }


    IEnumerator ShowSafteyWarning()
    {
        warningText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        warningText.gameObject.SetActive(false);
    }


    public void INCREASEiNDEX()
    {
        NextItemOrderTotake++;
        if(NextItemOrderTotake > Items.Count)
        {
            // increaseScore
            print("INcrease score");
        }

    }



}
