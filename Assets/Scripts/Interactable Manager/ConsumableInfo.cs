/// <summary>
/// Single source of truth for what each ItemType means in terms of UI text
/// and which player stat it affects. Both InventoryUI (for the button label)
/// and ConsumableUseHandler (for applying the effect) read from here, so
/// adding a new ItemType later means editing exactly this one file.
/// </summary>
public static class ConsumableInfo
{
    public static string GetVerb(ItemType type)
    {
        switch (type)
        {
            case ItemType.Food: return "Eat";
            case ItemType.Water: return "Drink";
            case ItemType.Medicine: return "Use";
            default: return "Use";
        }
    }
}