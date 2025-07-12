using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafteyManager : MonoSinglton<SafteyManager>
{


    public List<SafteyItem> Items;


   public void SubscribeItem(SafteyItem item)
    {
        Items.Add(item);
    }

    int NextItemOrderTotake = 1;

    public void CheckSaftey(SafteyItem item)
    {
      
        if(NextItemOrderTotake == item.Order)
        {
            print("Takee it"); /// sunscribe to ui manager

            item.OnTakeitemSucces();
            if (NextItemOrderTotake > Items.Count + 1)  return;
            NextItemOrderTotake++;
        }
        else
        {


            print("Don't Take it");  ///  to ui manager
        }
    }
}
