using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    [Header("Drive")]
    public float maxMotorForce = 1500f; // torque
    public float maxBrakeForce = 3000f;
    public float maxSteerAngle = 30f;
    public float topSpeed = 60f; // meters per second

    [Header("Stability")]
    public Vector3 centerOfMassOffset = new Vector3(0, -0.5f, 0);
    public float downforce = 100f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass += centerOfMassOffset;
    }

    void FixedUpdate()
    {
        float motor = Input.GetAxis("Vertical"); // W/S or Up/Down
        float steer = Input.GetAxis("Horizontal"); // A/D or Left/Right
        bool handBrake = Input.GetKey(KeyCode.Space);

        // Simple speed limiting
        float currentSpeed = rb.velocity.magnitude;
        float motorForce = 0f;
        if (motor > 0 && currentSpeed < topSpeed) motorForce = motor * maxMotorForce;
        else if (motor < 0) motorForce = motor * (maxMotorForce * 0.5f); // reverse

        // Apply force at the rear of the vehicle to approximate wheel drive
        Vector3 force = transform.forward * motorForce * Time.fixedDeltaTime;
        rb.AddForce(force);

        // Steering - approximate by rotating the transform based on speed
        float steerAmount = steer * maxSteerAngle * Time.fixedDeltaTime;
        float steerModifier = Mathf.Clamp01(rb.velocity.magnitude / (topSpeed * 0.5f));
        transform.Rotate(0f, steerAmount * steerModifier, 0f, Space.Self);

        // Braking / handbrake
        if (handBrake)
        {
            rb.drag = 5f;
        }
        else
        {
            rb.drag = 0.1f;
        }

        // Downforce to keep the car grounded at speed
        rb.AddForce(-transform.up * rb.velocity.magnitude * downforce * Time.fixedDeltaTime);
    }
}
