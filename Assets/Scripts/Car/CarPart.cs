using UnityEngine;

/// <summary>
/// Put this alongside ItemPickup on each of the 3 tutorial car part pickups
/// (Car Battery, Spark Plugs, Gas Container). When picked up, notifies the
/// Car script so it knows one more part has been collected.
///
/// Same OnDestroy pattern as TutorialFood/TutorialGetWater/TutorialGetMedicine.
/// </summary>
[RequireComponent(typeof(ItemPickup))]
public class CarPart : MonoBehaviour
{
    [Tooltip("Drag the Car GameObject here so this part can notify it on pickup.")]
    [SerializeField] private BrokenCar car;

    private void OnDestroy()
    {
        if (car != null)
        {
            car.OnPartCollected();
        }
    }
}