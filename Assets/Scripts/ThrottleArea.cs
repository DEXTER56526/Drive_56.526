using UnityEngine;
using UnityEngine.EventSystems;

// Vertical throttle/accelerator control for touch devices.
public class ThrottleArea : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler
{
    public MobileTouchController mobile;
    public RectTransform area;
    public float deadZone = 0.05f;

    void Awake()
    {
        if (mobile == null) mobile = FindObjectOfType<MobileTouchController>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateThrottle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateThrottle(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (mobile != null)
        {
            mobile.SetThrottle(0f);
        }
    }

    void UpdateThrottle(Vector2 pointer)
    {
        if (area == null || mobile == null) return;

        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(area, pointer, null, out local);
        float normalized = Mathf.Clamp(local.y / area.rect.height, -1f, 1f);

        // Invert: dragging up means forward
        float throttle = Mathf.Clamp(-normalized, -1f, 1f);
        if (Mathf.Abs(throttle) < deadZone) throttle = 0f;

        mobile.SetThrottle(throttle);
    }
}
