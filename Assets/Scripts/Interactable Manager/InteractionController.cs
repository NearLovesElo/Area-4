using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this on the Player. Keeps a list of every IInteractable currently inside
/// the player's trigger range and decides which one is "active" (closest).
/// Shows/hides the Interact Button automatically based on that.
///
/// Because each interactable reports itself via InteractableTrigger, this script
/// never needs to know what a Radio or a Car is - it just works with the interface.
/// </summary>
public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private Button interactButton;

    // Everything currently inside range, regardless of CanInteract() state.
    private readonly List<IInteractable> _inRange = new List<IInteractable>();

    private IInteractable _current;

    private void Awake()
    {
        // Simple singleton - fine since there is only ever one Player.
        Instance = this;
    }

    private void Start()
    {
        if (interactButton != null)
        {
            interactButton.onClick.AddListener(OnInteractButtonPressed);
        }

        RefreshButton();
    }

    public void RegisterInteractable(IInteractable interactable)
    {
        if (!_inRange.Contains(interactable))
        {
            _inRange.Add(interactable);
        }

        RefreshCurrent();
    }

    public void UnregisterInteractable(IInteractable interactable)
    {
        _inRange.Remove(interactable);
        RefreshCurrent();
    }

    /// <summary>
    /// Re-evaluates which interactable (if any) should currently be usable.
    /// Call this after the in-range list changes, OR whenever something's
    /// CanInteract() state might have changed (e.g. right after using it).
    /// </summary>
    public void RefreshCurrent()
    {
        _current = null;

        // Picks the first valid one. Good enough for now - if you ever have
        // multiple overlapping interactables, swap this for a "closest to player" check.
        for (int i = 0; i < _inRange.Count; i++)
        {
            if (_inRange[i].CanInteract())
            {
                _current = _inRange[i];
                break;
            }
        }

        RefreshButton();
    }

    private void RefreshButton()
    {
        if (interactButton == null) return;
        interactButton.gameObject.SetActive(_current != null);
    }

    private void OnInteractButtonPressed()
    {
        _current?.Interact();

        // The thing we just interacted with might now become "used up"
        // (e.g. radio already played, car already repaired), so re-check.
        RefreshCurrent();
    }
}
