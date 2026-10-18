using UnityEngine;

// InputAdapter: unified input access for keyboard, gamepad and touch.
// For consoles/gamepads this uses Unity's legacy Input axes/buttons; you can switch to the new Input System later.
// For mobile, MobileTouchController will populate the touch values.
public class InputAdapter : MonoBehaviour
{
    public static InputAdapter Instance;

    [HideInInspector] public float Throttle;   // -1..1 where positive is forward
    [HideInInspector] public float Steering;   // -1..1 left/right
    [HideInInspector] public bool HandBrake;
    [HideInInspector] public bool CameraToggle;
    [HideInInspector] public bool ResetPosition;
    [HideInInspector] public bool ResetDamage;

    public MobileTouchController mobileTouchController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        // Reset one-shot buttons
        CameraToggle = false;
        ResetPosition = false;
        ResetDamage = false;

        // Touch has priority if provided and enabled
        if (mobileTouchController != null && mobileTouchController.IsTouchConnected)
        {
            Throttle = mobileTouchController.Throttle;
            Steering = mobileTouchController.Steering;
            HandBrake = mobileTouchController.HandBrake;
            if (mobileTouchController.CameraToggle) CameraToggle = true;
            if (mobileTouchController.ResetPosition) ResetPosition = true;
            if (mobileTouchController.ResetDamage) ResetDamage = true;
            return;
        }

        // Desktop / Laptop / Editor (keyboard + mouse)
        float vertical = Input.GetAxis("Vertical");   // W/S or Up/Down
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right

        float accel = vertical;
        float steer = horizontal;

        // Gamepad triggers mapping (if configured) can be added here
        Throttle = accel;
        Steering = steer;

        HandBrake = Input.GetKey(KeyCode.Space) || Input.GetButton("Jump");

        if (Input.GetKeyDown(KeyCode.C) || Input.GetButtonDown("Fire3"))
            CameraToggle = true;

        if (Input.GetKeyDown(KeyCode.R) || Input.GetButtonDown("Submit"))
            ResetPosition = true;

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Cancel"))
            ResetDamage = true;
    }
}
