/// <summary>
/// One stack of a single item type inside the inventory (e.g. "3x Canned Food").
/// Plain C# class - not a MonoBehaviour - because it's pure data, not a scene object.
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity;

    public InventorySlot(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsFull => quantity >= item.maxStackSize;
}
