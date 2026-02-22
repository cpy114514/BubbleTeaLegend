using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Enemy Prefabs")]
    public GameObject weakEnemy;
    public GameObject normalEnemy;
    public GameObject strongEnemy;

    [Header("Spawn Area")]
    public float spawnRadius = 20f;
    public float safeRadius = 5f;

    [Header("Anti Overlap")]
    public float minEnemyDistance = 1.2f;

    [Header("Spawn Rules")]
    public float baseSpawnInterval = 1.5f;
    public int baseMaxEnemyCount = 15;

    [Header("Difficulty")]
    public float difficultyGrowSpeed = 0.05f;

    float timer;
    float survivalTime;

    List<GameObject> aliveEnemies = new List<GameObject>();

    void Update()
    {
        survivalTime += Time.deltaTime;
        timer += Time.deltaTime;

        aliveEnemies.RemoveAll(e => e == null);

        float difficulty = 1f + survivalTime * difficultyGrowSpeed;

        float spawnInterval =
            Mathf.Max(0.3f, baseSpawnInterval / difficulty);

        int maxEnemyCount =
            Mathf.RoundToInt(baseMaxEnemyCount + difficulty * 3);

        if (timer >= spawnInterval && aliveEnemies.Count < maxEnemyCount)
        {
            TrySpawnEnemy(difficulty);
            timer = 0f;
        }
    }

    void TrySpawnEnemy(float difficulty)
    {
        Vector2 spawnPos = GetRandomSpawnPosition();
        if (spawnPos == Vector2.zero) return;

        GameObject prefab = ChooseEnemyByDifficulty(difficulty);
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);
        aliveEnemies.Add(enemy);
    }

    GameObject ChooseEnemyByDifficulty(float difficulty)
    {
        if (difficulty < 2f)
            return weakEnemy;

        if (difficulty < 5f)
            return Random.value < 0.7f ? weakEnemy : normalEnemy;

        return Random.value < 0.6f ? normalEnemy : strongEnemy;
    }

    Vector2 GetRandomSpawnPosition()
    {
        for (int i = 0; i < 15; i++)
        {
            Vector2 pos =
                (Vector2)player.position +
                Random.insideUnitCircle.normalized * spawnRadius;

            if (Vector2.Distance(pos, player.position) < safeRadius)
                continue;

            bool tooClose = false;
            foreach (var e in aliveEnemies)
            {
                if (e == null) continue;
                if (Vector2.Distance(pos, e.transform.position) < minEnemyDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                return pos;
        }

        return Vector2.zero;
    }
}
