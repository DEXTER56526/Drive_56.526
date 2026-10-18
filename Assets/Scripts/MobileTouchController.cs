using UnityEngine;

// Simple bridge: expose values that a mobile UI (joystick + buttons) writes to.
// Create a UI canvas for mobile with a left joystick (steering) and right vertical throttle/slide (throttle),
// plus UI Buttons for handbrake, camera, reset position and reset damage.
public class MobileTouchController : MonoBehaviour
{
    [HideInInspector] public float Throttle;   // -1..1
    [HideInInspector] public float Steering;   // -1..1
    [HideInInspector] public bool HandBrake;
    [HideInInspector] public bool CameraToggle;
    [HideInInspector] public bool ResetPosition;
    [HideInInspector] public bool ResetDamage;

    // If true, InputAdapter will use these values instead of keyboard
    public bool IsTouchConnected = true;

    public void SetThrottle(float t) { Throttle = Mathf.Clamp(t, -1f, 1f); }
    public void SetSteering(float s) { Steering = Mathf.Clamp(s, -1f, 1f); }
    public void SetHandBrake(bool hb) { HandBrake = hb; }
    public void PressCameraToggle() { CameraToggle = true; }
    public void PressResetPosition() { ResetPosition = true; }
    public void PressResetDamage() { ResetDamage = true; }

    void LateUpdate()
    {
        // One-frame buttons will be consumed by InputAdapter; clear them here so they are only true for one Update.
        CameraToggle = false;
        ResetPosition = false;
        ResetDamage = false;
    }
}
