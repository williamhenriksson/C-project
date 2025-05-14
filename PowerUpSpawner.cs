using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject fireRatePowerUpPrefab;
    [SerializeField]
    private GameObject damagePowerUpPrefab;
    [SerializeField]
    private GameObject piercingPowerUpPrefab;
    
    public float spawnInterval = 3f;  // Spawn every 30 seconds
    public float spawnRadius = 10f;    // Distance from spawner to spawn powerups
    
    private float nextSpawnTime;
    private Transform playerTransform;

    void Start()
    {
        // Find player once at start
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        if (playerTransform == null)
            Debug.LogWarning("Player not found! PowerUps will spawn around spawner.");
            
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update()
    {
        // Check if it's time to spawn
        if (Time.time >= nextSpawnTime)
        {
            SpawnPowerUp();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    void SpawnPowerUp()
    {
        // Choose powerup type to spawn
        int randomValue = Random.Range(0, 3);
        GameObject prefabToSpawn = null;

        switch (randomValue)
        {
            case 0:
                prefabToSpawn = fireRatePowerUpPrefab;
                break;
            case 1:
                prefabToSpawn = damagePowerUpPrefab;
                break;
            case 2:
                prefabToSpawn = piercingPowerUpPrefab;
                break;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogWarning("No powerup prefab assigned!");
            return;
        }

        // Calculate spawn position
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = playerTransform != null 
            ? playerTransform.position + new Vector3(randomPoint.x, randomPoint.y, 0)
            : transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);

        // Spawn the powerup
        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }

    // Optional: Add this to visualize spawn radius in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // Different color from EnemySpawner
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
} 