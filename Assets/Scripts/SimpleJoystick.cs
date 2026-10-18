using UnityEngine;
using UnityEngine.EventSystems;

// Represents a virtual on-screen steering joystick for touch devices.
public class SimpleJoystick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public MobileTouchController mobile;
    public RectTransform handle;   // the knob that moves
    public RectTransform centerArea;
    public float radius = 60f;

    private Vector2 origin;
    private bool dragging;

    void Awake()
    {
        if (mobile == null) mobile = FindObjectOfType<MobileTouchController>();
        if (centerArea != null)
            origin = centerArea.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (centerArea == null || handle == null) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(centerArea, eventData.position, eventData.pressEventCamera, out localPoint);
        float mag = Mathf.Clamp(localPoint.magnitude, 0f, radius);
        Vector2 normal = localPoint.normalized;
        handle.anchoredPosition = normal * mag;

        float value = Mathf.Clamp(localPoint.x / radius, -1f, 1f);
        if (mobile != null) mobile.SetSteering(value);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        if (handle != null) handle.anchoredPosition = Vector2.zero;
        if (mobile != null) mobile.SetSteering(0f);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (handle != null) handle.anchoredPosition = Vector2.zero;
        if (mobile != null) mobile.SetSteering(0f);
    }
}
