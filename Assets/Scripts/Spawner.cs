using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Auto,
        Manual
    }

    public List<Transform> spawnPoints;
    public ObjectPool objectPool;
    public int maxObjects = 20;
    public float spawnDelay = 2f;
    public SpawnMode spawnMode = SpawnMode.Auto;

    private float spawnTimer = 0f;
    private int currentObjectCount = 0;
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
        if (spawnMode == SpawnMode.Auto)
        {
            spawnTimer += Time.deltaTime;

            if (currentObjectCount < maxObjects && spawnTimer >= spawnDelay)
            {
                SpawnObject();
                spawnTimer = 0f;
            }
        }
    }

    public void SpawnObjectManual()
    {
        if (spawnMode == SpawnMode.Manual)
        {
            SpawnObject();
        }
    }
    public void ResetCurrentCount()
    {
        currentObjectCount = 0;
    }
    private void SpawnObject()
    {
        Transform spawnPoint = GetUniqueSpawnPoint();
        GameObject obj = objectPool.GetPooledObject();
        if (obj != null)
        {
            obj.transform.position = spawnPoint.position;
            obj.SetActive(true);
            currentObjectCount++;
            Debug.Log($"Spawned Object at {spawnPoint.position}. Current Object Count: {currentObjectCount}");

            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null)
            {
                entity.SetObjectPool(objectPool);
            }
        }
    }

    private Transform GetUniqueSpawnPoint()
    {
        int spawnPointIndex;
        do
        {
            spawnPointIndex = Random.Range(0, spawnPoints.Count);
        } while (spawnPointIndex == lastSpawnPointIndex && spawnPoints.Count > 1);

        lastSpawnPointIndex = spawnPointIndex;
        return spawnPoints[spawnPointIndex];
    }

    public void OnObjectDeactivation(GameObject obj)
    {
        objectPool.ReturnToPool(obj);
        currentObjectCount--;
    }
}
