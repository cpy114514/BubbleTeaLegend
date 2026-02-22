using UnityEngine;
using System.Collections;

public class ToppingSpawner : MonoBehaviour
{
    [Header("Spawn Points")]
    public Transform milkSpawnPoint;
    public Transform teaSpawnPoint;

    [Header("Default Spawn Settings")]
    public float spawnInterval = 0.05f;
    public float randomXForce = 0.6f;

    public CupController cup;

    bool isSpawning = false;

    // ⭐ 新接口：数量由外部传入
    public void SpawnTopping(GameObject prefab, int count)
    {
        if (isSpawning) return;
        if (cup.state != CupController.CupState.Topping) return;

        Transform spawnPoint = cup.hasMilk
            ? milkSpawnPoint
            : teaSpawnPoint;

        StartCoroutine(SpawnRoutine(prefab, spawnPoint, count));
    }

    IEnumerator SpawnRoutine(GameObject prefab, Transform spawnPoint, int count)
    {
        isSpawning = true;

        for (int i = 0; i < count; i++)
        {
            GameObject topping = Instantiate(
                prefab,
                spawnPoint.position,
                Quaternion.identity
            );

            Rigidbody2D rb = topping.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float randomX = Random.Range(-randomXForce, randomXForce);
                rb.AddForce(new Vector2(randomX, 0f), ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(spawnInterval);
        }

        isSpawning = false;
    }
}
