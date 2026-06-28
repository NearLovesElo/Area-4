using System;
using UnityEngine;

/// <summary>
/// Put this on a persistent GameObject (e.g. "GameManagers", alongside InventoryManager).
/// Owns the player's core survival stats. Knows nothing about UI - StatSlider
/// scripts subscribe to OnStatsChanged and redraw themselves, same separation
/// as InventoryManager / InventoryUI.
/// </summary>
public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Current Values (0-100)")]
    [SerializeField] private float health = 50f;
    [SerializeField] private float hunger = 50f;
    [SerializeField] private float thirst = 50f;

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

    public void RestoreHealth(float amount) => ChangeStat(ref health, amount);
    public void RestoreHunger(float amount) => ChangeStat(ref hunger, amount);
    public void RestoreThirst(float amount) => ChangeStat(ref thirst, amount);

    private void ChangeStat(ref float stat, float amount)
    {
        stat = Mathf.Clamp(stat + amount, 0f, MaxValue);
        OnStatsChanged?.Invoke();
    }
}