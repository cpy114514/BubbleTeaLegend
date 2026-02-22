using UnityEngine;
using System.Collections;

public class LiquidSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public GameObject milkPrefab;
    public GameObject redTeaPrefab;
    public GameObject greenTeaPrefab;

    [Header("Spawn")]
    public float spawnInterval = 0.02f;
    public int particlesPerPour = 25;

    [Header("Timing")]
    public float waitAfterPour = 0.6f; // ⭐ Inspector 调这个

    public CupController cup;

    bool isPouring = false;

    // =====================
    // Public APIs
    // =====================
    public void PourMilk()
    {
        if (!CanStartPour()) return;
        DrinkRecorder.Instance.currentDrink.hasMilk = true;

        cup.hasMilk = true;
        StartCoroutine(PourRoutine(milkPrefab));
    }

    public void PourRedTea()
    {
        if (!CanStartPour()) return;
        DrinkRecorder.Instance.currentDrink.hasRedTea = true;
        StartCoroutine(PourRoutine(redTeaPrefab));
    }

    public void PourGreenTea()
    {
        if (!CanStartPour()) return;
        DrinkRecorder.Instance.currentDrink.hasGreenTea = true;
        StartCoroutine(PourRoutine(greenTeaPrefab));
    }

    bool CanStartPour()
    {
        if (isPouring) return false;
        if (!cup.CanPour()) return false;
        return true;
    }

    // =====================
    // Core routine
    // =====================
    IEnumerator PourRoutine(GameObject prefab)
    {
        isPouring = true;

        // ⭐ 生成粒子
        for (int i = 0; i < particlesPerPour; i++)
        {
            Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

        // ⭐ 等一下，让它“看起来都进杯了”
        yield return new WaitForSeconds(waitAfterPour);

        isPouring = false;
        cup.OnPoured(); // ⭐ 告诉杯子：这一种倒完了
    }
}
