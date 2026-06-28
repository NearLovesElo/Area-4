using UnityEngine;

/// <summary>
/// Put this on the SAME GameObject as ItemPickup, on the ONE specific water
/// item placed in the scene for the tutorial (not on every water item -
/// other water pickups should just use ItemPickup on its own).
///
/// Same pattern as TutorialFood - listens for the moment this specific
/// item is collected and advances the objective.
/// </summary>
[RequireComponent(typeof(ItemPickup))]
public class TutorialGetWater : MonoBehaviour
{
    private void OnDestroy()
    {
        // ItemPickup destroys this GameObject right after a successful
        // AddItem call, so OnDestroy firing here means the water was
        // genuinely picked up - not just looked at.
        ObjectiveManager.Instance.TryCompleteStep("GET_WATER");
    }
}