using UnityEngine;

/// <summary>
/// Put this on an empty GameObject with a BoxCollider2D. Place and resize the
/// box in the Scene view to cover a walkable area. ItemSpawner will pick
/// random points inside these boxes when spawning items.
///
/// Make the BoxCollider2D a trigger (Is Trigger = checked) so it doesn't
/// interfere with physics. Tag it however you like - ItemSpawner finds all
/// SpawnZone components directly, no tag needed.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
public class SpawnZone : MonoBehaviour
{
    private BoxCollider2D _box;

    private void Awake()
    {
        _box = GetComponent<BoxCollider2D>();
    }

    /// <summary>Returns a random world-space point inside this zone's box.</summary>
    public Vector2 GetRandomPoint()
    {
        Bounds bounds = _box.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }
}