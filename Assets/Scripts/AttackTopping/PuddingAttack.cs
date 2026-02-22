using UnityEngine;
using System.Collections.Generic;

public class PuddingAttack : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject puddingPrefab;

    [Header("Spawn")]
    public int baseCount = 1;          // 基础数量
    public float spawnRadius = 0.6f;

    List<GameObject> puddings = new();

    // ================= 生命周期 =================
    void Start()
    {
        Debug.Log("🍮 PuddingAttack Start");

        if (puddingPrefab == null)
        {
            Debug.LogError("❌ puddingPrefab 没有设置！");
            return;
        }

        Rebuild(); // ⭐ 核心：Start 就生成
    }

    // ================= 对外接口（升级用） =================
    public void Rebuild()
    {
        Clear();

        int count = baseCount + PlayerBattleData.puddingCountLv;
        Vector2 center = transform.position; // ⭐ Player 位置

        for (int i = 0; i < count; i++)
        {
            Vector2 pos =
                center + Random.insideUnitCircle * spawnRadius;

            GameObject p = Instantiate(
                puddingPrefab,
                new Vector3(pos.x, pos.y, 0f),
                Quaternion.identity
            );

            puddings.Add(p);
        }

        Debug.Log($"🍮 Spawned {count} pudding(s)");
    }

    void Clear()
    {
        foreach (var p in puddings)
        {
            if (p != null)
                Destroy(p);
        }
        puddings.Clear();
    }
}
