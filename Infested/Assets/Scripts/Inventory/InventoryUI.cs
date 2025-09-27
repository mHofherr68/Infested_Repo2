using UnityEngine;
using TMPro;
using System.Text;

public class InventoryUI : MonoBehaviour
{
    [Header("Inventory Settings")]

    [Space(16)]

    // Reference to the player's inventory
    [SerializeField] public Inventory inventoryReference;

    // Reference to the TextMeshPro UI element that displays the inventory Text
    [SerializeField] public TMP_Text inventoryList;

    void Update()
    {

        // Update the inventory display every frame
        UpdateInventoryDisplay();
    }

    void UpdateInventoryDisplay()
    {

        // Exit early if either the inventory or the UI text reference is missing
        if (inventoryReference == null || inventoryList == null)
            return;

        // Use a StringBuilder to build the inventory string
        StringBuilder sb = new StringBuilder();
        sb.AppendLine();

        // Add each item's name to the display string
        foreach (var item in inventoryReference.items)
        {
            sb.AppendLine(item.itemName);
        }

        // Assign the built string to the UI text component
        inventoryList.text = sb.ToString();
    }
}
