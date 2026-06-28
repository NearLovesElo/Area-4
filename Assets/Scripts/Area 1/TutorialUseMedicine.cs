using UnityEngine;

/// <summary>
/// Put this on any persistent GameManagers-style GameObject. Listens for the
/// FIRST time the player uses medicine, advances the objective from
/// "Use Medicine" to "Leave the House". No reveal needed here since the
/// next step is about leaving, not finding an item.
/// </summary>
public class TutorialUseMedicine : MonoBehaviour
{
    [SerializeField] private ItemType watchedType = ItemType.Medicine;

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
    }
}