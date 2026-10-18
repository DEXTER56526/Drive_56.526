using UnityEngine;

// WorldGenerator creates a flat terrain, places the road generator, and scatters simple props.
[RequireComponent(typeof(RoadGenerator))]
public class WorldGenerator : MonoBehaviour
{
    public int terrainSize = 1000;
    public GameObject groundPrefab; // optional, otherwise a plane will be created
    public GameObject[] propPrefabs; // trees, rocks (use primitives if needed)
    public int propCount = 100;

    RoadGenerator roadGen;

    void Awake()
    {
        roadGen = GetComponent<RoadGenerator>();
    }

    void Start()
    {
        CreateGround();
        roadGen.GenerateGrid();
        ScatterProps();
    }

    void CreateGround()
    {
        if (groundPrefab != null)
        {
            Instantiate(groundPrefab, transform.position, Quaternion.identity, transform);
        }
        else
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.transform.position = transform.position;
            ground.transform.localScale = new Vector3(terrainSize / 10f, 1f, terrainSize / 10f);
            ground.name = "Ground";
            ground.transform.parent = transform;
            var col = ground.GetComponent<Collider>();
            if (col != null) col.sharedMaterial = null;
        }
    }

    void ScatterProps()
    {
        if (propPrefabs == null || propPrefabs.Length == 0) return;

        for (int i = 0; i < propCount; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-terrainSize/2f, terrainSize/2f), 0f, Random.Range(-terrainSize/2f, terrainSize/2f));
            Ray ray = new Ray(pos + Vector3.up * 50f, Vector3.down);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200f))
            {
                pos = hit.point;
            }
            else
            {
                pos.y = 0f;
            }

            GameObject prefab = propPrefabs[Random.Range(0, propPrefabs.Length)];
            GameObject go = Instantiate(prefab, pos, Quaternion.Euler(0f, Random.Range(0f,360f), 0f), transform);
            go.transform.localScale *= Random.Range(0.8f, 1.4f);
        }
    }
}
