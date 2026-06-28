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

    [Header("Consumable")]
    [Tooltip("What kind of item this is. Controls the Use Button's verb and which stat it restores. Set to None for items that can't be consumed (car parts, key items, etc.).")]
    public ItemType itemType = ItemType.None;

    [Tooltip("How much the relevant stat is restored when used. Ignored if itemType is None.")]
    public float restoreAmount = 100f;
}

/// <summary>
/// What a consumable item does when used. Add new types here as needed
/// (e.g. Radiation for an anti-rad pill) - everything that reads this enum
/// (button text, effect routing) is a single small lookup, so adding a type
/// only ever means touching ItemType + ConsumableEffects, never the UI flow.
/// </summary>
public enum ItemType
{
    None,   // not consumable (car parts, key items, etc.)
    Food,
    Water,
    Medicine
}
