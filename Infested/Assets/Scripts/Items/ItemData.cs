using UnityEngine;

// Create a new item asset from the Unity editor via the "Assets > Create > Inventory > Item" menu
[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/Inventory/Item")]
public class ItemData : ScriptableObject
{

    [Header("Item Data Parameter")]

    [Space(16)]

    // The display name of the item
    public string itemName;

    // A unique identifier for the item
    public int itemID;

    // Item value
    public float itemValue;
}
