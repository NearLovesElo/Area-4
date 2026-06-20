using UnityEngine;

/// <summary>
/// Put this on the Radio GameObject (alongside an InteractableTrigger that
/// points back to this script). This is a template for ALL future
/// interactables - copy this pattern for Car, Door, ItemPickup, etc.
/// </summary>
public class Radio : MonoBehaviour, IInteractable
{
    [TextArea]
    [SerializeField]
    private string[] broadcastLines =
    {
        "Emergency broadcast. Three days ago, the Area 1 Nuclear Power Plant exploded, releasing radioactive contamination across Areas 1, 2, and 3.",
        "Many exposed survivors have become infected and hostile.",
        "If you're hearing this, you are still alive. Stay indoors whenever possible. Radiation exposure outside will gradually damage your health.",
        "Scavenge for food to satisfy hunger, water to prevent dehydration, and medicine to slow the effects of radiation sickness.",
        "If you find a working vehicle, use it. Faster travel means less exposure.",
        "Area 4 remains safe. Emergency containment barriers prevented the contamination from reaching the region, and a treatment is available there.",
        "We cannot send help. Your only chance of survival is to reach Area 4.",
        "Good luck, survivor."
    };

    // Radio can always be re-listened to (CanInteract = true forever).
    // The thing that does NOT repeat is the OBJECTIVE STEP, handled below.
    public bool CanInteract() => true;

    public void Interact()
    {
        DialogueManager.Instance.PlayLines(broadcastLines, onFinished: OnBroadcastFinished);
    }

    private void OnBroadcastFinished()
    {
        // Safe to call every single time the player listens to the radio.
        // First time: marks it complete and advances the objective list.
        // Every time after: ObjectiveManager sees it's already complete and does nothing.
        ObjectiveManager.Instance.TryCompleteStep("LISTEN_TO_RADIO");
    }
}
