using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject basicEnemyPrefab;
    [SerializeField]
    private GameObject runnerPrefab;
    [SerializeField]
    private GameObject brutusPrefab;
    
    public float spawnInterval = 3f;
    public float spawnRadius = 10f;  // Distance from spawner to spawn enemies
    
    private float nextSpawnTime;
    private Transform playerTransform;
    private List<GameObject> enemyPool = new List<GameObject>();
    private int poolSize = 50;  // Maximum number of enemies to keep in memory

    void Start()
    {
        // Find player once at start
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        if (playerTransform == null)
            Debug.LogWarning("Player not found! Enemies will spawn around spawner.");
            
        nextSpawnTime = Time.time + spawnInterval;
        
        // Pre-instantiate some enemies
        PrewarmPool();
    }

    void PrewarmPool()
    {
        // Create enemies at a far-off position where they won't be visible
        Vector3 hiddenPosition = new Vector3(1000f, 1000f, 0f);
        transform.position = hiddenPosition;

        // Create some of each type
        for (int i = 0; i < poolSize/2; i++)
            CreateEnemy(basicEnemyPrefab);
        for (int i = 0; i < poolSize/3; i++)
            CreateEnemy(runnerPrefab);
        for (int i = 0; i < poolSize/6; i++)
            CreateEnemy(brutusPrefab);

        // Move spawner back to its original position after pool creation
        transform.position = Vector3.zero;
    }

    GameObject CreateEnemy(GameObject prefab)
    {
        if (prefab == null) return null;
        
        // Create enemy at the spawner's current position (which is far off-screen)
        GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        enemy.SetActive(false);
        enemyPool.Add(enemy);
        return enemy;
    }

    GameObject GetInactiveEnemy(GameObject prefabType)
    {
        // First try to find an inactive enemy of the right type
        foreach (GameObject enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy && enemy.name.Contains(prefabType.name))
                return enemy;
        }
        
        // If none found and pool isn't too big, create new one
        if (enemyPool.Count < poolSize)
            return CreateEnemy(prefabType);
            
        return null;
    }

    void Update()
    {
        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnEnemy()
    {
        // Choose enemy type to spawn
        int randomValue = Random.Range(0, 100);
        GameObject prefabToUse = null;

        if (randomValue < 50)
            prefabToUse = basicEnemyPrefab;
        else if (randomValue < 80)
            prefabToUse = runnerPrefab;
        else
            prefabToUse = brutusPrefab;

        if (prefabToUse == null) return;

        // Get an inactive enemy or create new one
        GameObject enemy = GetInactiveEnemy(prefabToUse);
        if (enemy == null) return;

        // Calculate spawn position
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = playerTransform != null 
            ? playerTransform.position + new Vector3(randomPoint.x, randomPoint.y, 0)
            : transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

        // Reset and activate the enemy
        enemy.transform.position = spawnPosition;
        enemy.transform.rotation = Quaternion.identity;
        
        // Reset enemy components
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        if (enemyComponent != null)
        {
            // The Start method will be called again when SetActive(true)
            enemy.SetActive(true);
        }
    }

    // Optional: Add this to visualize spawn radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
} 