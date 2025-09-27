using UnityEngine;

public class Item : MonoBehaviour
{

    // Reference to the data associated with this item by the script "itemData"

    [Header("Select SC itemdata")]

    [Space(16)]

    [SerializeField] public ItemData data;
}
