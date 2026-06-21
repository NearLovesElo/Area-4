using UnityEngine;

/// <summary>
/// Put this on the SAME GameObject as the item's Collider2D and InteractableTrigger
/// (interactableSource on InteractableTrigger should point to this component) -
/// exactly the same pattern as Radio. Copy this object to make Canned Food,
/// Bottled Water, Medkit, etc., just swapping out the ItemData asset.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;

    private bool _collected;

    public bool CanInteract() => !_collected;

    public void Interact()
    {
        if (_collected) return;
        _collected = true;

        InventoryManager.Instance.AddItem(itemData, quantity);

        // Explicitly unregister BEFORE destroying. We can't rely on OnTriggerExit2D
        // firing reliably when a collider is destroyed mid-frame, and leaving a stale
        // (destroyed) entry in InteractionController's _inRange list would throw
        // MissingReferenceException the next time it's evaluated.
        InteractionController.Instance.UnregisterInteractable(this);

        Destroy(gameObject);
    }
}
