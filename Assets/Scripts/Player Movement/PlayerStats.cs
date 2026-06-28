using System;
using UnityEngine;

/// <summary>
/// Put this on a persistent GameObject (e.g. "GameManagers", alongside InventoryManager).
/// Owns the player's core survival stats. Knows nothing about UI - StatSlider
/// scripts subscribe to OnStatsChanged and redraw themselves, same separation
/// as InventoryManager / InventoryUI.
///
/// Also handles passive decay over time (health/hunger/thirst dropping on
/// their own timers). Each stat's decay rate and interval are set in the
/// Inspector, so tuning difficulty never requires touching code.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Current Values (0-100)")]
    [SerializeField] private float health = 50f;
    [SerializeField] private float hunger = 50f;
    [SerializeField] private float thirst = 50f;

    [Header("Passive Decay")]
    [Tooltip("How much health drops per tick.")]
    [SerializeField] private float healthDecayAmount = 1f;
    [Tooltip("Seconds between each health decay tick.")]
    [SerializeField] private float healthDecayInterval = 10f;

    [Tooltip("How much hunger drops per tick.")]
    [SerializeField] private float hungerDecayAmount = 1f;
    [Tooltip("Seconds between each hunger decay tick.")]
    [SerializeField] private float hungerDecayInterval = 30f;

    [Tooltip("How much thirst drops per tick.")]
    [SerializeField] private float thirstDecayAmount = 1f;
    [Tooltip("Seconds between each thirst decay tick.")]
    [SerializeField] private float thirstDecayInterval = 20f;

    public float Health => health;
    public float Hunger => hunger;
    public float Thirst => thirst;

    private const float MaxValue = 100f;

    /// <summary>Fired whenever any stat changes - sliders redraw themselves on this.</summary>
    public event Action OnStatsChanged;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // InvokeRepeating starts after 'time' seconds, then repeats every
        // 'repeatRate' seconds - using the interval for both means the
        // first tick doesn't happen instantly at scene start.
        InvokeRepeating(nameof(DecayHealth), healthDecayInterval, healthDecayInterval);
        InvokeRepeating(nameof(DecayHunger), hungerDecayInterval, hungerDecayInterval);
        InvokeRepeating(nameof(DecayThirst), thirstDecayInterval, thirstDecayInterval);
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }

    public void RestoreHealth(float amount) => ChangeStat(ref health, amount);
    public void RestoreHunger(float amount) => ChangeStat(ref hunger, amount);
    public void RestoreThirst(float amount) => ChangeStat(ref thirst, amount);

    private void DecayHealth() => ChangeStat(ref health, -healthDecayAmount);
    private void DecayHunger() => ChangeStat(ref hunger, -hungerDecayAmount);
    private void DecayThirst() => ChangeStat(ref thirst, -thirstDecayAmount);

    private void ChangeStat(ref float stat, float amount)
    {
        stat = Mathf.Clamp(stat + amount, 0f, MaxValue);
        OnStatsChanged?.Invoke();
    }
}