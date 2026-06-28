using UnityEngine;

/// <summary>
/// Put this on the SAME GameObject as ItemPickup, on the ONE specific medicine
/// item placed in the scene for the tutorial.
/// Same pattern as TutorialFood / TutorialGetWater.
/// </summary>
[RequireComponent(typeof(ItemPickup))]
public class TutorialGetMedicine : MonoBehaviour
{
    private void OnDestroy()
    {
        ObjectiveManager.Instance.TryCompleteStep("GET_MEDICINE");
    }
}