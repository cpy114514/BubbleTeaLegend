using UnityEngine;

public class ExpCupPhysicsFX : MonoBehaviour
{
    public PlayerExpController expController;
    public GameObject expDropPrefab;

    [Header("杯子满经验对应最大球数")]
    public int maxBallCount = 30;

    [Header("生成区域")]
    public Transform spawnPoint; // 杯口上方

    void Start()
    {
        if (expController != null)
            expController.OnExpGained += SpawnDrops;
    }

    void SpawnDrops(int amount)
    {
        int need = expController.expToNext;
        if (need <= 0) return;

        float ratio = (float)amount / need;
        int count = Mathf.RoundToInt(ratio * maxBallCount);

        for (int i = 0; i < count; i++)
        {
            Vector3 pos =
                spawnPoint.position +
                new Vector3(Random.Range(-0.3f, 0.3f), 0, 0);

            GameObject drop =
                Instantiate(expDropPrefab, pos, Quaternion.identity);

            drop.transform.SetParent(spawnPoint.parent);

        }
    }
}
