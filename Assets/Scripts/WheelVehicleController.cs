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
    DamageController damageController;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
        damageController = GetComponent<DamageController>();
    }

    void FixedUpdate()
    {
        var ia = InputAdapter.Instance;
        float motorInput = ia != null ? ia.Throttle : Input.GetAxis("Vertical");
        float steerInput = ia != null ? ia.Steering : Input.GetAxis("Horizontal");
        bool handBrake = ia != null ? ia.HandBrake : Input.GetKey(KeyCode.Space);

        float speedKPH = rb.velocity.magnitude * 3.6f;

        // Limit motor torque past top speed
        float motor = 0f;
        if (motorInput > 0 && speedKPH < topSpeedKPH) motor = motorInput * maxMotorTorque;
        else if (motorInput < 0) motor = motorInput * (maxMotorTorque * 0.5f); // reverse weaker

        // Apply motor torque to rear wheels (rear-wheel drive)
        if (rearLeftCollider != null) rearLeftCollider.motorTorque = motor * Time.fixedDeltaTime;
        if (rearRightCollider != null) rearRightCollider.motorTorque = motor * Time.fixedDeltaTime;

        // Steering
        float steer = steerInput * maxSteerAngle;
        if (frontLeftCollider != null) frontLeftCollider.steerAngle = steer;
        if (frontRightCollider != null) frontRightCollider.steerAngle = steer;

        // Brakes
        if (handBrake)
        {
            if (rearLeftCollider != null) rearLeftCollider.brakeTorque = maxBrakeTorque;
            if (rearRightCollider != null) rearRightCollider.brakeTorque = maxBrakeTorque;
        }
        else
        {
            if (rearLeftCollider != null) rearLeftCollider.brakeTorque = 0f;
            if (rearRightCollider != null) rearRightCollider.brakeTorque = 0f;
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

    void OnCollisionEnter(Collision collision)
    {
        // Simple damage on impact proportional to relative velocity
        if (damageController != null)
        {
            float impact = collision.relativeVelocity.magnitude;
            float dmg = impact * 2f; // tuning factor
            damageController.ApplyDamage(dmg);
        }
    }
}
