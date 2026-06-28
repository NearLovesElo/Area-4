/// <summary>
/// Single place that knows how to apply a consumable's effect and remove it
/// from inventory. Static because it's a stateless operation - call it from
/// anywhere (UI button, a future hotkey/quickbar, etc.) without needing a
/// scene reference. Adding a new ItemType's effect means adding one case here.
/// </summary>
public static class ConsumableUseHandler
{
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
    }
}