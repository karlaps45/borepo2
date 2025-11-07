using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;

    private int currentWave = 1;
    private float timeSinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Start()
    {
        StartWave();
    }

    private void Update()
    {
        if (isSpawning)
        {
            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
            {
                SpawnEnemy();
                enemiesLeftToSpawn--;
                timeSinceLastSpawn = 0f;

                if (enemiesLeftToSpawn <= 0)
                {
                    isSpawning = false;
                }
            }
        }

        // Start a new wave after all enemies are "gone"
        if (!isSpawning && enemiesAlive <= 0)
        {
            Invoke(nameof(StartWave), timeBetweenWaves);
        }
    }

    private void StartWave()
    {
        enemiesLeftToSpawn = EnemiesPerWave();
        isSpawning = true;
        Debug.Log($"Starting Wave {currentWave} with {enemiesLeftToSpawn} enemies!");
        currentWave++;
    }

    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("No enemy prefabs assigned!");
            return;
        }

        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned!");
            return;
        }

        GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
        enemiesAlive++;

        // Automatically decrease enemiesAlive after a few seconds (for testing)
        Destroy(enemy, 10f);
        Invoke(nameof(EnemyDied), 10f);

        Debug.Log($"Spawned {enemy.name}");
    }

    private void EnemyDied()
    {
        enemiesAlive--;
    }
}