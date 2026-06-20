using UnityEngine;

/// <summary>
/// Put this on the SAME GameObject as your interactable's own Trigger Collider2D
/// (e.g. Radio, Car). It detects the Player entering/exiting and reports itself
/// to the InteractionController. The interactable object does NOT need to know
/// about the player or the UI at all - this script bridges that gap.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class InteractableTrigger : MonoBehaviour
{
    [Tooltip("Drag the component that implements IInteractable here (can be this same GameObject).")]
    [SerializeField] private MonoBehaviour interactableSource;

    private IInteractable _interactable;

    private void Awake()
    {
        _interactable = interactableSource as IInteractable;

        if (_interactable == null)
        {
            Debug.LogError($"{name}: 'interactableSource' must implement IInteractable.", this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_interactable == null) return;

        if (other.CompareTag("Player"))
        {
            InteractionController.Instance.RegisterInteractable(_interactable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_interactable == null) return;

        if (other.CompareTag("Player"))
        {
            InteractionController.Instance.UnregisterInteractable(_interactable);
        }
    }
}
