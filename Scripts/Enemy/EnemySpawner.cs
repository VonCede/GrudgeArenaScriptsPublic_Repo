using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemyPrefabs;  // The list of enemy prefabs to spawn.
    public LayerMask groundLayer = 1 << 12;      // The layer to check for ground.
    public float groundCheckDistance = 1.0f; // How far down to look for ground.

    [Header("Spawn Settings")]
    public float minSpawnRadius = 25.0f;   // Minimum distance from the player to spawn enemies.
    public float maxSpawnRadius = 35.0f;  // Maximum distance from the player to spawn enemies.
    public float spawnInterval = 2.0f;    // Time between enemy spawns.
    public float startTime = 0.0f;        // Time to start spawning enemies.
    public float endTime = 60.0f;         // Time to stop spawning enemies.

    private Transform player; // Reference to the player's transform.
    private float nextSpawnTime; // Time for the next enemy spawn.
    private RaycastHit hit; // Used to check for ground.
    
    private void Start()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogError("No enemy prefabs assigned at enemySpawner in " + gameObject.name + "!");
            return;
        }
        
        player = GameObject.FindGameObjectWithTag("Player").transform; // You can tag your player object.
        nextSpawnTime = startTime;
    }

    private void Update()
    {
        if(PlayState.Instance.IsGameOver)
            return;
        if (Time.time >= nextSpawnTime && Time.time <= endTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        // Keep trying to find ground for a new random position.
        int maxAttempts = 10; // To avoid infinite loops.
        int attemptCount = 0;

        while (attemptCount < maxAttempts)
        {
            // Calculate a random distance from the player within the specified range.
            float spawnDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
            Vector3 randomOffset = Random.insideUnitCircle.normalized * spawnDistance;
            Vector3 spawnPosition = player.position + new Vector3(randomOffset.x, 0.0f, randomOffset.y);

            // Check if there's ground at the spawn position.
            if (Physics.Raycast(spawnPosition, Vector3.down, out hit, groundCheckDistance, groundLayer))
            {
                // Ground is found, choose a random enemy prefab to spawn.
                GameObject randomEnemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

                // Spawn the enemy at the determined position.
                Instantiate(randomEnemyPrefab, spawnPosition, Quaternion.identity);
                return; // Exit the loop as we've successfully spawned an enemy.
            }
            else
            {
                // If ground is not found, increment attemptCount and try again.
                attemptCount++;
            }
        }

        // If ground isn't found after multiple attempts, log a warning message.
        Debug.LogWarning("Couldn't find ground for enemy spawn after multiple attempts.");
    }

}
