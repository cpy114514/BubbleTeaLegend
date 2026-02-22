using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform spawnPoint;

    [Header("Spawn Condition")]
    public int scoreToSpawn = 2000;

    bool hasSpawned = false;

    void Update()
    {
        if (hasSpawned) return;
        if (ScoreManager.Instance == null) return;

        if (ScoreManager.Instance.TotalScore >= scoreToSpawn)
        {
            Spawn();
            hasSpawned = true;
        }
    }

    void Spawn()
    {
        Instantiate(
            bossPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        Debug.Log("🥤 Boss Spawned!");
    }
}