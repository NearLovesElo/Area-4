using UnityEngine;

/// <summary>
/// Added automatically by ItemSpawner to every spawned item. When the item
/// is destroyed (picked up via ItemPickup), it notifies the spawner entry
/// that one slot has freed up. This script does not need to be on prefabs -
/// ItemSpawner adds it at runtime.
/// </summary>
public class SpawnedItemTracker : MonoBehaviour
{
    private ItemSpawnEntry _entry;

    public void Initialize(ItemSpawnEntry entry)
    {
        _entry = entry;
    }

    private void OnDestroy()
    {
        if (_entry != null)
        {
            _entry.currentCount--;
        }
    }
}