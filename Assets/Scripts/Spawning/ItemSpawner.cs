using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put this on any persistent GameObject (e.g. "GameManagers").
/// At scene start, spawns initialCount of each item type. Then runs an
/// independent repeating timer per item type that spawns one more every
/// spawnInterval seconds, up to maxCount.
///
/// When a picked-up item is destroyed, its count drops automatically so the
/// spawner knows a slot has opened up.
///
/// To add a new spawnable item type: add one entry to Items in the Inspector.
/// To add a new safe spawn area: place a SpawnZone in the scene.
/// No code changes needed for either.
/// </summary>
public class ItemSpawner : MonoBehaviour
{
    [Header("Items to Spawn")]
    [SerializeField] private List<ItemSpawnEntry> items = new List<ItemSpawnEntry>();

    [Header("Spawn Zones")]
    [Tooltip("Leave empty to auto-find all SpawnZone components in the scene.")]
    [SerializeField] private List<SpawnZone> spawnZones = new List<SpawnZone>();

    [Header("Placement")]
    [Tooltip("Parent transform to keep spawned items organized in the Hierarchy.")]
    [SerializeField] private Transform itemContainer;

    private void Start()
    {
        if (spawnZones.Count == 0)
        {
            spawnZones.AddRange(FindObjectsByType<SpawnZone>(FindObjectsSortMode.None));
        }

        if (spawnZones.Count == 0)
        {
            Debug.LogWarning("ItemSpawner: No SpawnZones found. Add SpawnZone components.");
            return;
        }

        foreach (var entry in items)
        {
            if (entry.itemPrefab == null) continue;

            // Spawn initial batch.
            for (int i = 0; i < entry.initialCount; i++)
            {
                SpawnOne(entry);
            }

            // Start an independent repeating coroutine per item type.
            StartCoroutine(RespawnLoop(entry));
        }
    }

    private IEnumerator RespawnLoop(ItemSpawnEntry entry)
    {
        while (true)
        {
            yield return new WaitForSeconds(entry.spawnInterval);

            if (entry.currentCount < entry.maxCount)
            {
                SpawnOne(entry);
            }
        }
    }

    private void SpawnOne(ItemSpawnEntry entry)
    {
        SpawnZone zone = spawnZones[Random.Range(0, spawnZones.Count)];
        Vector2 position = zone.GetRandomPoint();

        GameObject spawned = Instantiate(entry.itemPrefab, position,
                                         Quaternion.identity, itemContainer);

        entry.currentCount++;

        // When the item is picked up (destroyed), decrement the count
        // so the respawn loop knows a slot opened up.
        SpawnedItemTracker tracker = spawned.AddComponent<SpawnedItemTracker>();
        tracker.Initialize(entry);
    }
}