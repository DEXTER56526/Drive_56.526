using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WheelVehicleController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftCollider;
    public WheelCollider frontRightCollider;
    public WheelCollider rearLeftCollider;
    public WheelCollider rearRightCollider;

    [Header("Wheel visuals")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    [Header("Drive Parameters")]
    public float maxMotorTorque = 1500f; // Nm applied to drive wheels
    public float maxBrakeTorque = 3000f;
    public float maxSteerAngle = 30f;
    public float topSpeedKPH = 180f;

    [Header("Stability")]
    public float downforce = 100f;

    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void FixedUpdate()
    {
        float motorInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");
        bool handBrake = Input.GetKey(KeyCode.Space);

        float speedKPH = rb.velocity.magnitude * 3.6f;

        // Limit motor torque past top speed
        float motor = 0f;
        if (motorInput > 0 && speedKPH < topSpeedKPH) motor = motorInput * maxMotorTorque;
        else if (motorInput < 0) motor = motorInput * (maxMotorTorque * 0.5f); // reverse weaker

        // Apply motor torque to rear wheels (rear-wheel drive)
        rearLeftCollider.motorTorque = motor * Time.fixedDeltaTime;
        rearRightCollider.motorTorque = motor * Time.fixedDeltaTime;

        // Steering
        float steer = steerInput * maxSteerAngle;
        frontLeftCollider.steerAngle = steer;
        frontRightCollider.steerAngle = steer;

        // Brakes
        float brake = 0f;
        if (handBrake)
        {
            brake = maxBrakeTorque;
            rearLeftCollider.brakeTorque = brake;
            rearRightCollider.brakeTorque = brake;
        }
        else
        {
            rearLeftCollider.brakeTorque = 0f;
            rearRightCollider.brakeTorque = 0f;
        }

        // Simple downforce
        rb.AddForce(-transform.up * rb.velocity.magnitude * downforce * Time.fixedDeltaTime);

        // Update visual wheels
        UpdateWheelPose(frontLeftCollider, frontLeftTransform);
        UpdateWheelPose(frontRightCollider, frontRightTransform);
        UpdateWheelPose(rearLeftCollider, rearLeftTransform);
        UpdateWheelPose(rearRightCollider, rearRightTransform);
    }

    void UpdateWheelPose(WheelCollider collider, Transform trans)
    {
        if (collider == null || trans == null) return;
        Vector3 pos;
        Quaternion quat;
        collider.GetWorldPose(out pos, out quat);
        trans.position = pos;
        trans.rotation = quat;
    }

    public float GetSpeedKPH()
    {
        if (rb == null) return 0f;
        return rb.velocity.magnitude * 3.6f;
    }
}
