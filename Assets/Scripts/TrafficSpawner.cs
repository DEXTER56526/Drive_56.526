using UnityEngine;
using System.Collections.Generic;

public class TrafficSpawner : MonoBehaviour
{
    public GameObject trafficPrefab;
    public Transform[] spawnPoints;
    public int poolSize = 12;
    public float respawnDelay = 2f;

    private List<GameObject> spawned = new List<GameObject>();
    private float nextSpawnTime;

    void Start()
    {
        if (trafficPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;

        for (int i = 0; i < poolSize; i++)
        {
            SpawnTrafficVehicle(i % spawnPoints.Length);
        }
    }

    void Update()
    {
        if (Time.time < nextSpawnTime) return;
        nextSpawnTime = Time.time + respawnDelay;

        // Recycle or respawn if there are less than poolSize active cars.
        if (spawned.Count < poolSize)
        {
            int idx = Random.Range(0, spawnPoints.Length);
            SpawnTrafficVehicle(idx);
        }
    }

    void SpawnTrafficVehicle(int spawnIndex)
    {
        if (trafficPrefab == null || spawnPoints == null || spawnPoints.Length == 0) return;

        Transform spawn = spawnPoints[spawnIndex];
        GameObject go = Instantiate(trafficPrefab, spawn.position, spawn.rotation);
        go.name = "TrafficVehicle_" + spawned.Count;

        var ai = go.GetComponent<TrafficVehicleAI>();
        if (ai != null)
        {
            // For a generated road network, waypoints should be assigned by the spawner.
            // If no waypoints are assigned, the car will just idle.
        }

        spawned.Add(go);
    }
}
