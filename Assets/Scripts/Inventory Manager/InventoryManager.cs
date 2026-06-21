using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on a persistent GameObject in your scene (e.g. an empty "GameManagers" object,
/// same idea as InteractionController). Owns the actual inventory data.
///
/// This script knows NOTHING about UI - it just stores slots and fires OnInventoryChanged.
/// InventoryUI subscribes to that event and redraws itself. That separation is what lets
/// you add new ways to gain/lose items later (crafting, trading, drops) without ever
/// touching the UI code.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public IReadOnlyList<InventorySlot> Slots => _slots;
    private readonly List<InventorySlot> _slots = new List<InventorySlot>();

    /// <summary>Fired any time slots are added, removed, or their quantities change.</summary>
    public event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Adds 'amount' of 'item', filling existing partial stacks first and creating
    /// new stacks for whatever doesn't fit (handles maxStackSize automatically).
    /// </summary>
    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        int remaining = amount;

        foreach (var slot in _slots)
        {
            if (remaining <= 0) break;
            if (slot.item != item || slot.IsFull) continue;

            int space = slot.item.maxStackSize - slot.quantity;
            int added = Mathf.Min(space, remaining);
            slot.quantity += added;
            remaining -= added;
        }

        while (remaining > 0)
        {
            int added = Mathf.Min(item.maxStackSize, remaining);
            _slots.Add(new InventorySlot(item, added));
            remaining -= added;
        }

        OnInventoryChanged?.Invoke();
    }

    /// <summary>Removes up to 'amount' of 'item', clearing out emptied slots.</summary>
    public void RemoveItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0) return;

        int remaining = amount;
        for (int i = _slots.Count - 1; i >= 0 && remaining > 0; i--)
        {
            var slot = _slots[i];
            if (slot.item != item) continue;

            int removed = Mathf.Min(slot.quantity, remaining);
            slot.quantity -= removed;
            remaining -= removed;

            if (slot.quantity <= 0) _slots.RemoveAt(i);
        }

        OnInventoryChanged?.Invoke();
    }

    public int GetItemCount(ItemData item)
    {
        int total = 0;
        foreach (var slot in _slots)
        {
            if (slot.item == item) total += slot.quantity;
        }
        return total;
    }
}
