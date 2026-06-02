using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform handle;

    public Vector2 InputDirection { get; private set; }
    public bool IsPressed { get; private set; }
    public bool WasReleased { get; private set; }  // true for one frame on release

    private RectTransform _background;
    private float _radius;
    private Canvas _canvas;

    void Start()
    {
        _background = GetComponent<RectTransform>();
        _radius = _background.rect.width / 2f;
        _canvas = GetComponentInParent<Canvas>();
    }

    void LateUpdate()
    {
        WasReleased = false;  // reset after one frame
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPoint;
        Camera cam = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : _canvas.worldCamera;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _background, eventData.position, cam, out localPoint);

        localPoint = Vector2.ClampMagnitude(localPoint, _radius);
        handle.localPosition = localPoint;
        InputDirection = localPoint / _radius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
        WasReleased = true;  // signal PlayerMovement to stop
        InputDirection = Vector2.zero;
        handle.localPosition = Vector2.zero;
    }
}