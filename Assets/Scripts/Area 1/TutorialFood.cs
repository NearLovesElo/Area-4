using UnityEngine;

/// <summary>
/// Put this on the SAME GameObject as ItemPickup, on the ONE specific food
/// item placed in the scene for the tutorial (not on every food item -
/// other food pickups should just use ItemPickup on its own).
///
/// This does NOT duplicate ItemPickup's logic - it just listens for the
/// moment the item is actually collected and advances the objective.
/// Exactly the same pattern as Radio.OnBroadcastFinished, but for a pickup
/// instead of a dialogue.
/// </summary>
[RequireComponent(typeof(ItemPickup))]
public class TutorialFood : MonoBehaviour
{
    private void OnDestroy()
    {
        // ItemPickup destroys this GameObject right after a successful
        // AddItem call, so OnDestroy firing here means the food was
        // genuinely picked up - not just looked at.
        ObjectiveManager.Instance.TryCompleteStep("GET_FOOD");
    }
}