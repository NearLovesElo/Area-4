using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Put this on the description panel (right side of the inventory). Pure view -
/// just renders whatever ItemData it's told to show.
/// </summary>
public class ItemDescriptionUI : MonoBehaviour
{
    [Tooltip("The group of elements to hide when no item is selected yet.")]
    [SerializeField] private GameObject content;

    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    public void ShowItem(ItemData item)
    {
        if (item == null)
        {
            Clear();
            return;
        }

        content.SetActive(true);
        iconImage.sprite = item.icon;
        nameText.text = item.itemName;
        descriptionText.text = item.description;
    }

    public void Clear()
    {
        content.SetActive(false);
    }
}
