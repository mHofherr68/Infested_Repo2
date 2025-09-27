using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpController : MonoBehaviour
{
    // Reference to the PlayerInput component
    private PlayerInput playerInput;  
    
    // Reference to the player's inventory
    private Inventory inventory;

    [Header("Item at Player settings")]

    [Space(16)]

    // Select item at Player, (e.g., flashlight, Weapon) when is picked up.
    public GameObject selectItem; 
    
    [Header("UI Text Settings")]

    [Space(16)]

    // selected UI Text in "GameTextPanel", shown when the player is near an item
    public GameObject selectUiText;

    // Stores the collider of the item currently in range
    private Collider currentItemCollider;     

    void Start()
    {

        // Hide the UI Text, in GameObject selectUiText, initially
        selectUiText.SetActive(false);

        // Get references to PlayerInput and Inventory components
        playerInput = GetComponent<PlayerInput>();
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {

        // Check if an item is in range and the interact key ("E") was pressed this frame
        if (currentItemCollider != null && playerInput.actions
            ["UseThings"].WasPressedThisFrame())
        {

            // Try to get the Item component from the current collider
            Item item = currentItemCollider.GetComponent<Item>();
            if (item != null && item.data != null)
            {

                // Add the item data to the inventory
                inventory.AddItem(item.data);

                // If the item is a Weapon, activate the player Weapon object
                if (item.data.itemName == "Spraygun")
                {
                    selectItem.SetActive(true);
                } 

                // Hide the interaction UI Panel
                selectUiText.SetActive(false);

                // Destroy the picked-up item in the scene
                Destroy(currentItemCollider.gameObject);

                // Clear the current item collider
                currentItemCollider = null;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        // If the player enters the trigger area of an item
        if (other.CompareTag("Item"))
        {
            currentItemCollider = other;

            // Show the interaction UI Panel
            selectUiText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        // If the player exits the trigger area of the current item
        if (other == currentItemCollider)
        {
            // Clear the item reference and hide the interaction UI Panel
            currentItemCollider = null;
            selectUiText.SetActive(false);
        }
    }
}
