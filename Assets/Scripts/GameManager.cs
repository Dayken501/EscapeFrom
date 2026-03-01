using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Настройки игры")]
    public float gameDuration = 600f;
    public GameObject enemyPrefab;
    public Transform player;

    [Header("Настройки спавна")]
    public float spawnInterval = 5f;
    public float spawnRadius = 10f;

    private float timeRemaining;
    private float lastSpawnTime;
    private int currentEnemyCount;
    public int maxEnemies = 50;

    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeRemaining = gameDuration;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        UIManager.Instance.UpdateTimer(timeRemaining);

        if (timeRemaining <= 0)
        {
            Win();
            return;
        }

        if (Time.time - lastSpawnTime >= spawnInterval)
        {
            lastSpawnTime = Time.time;
            SpawnEnemies();
        }
    }

    void SpawnEnemies()
    {
        if (currentEnemyCount >= maxEnemies) return;

        int count = GetSpawnCount();
        for (int i = 0; i < count; i++)
        {
            if (currentEnemyCount >= maxEnemies) break;

            Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPos = new Vector3(
                player.position.x + randomCircle.x,
                0,
                player.position.z + randomCircle.y
            );

            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            currentEnemyCount++;
        }
    }

    int GetSpawnCount()
    {
        return 10;
    }

    public void EnemyDied()
    {
        currentEnemyCount--;
    }

    void Win()
    {
        UIManager.Instance.ShowWin();
    }
}