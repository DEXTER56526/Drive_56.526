using UnityEngine;

// VehicleReset handles resetting the vehicle position/rotation and clearing velocities.
// Attach to the Car root and set the spawnPosition (or it will record initial position at Start).
[RequireComponent(typeof(Rigidbody))]
public class VehicleReset : MonoBehaviour
{
    public Vector3 spawnPosition;
    public Quaternion spawnRotation;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (spawnPosition == Vector3.zero && spawnRotation == Quaternion.identity)
        {
            spawnPosition = transform.position;
            spawnRotation = transform.rotation;
        }
    }

    void Update()
    {
        var ia = InputAdapter.Instance;
        if (ia != null && ia.ResetPosition)
        {
            ResetToSpawn();
        }

        if (ia != null && ia.ResetDamage)
        {
            var dc = GetComponent<DamageController>();
            if (dc != null) dc.ResetDamage();
        }
    }

    public void ResetToSpawn()
    {
        transform.position = spawnPosition + Vector3.up * 1.0f; // nudge up to avoid getting stuck
        transform.rotation = spawnRotation;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
