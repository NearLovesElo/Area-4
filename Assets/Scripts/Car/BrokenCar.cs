using UnityEngine;

/// <summary>
/// Put this on the Car GameObject, alongside an InteractableTrigger that
/// points back to this script.
///
/// The car has three phases:
///   Inspect  - player hasn't seen the car yet. Shows "missing parts" dialogue,
///              advances objective to FIND_CAR_PARTS, reveals the part pickups.
///   Repair   - all 3 parts collected. Repairing advances to REACH_AREA_4.
///   Done     - car is repaired. CanInteract() = false, button disappears.
///
/// The car tracks its own state. Nothing outside this script needs to know
/// which phase it's in.
/// </summary>
public class BrokenCar : MonoBehaviour, IInteractable
{
    private enum CarPhase { Inspect, Repair, Done }

    [Header("Car Parts to Reveal")]
    [Tooltip("The 3 car part GameObjects to enable when the player inspects the car. " +
             "Keep them disabled in the scene until then.")]
    [SerializeField] private GameObject[] carPartObjects;

    [Header("Total Parts Required")]
    [Tooltip("Must match the number of CarPart pickups in the scene.")]
    [SerializeField] private int totalPartsRequired = 3;

    [Header("Inspect Dialogue")]
    [TextArea]
    [SerializeField]
    private string[] inspectLines =
    {
        "The car has missing parts.",
        "But I think I can make this car run if I just find a Car Battery, Spark Plugs, and a Container of Gas."
    };

    [Header("Repair Dialogue")]
    [TextArea]
    [SerializeField]
    private string[] repairLines =
    {
        "I have everything I need.",
        "Let me get this running..."
    };

    private CarPhase _phase = CarPhase.Inspect;
    private int _partsCollected = 0;

    // ------------------------------------------------------------------ //
    // IInteractable
    // ------------------------------------------------------------------ //

    public bool CanInteract() => _phase != CarPhase.Done;

    public void Interact()
    {
        switch (_phase)
        {
            case CarPhase.Inspect:
                HandleInspect();
                break;

            case CarPhase.Repair:
                HandleRepair();
                break;
        }
    }

    // ------------------------------------------------------------------ //
    // Called by CarPart.OnDestroy when a part pickup is collected
    // ------------------------------------------------------------------ //

    public void OnPartCollected()
    {
        _partsCollected++;

        if (_partsCollected >= totalPartsRequired && _phase == CarPhase.Inspect)
        {
            // All parts are in the player's inventory - car is ready to repair.
            // Advance the objective and flip phase so the next interaction repairs.
            ObjectiveManager.Instance.TryCompleteStep("FIND_CAR_PARTS");
            _phase = CarPhase.Repair;

            // Tell InteractionController to re-evaluate so the button shows
            // again immediately if the player is already standing next to the car.
            InteractionController.Instance.RefreshCurrent();
        }
    }

    // ------------------------------------------------------------------ //
    // Private
    // ------------------------------------------------------------------ //

    private void HandleInspect()
    {
        // First time interacting with the car. Show dialogue, reveal parts.
        bool isFirstTime = !ObjectiveManager.Instance.IsStepComplete("WALK_TO_CAR");

        ObjectiveManager.Instance.TryCompleteStep("WALK_TO_CAR");

        DialogueManager.Instance.PlayLines(inspectLines);

        if (isFirstTime)
        {
            foreach (var part in carPartObjects)
            {
                if (part != null) part.SetActive(true);
            }
        }

        // Phase stays Inspect until all parts are collected.
        // CanInteract() remains true so the button stays visible,
        // but re-interacting just replays the inspect dialogue (harmless).
    }

    private void HandleRepair()
    {
        _phase = CarPhase.Done;

        DialogueManager.Instance.PlayLines(repairLines, onFinished: OnRepairFinished);

        // Hide the interact button immediately since the car is now done.
        InteractionController.Instance.RefreshCurrent();
    }

    private void OnRepairFinished()
    {
        ObjectiveManager.Instance.TryCompleteStep("REPAIR_CAR");
    }
}