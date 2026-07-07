using UnityEngine;

/// <summary>
/// One row in ItemSpawner's list. Defines the prefab, how many spawn at
/// start, the periodic spawn interval, and the maximum allowed in the world
/// at any time. All values are per item type - Food, Water, and Medicine
/// each have their own independent settings and counters.
/// </summary>
[System.Serializable]
public class ItemSpawnEntry
{
    [Tooltip("Prefab to spawn. Must have ItemPickup + InteractableTrigger on it.")]
    public GameObject itemPrefab;

    [Tooltip("How many of this item to spawn immediately at scene start.")]
    [Min(0)]
    public int initialCount = 10;

    [Tooltip("Seconds between each automatic respawn tick.")]
    [Min(0.1f)]
    public float spawnInterval = 2f;

    [Tooltip("Maximum number of this item allowed in the world at once. " +
             "Respawn ticks are skipped while at or above this limit.")]
    [Min(1)]
    public int maxCount = 20;

    // Tracked at runtime - not shown in Inspector.
    [System.NonSerialized] public int currentCount;
}