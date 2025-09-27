using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Generates a dynamic list, containing all collected items in the inventory
    public List<ItemData> items = new List<ItemData>();

    // Adds a new collected item to the inventory
    public void AddItem(ItemData item)
    {
        items.Add(item);
    }
}
