using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject bigStonePrefab;
    [SerializeField]
    private GameObject smallStonePrefab;
    [SerializeField]
    private GameObject bigTreePrefab;
    [SerializeField]
    private GameObject smallTreePrefab;
    
    [Header("Spawn Settings")]
    public float mapWidth = 100f;     // Width of the map
    public float mapHeight = 100f;    // Height of the map
    public float minSpacing = 5f;     // Minimum space between obstacles
    public float maxSpacing = 20f;    // Maximum space between obstacles
    public int obstaclesPerType = 5;  // Number of each type to spawn
    
    private List<GameObject> obstaclePool = new List<GameObject>();

    void Start()
    {
        // Spawn all obstacles at start
        SpawnAllObstacles();
    }

    void SpawnAllObstacles()
    {
        // Calculate start position (bottom-left corner)
        Vector3 startPos = new Vector3(-mapWidth/2, -mapHeight/2, 0);
        
        // Spawn each type of obstacle
        SpawnObstacleType(bigStonePrefab, obstaclesPerType, startPos);
        SpawnObstacleType(smallStonePrefab, obstaclesPerType, startPos);
        SpawnObstacleType(bigTreePrefab, obstaclesPerType, startPos);
        SpawnObstacleType(smallTreePrefab, obstaclesPerType, startPos);
    }

    void SpawnObstacleType(GameObject prefab, int count, Vector3 startPos)
    {
        if (prefab == null) return;

        for (int i = 0; i < count; i++)
        {
            // Get random position within map bounds
            float x = Random.Range(0, mapWidth);
            float y = Random.Range(0, mapHeight);
            
            // Calculate world position
            Vector3 position = startPos + new Vector3(x, y, 0);
            
            // Check if position is too close to other obstacles
            if (IsPositionValid(position))
            {
                // Create and position the obstacle
                GameObject obstacle = Instantiate(prefab, position, Quaternion.identity);
                
                // Get the Obstacle component and ensure size is set
                Obstacle obstacleComponent = obstacle.GetComponent<Obstacle>();
                if (obstacleComponent != null)
                {
                    // The size will be applied in the Obstacle's Start method
                    obstaclePool.Add(obstacle);
                }
                else
                {
                    Debug.LogError($"Obstacle component missing from {prefab.name} prefab!");
                    Destroy(obstacle);
                }
            }
            else
            {
                // Try again with a different position
                i--;
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        // Check distance to all existing obstacles
        foreach (GameObject obstacle in obstaclePool)
        {
            float distance = Vector3.Distance(position, obstacle.transform.position);
            if (distance < minSpacing)
            {
                return false;
            }
        }
        return true;
    }

    // Optional: Add this to visualize spawn area in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(mapWidth, mapHeight, 0));
    }
} 