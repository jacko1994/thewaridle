using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public ObjectPool objectPool;
    public int maxEnemies = 20;
    public float spawnDelay = 2f;
    private float spawnTimer = 0f;
    private int currentEnemyCount = 0;
    private int lastSpawnPointIndex = -1;

    void Start()
    {
        if (objectPool == null)
        {
            Debug.LogError("ObjectPool is not assigned.");
            return;
        }
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (currentEnemyCount < maxEnemies && spawnTimer >= spawnDelay)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Transform spawnPoint = GetUniqueSpawnPoint();
        GameObject enemy = objectPool.GetPooledObject(); // Lấy đối tượng từ object pool
        if (enemy != null)
        {
            enemy.transform.position = spawnPoint.position;
            enemy.SetActive(true); // Kích hoạt đối tượng
            currentEnemyCount++;
        }
    }

    Transform GetUniqueSpawnPoint()
    {
        int spawnPointIndex;
        do
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPointIndex == lastSpawnPointIndex && spawnPoints.Count > 1);

        lastSpawnPointIndex = spawnPointIndex;
        return spawnPoints[spawnPointIndex];
    }

    public void OnEnemyDeath(GameObject enemy)
    {
        objectPool.ReturnToPool(enemy); // Trả đối tượng về object pool
        currentEnemyCount--;
    }
}
