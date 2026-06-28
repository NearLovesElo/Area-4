using UnityEngine;

/// <summary>
/// Put this on any persistent GameManagers-style GameObject (same idea as
/// ObjectiveManager/InventoryManager). Listens for the FIRST time the
/// player eats food, advances the objective from "Eat Food" to "Get Water",
/// and reveals the water item in the scene.
///
/// Only fires once, ever - same one-shot guard pattern as Radio's
/// foodToEnable reveal. Safe even if the player eats multiple food items,
/// since ObjectiveManager.IsStepComplete already no-ops repeats.
/// </summary>
public class TutorialEatFood : MonoBehaviour
{
    [SerializeField] private ItemType watchedType = ItemType.Food;
    [SerializeField] private GameObject waterToEnable;

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

        // Same first-time check style as Radio: ask BEFORE completing,
        // so we know whether this is the moment that finishes the step.
        bool isFirstTime = !ObjectiveManager.Instance.IsStepComplete("EAT_FOOD");

        ObjectiveManager.Instance.TryCompleteStep("EAT_FOOD");

        if (isFirstTime && waterToEnable != null)
        {
            waterToEnable.SetActive(true);
        }
    }
}