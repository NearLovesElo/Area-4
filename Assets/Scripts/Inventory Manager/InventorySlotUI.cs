using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Put this on the slot PREFAB (Background + Icon Image + Count Text + Button).
/// Purely a "view" - it displays whatever InventorySlot data it's given and reports
/// clicks back to InventoryUI. It never talks to InventoryManager directly.
/// </summary>
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button button;

    private InventorySlot _slot;
    private InventoryUI _owner;

    public void Setup(InventorySlot slot, InventoryUI owner)
    {
        _slot = slot;
        _owner = owner;

        iconImage.sprite = slot.item.icon;
        iconImage.enabled = true;
        countText.text = slot.quantity.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => _owner.SelectSlot(_slot));
    }
}
