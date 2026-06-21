using UnityEngine;

/// <summary>
/// Defines a TYPE of item (Canned Food, Bottled Water, Medkit...), not an instance.
/// Create one asset per item via Assets > Create > Inventory > Item Data.
/// Both InventoryManager and the pickups in the world reference these assets,
/// so adding a new item never requires touching code.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("Identification")]
    [Tooltip("Unique key, used internally (e.g. 'canned_food'). Not shown to the player.")]
    public string itemId;

    public string itemName;

    [Header("Display")]
    public Sprite icon;

    [TextArea(2, 5)]
    public string description;

    [Header("Stacking")]
    [Tooltip("How many of this item can sit in a single inventory slot.")]
    public int maxStackSize = 99;
}
