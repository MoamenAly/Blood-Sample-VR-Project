using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanerHelper : MonoSinglton<CleanerHelper>
{

    public List<ItemToClean> itemsToClean; // List to store found objects with ItemToClean script

    void Start()
    {
        // Find all objects with the ItemToClean script
        ItemToClean[] objects = FindObjectsOfType<ItemToClean>();

        // Initialize the list and add found objects to it
        itemsToClean = new List<ItemToClean>(objects);

        // Log the number of objects found
        Debug.Log("Found " + itemsToClean.Count + " objects with ItemToClean script.");

        // Optionally, iterate through the list and log each object
        foreach (ItemToClean item in itemsToClean)
        {
            Debug.Log("Found item to clean: " + item.gameObject.name);
        }
    }


    public void SetObjectActive(bool _state)
    {

        foreach (ItemToClean item in itemsToClean)
        {
            if (item.GetComponent<ObjectClickInteraction>())
            {
                item.GetComponent<ObjectClickInteraction>().CanClick = _state;
            }
        }
    }
}
