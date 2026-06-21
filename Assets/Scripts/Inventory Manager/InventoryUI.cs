using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on a persistent UI controller object (e.g. on your Canvas, or a child
/// of it). Subscribes to InventoryManager.OnInventoryChanged and rebuilds the slot
/// list whenever it fires - it never needs to know HOW an item was added or removed.
///
/// Wire the Bag Button's OnClick (in the Inspector) to this object's ToggleInventory().
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Slots")]
    [Tooltip("Parent with a Grid Layout Group - spawned slot UIs go here.")]
    [SerializeField] private Transform slotContainer;
    [SerializeField] private InventorySlotUI slotPrefab;

    [Header("Description")]
    [SerializeField] private ItemDescriptionUI descriptionUI;

    private readonly List<InventorySlotUI> _spawnedSlots = new List<InventorySlotUI>();

    private void Start()
    {
        inventoryPanel.SetActive(false);
        descriptionUI.Clear();

        // Safe to subscribe here: Unity runs every object's Awake() before any
        // Start(), and InventoryManager.Instance is assigned in its Awake().
        InventoryManager.Instance.OnInventoryChanged += Refresh;
        Refresh();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= Refresh;
        }
    }

    public void ToggleInventory()
    {
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);
    }

    public void SelectSlot(InventorySlot slot)
    {
        descriptionUI.ShowItem(slot.item);
    }

    private void Refresh()
    {
        foreach (var slotUI in _spawnedSlots)
        {
            Destroy(slotUI.gameObject);
        }
        _spawnedSlots.Clear();

        foreach (var slot in InventoryManager.Instance.Slots)
        {
            InventorySlotUI slotUI = Instantiate(slotPrefab, slotContainer);
            slotUI.Setup(slot, this);
            _spawnedSlots.Add(slotUI);
        }
    }
}
