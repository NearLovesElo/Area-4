using System;

/// <summary>
/// Single place that knows how to apply a consumable's effect and remove it
/// from inventory. Static because it's a stateless operation - call it from
/// anywhere (UI button, a future hotkey/quickbar, etc.) without needing a
/// scene reference. Adding a new ItemType's effect means adding one case here.
/// </summary>
public static class ConsumableUseHandler
{
    /// <summary>
    /// Fired after ANY item is successfully used/eaten/drunk, with the item
    /// that was consumed. Lets one-off systems (tutorial triggers, stats,
    /// achievements) react without ConsumableUseHandler needing to know
    /// they exist.
    /// </summary>
    public static event Action<ItemData> OnItemUsed;

    public static void Use(ItemData item)
    {
        if (item == null || item.itemType == ItemType.None) return;

        switch (item.itemType)
        {
            case ItemType.Food:
                PlayerStats.Instance.RestoreHunger(item.restoreAmount);
                break;
            case ItemType.Water:
                PlayerStats.Instance.RestoreThirst(item.restoreAmount);
                break;
            case ItemType.Medicine:
                PlayerStats.Instance.RestoreHealth(item.restoreAmount);
                break;
        }

        InventoryManager.Instance.RemoveItem(item, 1);

        OnItemUsed?.Invoke(item);
    }
}