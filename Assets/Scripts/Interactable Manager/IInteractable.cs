/// <summary>
/// Implement this on ANY object the player can interact with (Radio, Car, Door, Item...).
/// The InteractionController only ever talks to this interface, so adding a new
/// interactable later never requires changing the controller.
/// </summary>
public interface IInteractable
{
    /// <summary>Called when the player presses the Interact Button while this is the active target.</summary>
    void Interact();

    /// <summary>
    /// Return false to temporarily hide the Interact Button for this object
    /// (e.g. car already repaired, radio already used, door already unlocked).
    /// </summary>
    bool CanInteract();
}
