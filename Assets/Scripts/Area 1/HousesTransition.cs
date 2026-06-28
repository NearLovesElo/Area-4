using Unity.Cinemachine;
using UnityEngine;
using System.Collections;

public class HousesTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    [SerializeField] Transform spawnPoint;
    CinemachineConfiner2D confiner;
    CinemachineCamera cinemachineCamera;

    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Safe to call every time the player walks through this exit -
            // ObjectiveManager already no-ops repeats. First time through,
            // this advances "LEAVE_HOUSE" -> "WALK_TO_CAR".
            ObjectiveManager.Instance.TryCompleteStep("LEAVE_HOUSE");

            StartCoroutine(Transition(collision.transform));
        }
    }

    private IEnumerator Transition(Transform player)
    {
        // 1. Disable camera
        cinemachineCamera.enabled = false;
        // 2. Teleport player and swap boundary
        player.position = spawnPoint.position;
        confiner.BoundingShape2D = mapBoundary;
        confiner.InvalidateBoundingShapeCache();
        // 3. Wait one frame so Cinemachine registers the new state
        yield return null;
        // 4. Re-enable — camera snaps to new position
        cinemachineCamera.enabled = true;
    }
}