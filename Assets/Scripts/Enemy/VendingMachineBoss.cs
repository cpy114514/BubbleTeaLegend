using UnityEngine;

public class BossCircleSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;

    [Header("Spawn Settings")]
    public float spawnInterval = 2f;
    public int spawnCount = 3;

    [Header("Circle Settings")]
    public float circleRadius = 2.5f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnCircle();
            timer = 0f;
        }
    }

    void SpawnCircle()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            float angle = (360f / spawnCount) * i;
            float rad = angle * Mathf.Deg2Rad;

            Vector2 offset =
                new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * circleRadius;

            Vector2 spawnPos =
                (Vector2)transform.position + offset;

            GameObject enemy =
                Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemy.AddComponent<CircleAroundBoss>()
                .Init(transform);
        }
    }
}