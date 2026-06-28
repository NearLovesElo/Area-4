using UnityEngine;

/// <summary>
/// Put this on any persistent GameManagers-style GameObject. Listens for the
/// FIRST time the player drinks water, advances the objective from
/// "Drink Water" to "Get Medicine", and reveals the medicine item in the scene.
///
/// Same one-shot guard pattern as TutorialEatFood.
/// </summary>
public class TutorialDrinkWater : MonoBehaviour
{
    [SerializeField] private ItemType watchedType = ItemType.Water;
    [SerializeField] private GameObject medicineToEnable;

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

        bool isFirstTime = !ObjectiveManager.Instance.IsStepComplete("DRINK_WATER");

        ObjectiveManager.Instance.TryCompleteStep("DRINK_WATER");

        if (isFirstTime && medicineToEnable != null)
        {
            medicineToEnable.SetActive(true);
        }
    }
}