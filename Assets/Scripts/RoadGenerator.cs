using System.Collections.Generic;
using UnityEngine;

// Simple grid-based road generator that instantiates a roadPrefab along a grid.
// RoadPrefab should be a straight road segment (e.g., long cube scaled on X/Z).
// This generator is intentionally simple so you can replace it later with splines or tile-based meshes.
public class RoadGenerator : MonoBehaviour
{
    [Header("Grid")]
    public int width = 5;
    public int height = 5;
    public float spacing = 20f; // distance between intersections

    [Header("Prefabs")]
    public GameObject roadPrefab; // straight road segment aligned on the X axis
    public GameObject intersectionPrefab; // optional

    [Header("Generation")]
    public bool generateOnStart = true;

    List<GameObject> generated = new List<GameObject>();

    void Start()
    {
        if (generateOnStart) GenerateGrid();
    }

    [ContextMenu("GenerateGrid")]
    public void GenerateGrid()
    {
        Clear();

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                Vector3 center = transform.position + new Vector3((x - width/2) * spacing, 0f, (z - height/2) * spacing);

                // create two road segments crossing at the center: one along X and one along Z (if prefabs exist)
                if (roadPrefab != null)
                {
                    // X road
                    GameObject rx = Instantiate(roadPrefab, center, Quaternion.identity, transform);
                    rx.transform.localScale = new Vector3(spacing * 0.9f, 1f, spacing * 0.25f);
                    generated.Add(rx);

                    // Z road: rotate 90 degrees
                    GameObject rz = Instantiate(roadPrefab, center, Quaternion.Euler(0f, 90f, 0f), transform);
                    rz.transform.localScale = new Vector3(spacing * 0.9f, 1f, spacing * 0.25f);
                    generated.Add(rz);
                }

                if (intersectionPrefab != null)
                {
                    GameObject inter = Instantiate(intersectionPrefab, center + Vector3.up * 0.01f, Quaternion.identity, transform);
                    generated.Add(inter);
                }
            }
        }
    }

    public void Clear()
    {
        foreach (var g in generated)
        {
            if (g != null) DestroyImmediate(g);
        }
        generated.Clear();

        // Also clear children generated at runtime (safe guard)
        // Note: In play mode DestroyImmediate can be problematic; use Destroy instead if needed.
    }
}
