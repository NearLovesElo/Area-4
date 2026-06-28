using UnityEngine;

/// <summary>
/// Put this on any persistent GameManagers-style GameObject. Listens for the
/// FIRST time the player uses medicine, advances the objective from
/// "Use Medicine" to "Leave the House", and unlocks the door to outside by
/// flipping its Collider2D from solid wall to passable trigger.
/// </summary>
public class TutorialUseMedicine : MonoBehaviour
{
    [SerializeField] private ItemType watchedType = ItemType.Medicine;
    [SerializeField] private Collider2D doorToOutsideCollider;

    private void OnEnable()
    {
        ConsumableUseHandler.OnItemUsed += HandleItemUsed;
    }

    private void OnDisable()
    {
        ConsumableUseHandler.OnItemUsed -= HandleItemUsed;
    }

    private void HandleItemUsed(ItemData item)
    {
        if (item.itemType != watchedType) return;

        ObjectiveManager.Instance.TryCompleteStep("USE_MEDICINE");

        if (doorToOutsideCollider != null)
        {
            doorToOutsideCollider.isTrigger = true;
        }
    }
}