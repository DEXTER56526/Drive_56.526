using UnityEngine;

// TrafficVehicleAI: simple waypoint-based AI for cars. Use on a vehicle with Rigidbody + WheelCollider-based setup.
[RequireComponent(typeof(Rigidbody))]
public class TrafficVehicleAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float minStopDistance = 2f;
    public float maxSpeed = 25f;
    public float steeringStrength = 1.2f;
    public float brakingStrength = 0.7f;
    public float waypointTolerance = 4f;
    public float visualTurnFactor = 0.25f;

    public int currentWaypointIndex;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null) rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        if (target == null)
        {
            currentWaypointIndex = 0;
            target = waypoints[currentWaypointIndex];
        }

        Vector3 dirToTarget = target.position - transform.position;
        float distance = dirToTarget.magnitude;

        if (distance <= waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            target = waypoints[currentWaypointIndex];
            dirToTarget = target.position - transform.position;
        }

        Vector3 localTarget = transform.InverseTransformDirection(dirToTarget.normalized);
        float steer = Mathf.Clamp(localTarget.x * steeringStrength, -1f, 1f);

        float speed = rb.velocity.magnitude;
        float brake = 0f;
        if (distance < minStopDistance || speed > maxSpeed)
        {
            brake = brakingStrength;
        }

        // Simple behavior approximation: accelerate or brake based on target direction
        float throttle = 1f;
        if (brake > 0f) throttle = 0f;

        // If you integrate with WheelVehicleController, call its methods or set normalized inputs here.
        // For this prototype we only use simple rigidbody steering.
        Vector3 desiredVelocity = transform.forward * maxSpeed * throttle;
        rb.velocity = Vector3.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 0.8f);
        Quaternion targetRot = Quaternion.LookRotation(dirToTarget.normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 1.2f * (1f + Mathf.Abs(steer) * visualTurnFactor));

        // Apply tiny drift to make traffic feel less robotic
        rb.AddForce(-transform.right * steer * 12f, ForceMode.Acceleration);
    }
}
