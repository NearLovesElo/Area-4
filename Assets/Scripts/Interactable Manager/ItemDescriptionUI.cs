using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Put this on the description panel (right side of the inventory). Shows
/// whatever ItemData it's told to show, and drives the Use Button's label +
/// click behavior based on that item's ItemType. Doesn't know HOW to apply
/// effects - delegates that to ConsumableUseHandler.
/// </summary>
public class ItemDescriptionUI : MonoBehaviour
{
    [Tooltip("The group of elements to hide when no item is selected yet.")]
    [SerializeField] private GameObject content;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    [Header("Use Button")]
    [SerializeField] private Button useButton;
    [SerializeField] private TextMeshProUGUI useButtonText;

    private ItemData _currentItem;

    private void Awake()
    {
        useButton.onClick.AddListener(OnUseButtonClicked);
    }

    public void ShowItem(ItemData item)
    {
        _currentItem = item;

        if (item == null)
        {
            Clear();
            return;
        }

        content.SetActive(true);
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.description;

        bool isConsumable = item.itemType != ItemType.None;
        useButton.gameObject.SetActive(isConsumable);

        if (isConsumable)
        {
            useButtonText.text = ConsumableInfo.GetVerb(item.itemType);
        }
    }

    public void Clear()
    {
        _currentItem = null;
        content.SetActive(false);
    }

    private void OnUseButtonClicked()
    {
        if (_currentItem == null) return;

        ConsumableUseHandler.Use(_currentItem);

        // Item may now be gone or reduced - clear the panel since InventoryUI's
        // Refresh (triggered by OnInventoryChanged) will rebuild slots anyway.
        Clear();
    }
}