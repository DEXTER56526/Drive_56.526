using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject[] vehiclePrefabs; // assign prefabs in inspector
    public int initialSpawn = 5;
    public float spawnRadius = 50f;

    void Start()
    {
        for (int i = 0; i < initialSpawn; i++)
        {
            SpawnRandomVehicle();
        }
    }

    public GameObject SpawnRandomVehicle()
    {
        if (vehiclePrefabs == null || vehiclePrefabs.Length == 0) return null;

        int idx = Random.Range(0, vehiclePrefabs.Length);
        Vector3 pos = transform.position + Random.insideUnitSphere * spawnRadius;
        pos.y = transform.position.y + 1f; // make sure it spawns above ground

        Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        GameObject go = Instantiate(vehiclePrefabs[idx], pos, rot);
        return go;
    }
}
